﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Core.Testing;
using Azure.Storage.Queues.Models;
using Azure.Storage.Sas;
using Azure.Storage.Test;
using Azure.Storage.Test.Shared;

namespace Azure.Storage.Queues.Tests
{
    public class QueueTestBase : StorageTestBase
    {
        public string GetNewQueueName() => $"test-queue-{Recording.Random.NewGuid()}";
        public string GetNewMessageId() => $"test-message-{Recording.Random.NewGuid()}";

        protected string SecondaryStorageTenantPrimaryHost() =>
            new Uri(TestConfigSecondary.QueueServiceEndpoint).Host;

        protected string SecondaryStorageTenantSecondaryHost() =>
            new Uri(TestConfigSecondary.QueueServiceSecondaryEndpoint).Host;

        public QueueTestBase(bool async) : this(async, null) { }

        public QueueTestBase(bool async, RecordedTestMode? mode = null)
            : base(async, mode)
        {
        }

        public QueueClientOptions GetOptions()
        {
            var options = new QueueClientOptions
            {
                Diagnostics = { IsLoggingEnabled = true },
                Retry =
                {
                    Mode = RetryMode.Exponential,
                    MaxRetries = Azure.Storage.Constants.MaxReliabilityRetries,
                    Delay = TimeSpan.FromSeconds(Mode == RecordedTestMode.Playback ? 0.01 : 0.5),
                    MaxDelay = TimeSpan.FromSeconds(Mode == RecordedTestMode.Playback ? 0.1 : 10)
                }
            };
            if (Mode != RecordedTestMode.Live)
            {
                options.AddPolicy(new RecordedClientRequestIdPolicy(Recording), HttpPipelinePosition.PerCall);
            }

            return Recording.InstrumentClientOptions(options);
        }

        public QueueServiceClient GetServiceClient_SharedKey()
            => InstrumentClient(
                new QueueServiceClient(
                    new Uri(TestConfigDefault.QueueServiceEndpoint),
                    new StorageSharedKeyCredential(
                        TestConfigDefault.AccountName,
                        TestConfigDefault.AccountKey),
                    GetOptions()));

        public QueueServiceClient GetServiceClient_AccountSas(StorageSharedKeyCredential sharedKeyCredentials = default, SasQueryParameters sasCredentials = default)
            => InstrumentClient(
                new QueueServiceClient(
                    new Uri($"{TestConfigDefault.QueueServiceEndpoint}?{sasCredentials ?? GetNewAccountSasCredentials(sharedKeyCredentials ?? GetNewSharedKeyCredentials())}"),
                    GetOptions()));

        public QueueServiceClient GetServiceClient_QueueServiceSas(string queueName, StorageSharedKeyCredential sharedKeyCredentials = default, SasQueryParameters sasCredentials = default)
            => InstrumentClient(
                new QueueServiceClient(
                    new Uri($"{TestConfigDefault.QueueServiceEndpoint}?{sasCredentials ?? GetNewQueueServiceSasCredentials(queueName, sharedKeyCredentials ?? GetNewSharedKeyCredentials())}"),
                    GetOptions()));

        public QueueServiceClient GetServiceClient_OauthAccount() =>
            GetServiceClientFromOauthConfig(TestConfigOAuth);

        public QueueServiceClient GetServiceClient_SecondaryAccount_ReadEnabledOnRetry(int numberOfReadFailuresToSimulate, out TestExceptionPolicy testExceptionPolicy, bool simulate404 = false)
=> GetSecondaryReadServiceClient(TestConfigSecondary, numberOfReadFailuresToSimulate, out testExceptionPolicy, simulate404);

        public QueueClient GetQueueClient_SecondaryAccount_ReadEnabledOnRetry(int numberOfReadFailuresToSimulate, out TestExceptionPolicy testExceptionPolicy, bool simulate404 = false)
=> GetSecondaryReadQueueClient(TestConfigSecondary, numberOfReadFailuresToSimulate, out testExceptionPolicy, simulate404);

        private QueueServiceClient GetSecondaryReadServiceClient(TenantConfiguration config, int numberOfReadFailuresToSimulate, out TestExceptionPolicy testExceptionPolicy, bool simulate404 = false, List<RequestMethod> enabledRequestMethods = null)
        {
            QueueClientOptions options = getSecondaryStorageOptions(config, out testExceptionPolicy, numberOfReadFailuresToSimulate, simulate404, enabledRequestMethods);

            return InstrumentClient(
                 new QueueServiceClient(
                    new Uri(config.QueueServiceEndpoint),
                    new StorageSharedKeyCredential(config.AccountName, config.AccountKey),
                    options));
        }

