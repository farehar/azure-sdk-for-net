﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.Tracing;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Testing;
using Azure.Identity.Tests.Mock;
using Microsoft.Identity.Client;
using NUnit.Framework;

namespace Azure.Identity.Tests
{
    // Avoid running these tests in parallel with anything else that's sharing the event source
    [NonParallelizable]
    public class AzureIdentityEventSourceTests : ClientTestBase
    {
        private const int GetTokenEvent = 1;
        private const int GetTokenSucceededEvent = 2;
        private const int GetTokenFailedEvent = 3;

        private TestEventListener _listener;

        public AzureIdentityEventSourceTests(bool isAsync) : base(isAsync)
        {
        }

        [SetUp]
        public void Setup()
        {
            _listener = new TestEventListener();
            _listener.EnableEvents(AzureIdentityEventSource.Singleton, EventLevel.Verbose);
        }

        [Test]
        public void MatchesNameAndGuid()
        {
            // Arrange & Act
            Type eventSourceType = typeof(AzureIdentityEventSource);

            // Assert
            Assert.NotNull(eventSourceType);
            Assert.AreEqual("Azure-Identity", EventSource.GetName(eventSourceType));
            Assert.AreEqual(Guid.Parse("50c8e6e8-b11b-5998-63f4-3944e66d312a"), EventSource.GetGuid(eventSourceType));
            Assert.IsNotEmpty(EventSource.GenerateManifest(eventSourceType, "assemblyPathToIncludeInManifest"));
        }

        [Test]
        public async Task ValidateClientSecretCredentialSucceededEvents()
        {
            var mockAadClient = new MockAadIdentityClient(() => new AccessToken(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow.AddMinutes(10)));

            var credential = InstrumentClient(new ClientSecretCredential(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), CredentialPipeline.GetInstance(null), mockAadClient));

            var method = "Azure.Identity.ClientSecretCredential.GetToken";

            await AssertCredentialGetTokenSucceededAsync(credential, method);
        }

        [Test]
        public async Task ValidateClientCertificateCredentialSucceededEvents()
        {
            var mockAadClient = new MockAadIdentityClient(() => new AccessToken(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow.AddMinutes(10)));

            var credential = InstrumentClient(new ClientCertificateCredential(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new X509Certificate2(), CredentialPipeline.GetInstance(null), mockAadClient));

            var method = "Azure.Identity.ClientCertificateCredential.GetToken";

            await AssertCredentialGetTokenSucceededAsync(credential, method);
        }

        [Test]
        public async Task ValidateDeviceCodeCredentialSucceededEvents()
        {
            var mockMsalClient = new MockMsalPublicClient() { DeviceCodeAuthFactory = (_) => { return AuthenticationResultFactory.Create(accessToken: Guid.NewGuid().ToString(), expiresOn: DateTimeOffset.UtcNow.AddMinutes(10)); } };

            var credential = InstrumentClient(new DeviceCodeCredential((_, __) => { return Task.CompletedTask; }, Guid.NewGuid().ToString(), CredentialPipeline.GetInstance(null), mockMsalClient));

            var method = "Azure.Identity.DeviceCodeCredential.GetToken";

            await AssertCredentialGetTokenSucceededAsync(credential, method);
        }

        [Test]
        public async Task ValidateInteractiveBrowserCredentialSucceededEvents()
        {
            var mockMsalClient = new MockMsalPublicClient() { InteractiveAuthFactory = (_) => { return AuthenticationResultFactory.Create(accessToken: Guid.NewGuid().ToString(), expiresOn: DateTimeOffset.UtcNow.AddMinutes(10)); } };

            var credential = InstrumentClient(new InteractiveBrowserCredential(CredentialPipeline.GetInstance(null), mockMsalClient));

            var method = "Azure.Identity.InteractiveBrowserCredential.GetToken";

            await AssertCredentialGetTokenSucceededAsync(credential, method);
        }

        [Test]
        public async Task ValidateSharedTokenCacheCredentialSucceededEvents()
        {
            var mockMsalClient = new MockMsalPublicClient
            {
                Accounts = new IAccount[] { new MockAccount("mockuser@mockdomain.com") },
                SilentAuthFactory = (_) => { return AuthenticationResultFactory.Create(accessToken: Guid.NewGuid().ToString(), expiresOn: DateTimeOffset.UtcNow.AddMinutes(10)); }
            };

            var credential = InstrumentClient(new SharedTokenCacheCredential("mockuser@mockdomain.com", CredentialPipeline.GetInstance(null), mockMsalClient));

            var method = "Azure.Identity.SharedTokenCacheCredential.GetToken";

            await AssertCredentialGetTokenSucceededAsync(credential, method);
        }

        [Test]
        public async Task ValidateClientSecretCredentialFailedEvents()
        {
            var expExMessage = Guid.NewGuid().ToString();

            var mockAadClient = new MockAadIdentityClient(() => throw new MockClientException(expExMessage));

            var credential = InstrumentClient(new ClientSecretCredential(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), CredentialPipeline.GetInstance(null), mockAadClient));

            var method = "Azure.Identity.ClientSecretCredential.GetToken";

