﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Storage.Files.DataLake.Models
{
    internal class GetPathsAsyncCollection : StorageCollectionEnumerator<PathItem>
    {
        private readonly FileSystemClient _client;
        private readonly GetPathsOptions? _options;

        public GetPathsAsyncCollection(
            FileSystemClient client,
            GetPathsOptions? options)
        {
            _client = client;
            _options = options;
        }

        public override async ValueTask<Page<PathItem>> GetNextPageAsync(
            string continuationToken,
            int? pageSizeHint,
            bool isAsync,
            CancellationToken cancellationToken)
        {
            Task<Response<PathSegment>> task = _client.ListPathsInternal(
                _options,
                continuationToken,
                pageSizeHint,
                isAsync,
                cancellationToken);
            Response<PathSegment> response = isAsync ?
                await task.ConfigureAwait(false) :
                task.EnsureCompleted();

            return Page<PathItem>.FromValues(
                response.Value.Paths.ToArray(),
                response.Value.Continuation,
                response.GetRawResponse());
        }
    }
}