        private QueueClient GetSecondaryReadQueueClient(TenantConfiguration config, int numberOfReadFailuresToSimulate, out TestExceptionPolicy testExceptionPolicy, bool simulate404 = false, List<RequestMethod> enabledRequestMethods = null)
        {
            QueueClientOptions options = getSecondaryStorageOptions(config, out testExceptionPolicy, numberOfReadFailuresToSimulate, simulate404, enabledRequestMethods);

            return InstrumentClient(
                 new QueueClient(
                    new Uri(config.QueueServiceEndpoint).AppendToPath(GetNewQueueName()),
                    new StorageSharedKeyCredential(config.AccountName, config.AccountKey),
                    options));
        }

        private QueueClientOptions getSecondaryStorageOptions(TenantConfiguration config, out TestExceptionPolicy testExceptionPolicy, int numberOfReadFailuresToSimulate = 1, bool simulate404 = false, List<RequestMethod> enabledRequestMethods = null)
        {
            QueueClientOptions options = GetOptions();
            options.GeoRedundantSecondaryUri = new Uri(config.QueueServiceSecondaryEndpoint);
            options.Retry.MaxRetries = 4;
            testExceptionPolicy = new TestExceptionPolicy(numberOfReadFailuresToSimulate, options.GeoRedundantSecondaryUri, simulate404, enabledRequestMethods);
            options.AddPolicy(testExceptionPolicy, HttpPipelinePosition.PerRetry);
            return options;
        }

        private QueueServiceClient GetServiceClientFromOauthConfig(TenantConfiguration config) =>
            InstrumentClient(
                new QueueServiceClient(
                    new Uri(config.QueueServiceEndpoint),
                    GetOAuthCredential(config),
                    GetOptions()));

        public async Task<DisposingQueue> GetTestQueueAsync(QueueServiceClient service = default, IDictionary<string, string> metadata = default)
        {
            service ??= GetServiceClient_SharedKey();
            metadata ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            QueueClient queue = InstrumentClient(service.GetQueueClient(GetNewQueueName()));
            return await DisposingQueue.CreateAsync(queue, metadata);
        }

        public StorageSharedKeyCredential GetNewSharedKeyCredentials()
            => new StorageSharedKeyCredential(
                TestConfigDefault.AccountName,
                TestConfigDefault.AccountKey);

        public SasQueryParameters GetNewAccountSasCredentials(StorageSharedKeyCredential sharedKeyCredentials = default)
        {
            var builder = new AccountSasBuilder
            {
                Protocol = SasProtocol.None,
                Services = AccountSasServices.Queues,
                ResourceTypes = AccountSasResourceTypes.Container,
                StartsOn = Recording.UtcNow.AddHours(-1),
                ExpiresOn = Recording.UtcNow.AddHours(+1),
                IPRange = new SasIPRange(IPAddress.None, IPAddress.None)
            };
            builder.SetPermissions(
                AccountSasPermissions.Read |
                AccountSasPermissions.Write |
                AccountSasPermissions.Update |
                AccountSasPermissions.Process |
                AccountSasPermissions.Add |
                AccountSasPermissions.Delete |
                AccountSasPermissions.List);
            return builder.ToSasQueryParameters(sharedKeyCredentials);
        }

        public SasQueryParameters GetNewQueueServiceSasCredentials(string queueName, StorageSharedKeyCredential sharedKeyCredentials = default)
        {
            var builder = new QueueSasBuilder
            {
                QueueName = queueName,
                Protocol = SasProtocol.None,
                StartsOn = Recording.UtcNow.AddHours(-1),
                ExpiresOn = Recording.UtcNow.AddHours(+1),
                IPRange = new SasIPRange(IPAddress.None, IPAddress.None)
            };
            builder.SetPermissions(QueueAccountSasPermissions.Read | QueueAccountSasPermissions.Update | QueueAccountSasPermissions.Process | QueueAccountSasPermissions.Add);
            return builder.ToSasQueryParameters(sharedKeyCredentials ?? GetNewSharedKeyCredentials());
        }

        public class DisposingQueue : IAsyncDisposable
        {
            public QueueClient Queue { get; private set; }

            public static async Task<DisposingQueue> CreateAsync(QueueClient queue, IDictionary<string, string> metadata)
            {
                await queue.CreateAsync(metadata: metadata);
                return new DisposingQueue(queue);
            }

            private DisposingQueue(QueueClient queue)
            {
                Queue = queue;
            }

            public async ValueTask DisposeAsync()
            {
                if (Queue != null)
                {
                    try
                    {
                        await Queue.DeleteAsync();
                        Queue = null;
                    }
                    catch
                    {
                        // swallow the exception to avoid hiding another test failure
                    }
                }
            }
        }

        public QueueSignedIdentifier[] BuildSignedIdentifiers() =>
            new[]
            {
                new QueueSignedIdentifier
                {
                    Id = GetNewString(),
                    AccessPolicy =
                        new QueueAccessPolicy
                        {
                            StartsOn =  Recording.UtcNow.AddHours(-1),
                            ExpiresOn =  Recording.UtcNow.AddHours(1),
                            Permissions = "raup"
                        }
                }
            };
    }
}