            await AssertCredentialGetTokenFailedAsync(credential, method, expExMessage);
        }

        [Test]
        public async Task ValidateClientCertificateCrededntialFailedEvents()
        {
            var expExMessage = Guid.NewGuid().ToString();

            var mockAadClient = new MockAadIdentityClient(() => throw new MockClientException(expExMessage));

            var credential = InstrumentClient(new ClientCertificateCredential(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new X509Certificate2(), CredentialPipeline.GetInstance(null), mockAadClient));

            var method = "Azure.Identity.ClientCertificateCredential.GetToken";

            await AssertCredentialGetTokenFailedAsync(credential, method, expExMessage);
        }

        [Test]
        public async Task ValidateDeviceCodeCredentialFailedEvents()
        {
            var expExMessage = Guid.NewGuid().ToString();

            var mockMsalClient = new MockMsalPublicClient() { DeviceCodeAuthFactory = (_) => throw new MockClientException(expExMessage) };

            var credential = InstrumentClient(new DeviceCodeCredential((_, __) => { return Task.CompletedTask; }, Guid.NewGuid().ToString(), CredentialPipeline.GetInstance(null), mockMsalClient));

            var method = "Azure.Identity.DeviceCodeCredential.GetToken";

            await AssertCredentialGetTokenFailedAsync(credential, method, expExMessage);
        }

        [Test]
        public async Task ValidateInteractiveBrowserCredentialFailedEvents()
        {
            var expExMessage = Guid.NewGuid().ToString();

            var mockMsalClient = new MockMsalPublicClient() { InteractiveAuthFactory = (_) => throw new MockClientException(expExMessage) };

            var credential = InstrumentClient(new InteractiveBrowserCredential(CredentialPipeline.GetInstance(null), mockMsalClient));

            var method = "Azure.Identity.InteractiveBrowserCredential.GetToken";

            await AssertCredentialGetTokenFailedAsync(credential, method, expExMessage);
        }

        [Test]
        public async Task ValidateSharedTokenCacheCredentialFailedEvents()
        {
            var expExMessage = Guid.NewGuid().ToString();

            var mockMsalClient = new MockMsalPublicClient
            {
                Accounts = new IAccount[] { new MockAccount("mockuser@mockdomain.com") },
                SilentAuthFactory = (_) => throw new MockClientException(expExMessage)
            };

            var credential = InstrumentClient(new SharedTokenCacheCredential("mockuser@mockdomain.com", CredentialPipeline.GetInstance(null), mockMsalClient));

            var method = "Azure.Identity.SharedTokenCacheCredential.GetToken";

            await AssertCredentialGetTokenFailedAsync(credential, method, expExMessage);
        }

        private async Task AssertCredentialGetTokenSucceededAsync(TokenCredential credential, string method)
        {
            var expParentRequestId = Guid.NewGuid().ToString();

            var expScopes = new string[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            await credential.GetTokenAsync(new TokenRequestContext(expScopes, expParentRequestId), default);

            EventWrittenEventArgs e = _listener.SingleEventById(GetTokenEvent);

            Assert.AreEqual(EventLevel.Informational, e.Level);
            Assert.AreEqual("GetToken", e.EventName);
            Assert.AreEqual(method, e.GetProperty<string>("method"));
            Assert.AreEqual($"[ {string.Join(", ", expScopes)} ]", e.GetProperty<string>("scopes"));
            Assert.AreEqual(expParentRequestId, e.GetProperty<string>("parentRequestId"));

            e = _listener.SingleEventById(GetTokenSucceededEvent);

            Assert.AreEqual(EventLevel.Informational, e.Level);
            Assert.AreEqual("GetTokenSucceeded", e.EventName);
            Assert.AreEqual(method, e.GetProperty<string>("method"));
            Assert.AreEqual($"[ {string.Join(", ", expScopes)} ]", e.GetProperty<string>("scopes"));
            Assert.AreEqual(expParentRequestId, e.GetProperty<string>("parentRequestId"));
            Assert.IsTrue(DateTimeOffset.TryParse(e.GetProperty<string>("expiresOn"), out _));
        }

        private async Task AssertCredentialGetTokenFailedAsync(TokenCredential credential, string method, string expExMessage)
        {
            var expParentRequestId = Guid.NewGuid().ToString();

            var expScopes = new string[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

            Assert.CatchAsync<AuthenticationFailedException>(async () => await credential.GetTokenAsync(new TokenRequestContext(expScopes, expParentRequestId), default));

            EventWrittenEventArgs e = _listener.SingleEventById(GetTokenEvent);

            Assert.AreEqual(EventLevel.Informational, e.Level);
            Assert.AreEqual("GetToken", e.EventName);
            Assert.AreEqual(method, e.GetProperty<string>("method"));
            Assert.AreEqual($"[ {string.Join(", ", expScopes)} ]", e.GetProperty<string>("scopes"));
            Assert.AreEqual(expParentRequestId, e.GetProperty<string>("parentRequestId"));

            e = _listener.SingleEventById(GetTokenFailedEvent);

            Assert.AreEqual(EventLevel.Informational, e.Level);
            Assert.AreEqual("GetTokenFailed", e.EventName);
            Assert.AreEqual(method, e.GetProperty<string>("method"));
            Assert.AreEqual($"[ {string.Join(", ", expScopes)} ]", e.GetProperty<string>("scopes"));
            Assert.AreEqual(expParentRequestId, e.GetProperty<string>("parentRequestId"));
            Assert.IsTrue(e.GetProperty<string>("exception").Contains(expExMessage));

            await Task.CompletedTask;
        }
    }
}
