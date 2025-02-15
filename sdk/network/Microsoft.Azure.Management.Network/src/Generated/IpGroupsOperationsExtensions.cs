// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Network
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for IpGroupsOperations.
    /// </summary>
    public static partial class IpGroupsOperationsExtensions
    {
            /// <summary>
            /// Gets the specified ipGroups.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='expand'>
            /// Expands resourceIds (of Firewalls/Network Security Groups etc.) back
            /// referenced by the IpGroups resource.
            /// </param>
            public static IpGroup Get(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, string expand = default(string))
            {
                return operations.GetAsync(resourceGroupName, ipGroupsName, expand).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets the specified ipGroups.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='expand'>
            /// Expands resourceIds (of Firewalls/Network Security Groups etc.) back
            /// referenced by the IpGroups resource.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IpGroup> GetAsync(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, string expand = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWithHttpMessagesAsync(resourceGroupName, ipGroupsName, expand, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Creates or updates an ipGroups in a specified resource group.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='parameters'>
            /// Parameters supplied to the create or update IpGroups operation.
            /// </param>
            public static IpGroup CreateOrUpdate(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, IpGroup parameters)
            {
                return operations.CreateOrUpdateAsync(resourceGroupName, ipGroupsName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Creates or updates an ipGroups in a specified resource group.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='parameters'>
            /// Parameters supplied to the create or update IpGroups operation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IpGroup> CreateOrUpdateAsync(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, IpGroup parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateOrUpdateWithHttpMessagesAsync(resourceGroupName, ipGroupsName, parameters, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Updates an IpGroups
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='parameters'>
            /// Parameters supplied to the update ipGroups operation.
            /// </param>
            public static IpGroup UpdateGroups(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, TagsObject parameters)
            {
                return operations.UpdateGroupsAsync(resourceGroupName, ipGroupsName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Updates an IpGroups
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='parameters'>
            /// Parameters supplied to the update ipGroups operation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IpGroup> UpdateGroupsAsync(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, TagsObject parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateGroupsWithHttpMessagesAsync(resourceGroupName, ipGroupsName, parameters, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes the specified ipGroups.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            public static void Delete(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName)
            {
                operations.DeleteAsync(resourceGroupName, ipGroupsName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes the specified ipGroups.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteAsync(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteWithHttpMessagesAsync(resourceGroupName, ipGroupsName, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Gets all IpGroups in a resource group.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            public static IPage<IpGroup> ListByResourceGroup(this IIpGroupsOperations operations, string resourceGroupName)
            {
                return operations.ListByResourceGroupAsync(resourceGroupName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets all IpGroups in a resource group.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IPage<IpGroup>> ListByResourceGroupAsync(this IIpGroupsOperations operations, string resourceGroupName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByResourceGroupWithHttpMessagesAsync(resourceGroupName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Gets all IpGroups in a subscription.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IPage<IpGroup> List(this IIpGroupsOperations operations)
            {
                return operations.ListAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets all IpGroups in a subscription.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IPage<IpGroup>> ListAsync(this IIpGroupsOperations operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Creates or updates an ipGroups in a specified resource group.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='parameters'>
            /// Parameters supplied to the create or update IpGroups operation.
            /// </param>
            public static IpGroup BeginCreateOrUpdate(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, IpGroup parameters)
            {
                return operations.BeginCreateOrUpdateAsync(resourceGroupName, ipGroupsName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Creates or updates an ipGroups in a specified resource group.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='parameters'>
            /// Parameters supplied to the create or update IpGroups operation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IpGroup> BeginCreateOrUpdateAsync(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, IpGroup parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.BeginCreateOrUpdateWithHttpMessagesAsync(resourceGroupName, ipGroupsName, parameters, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes the specified ipGroups.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            public static void BeginDelete(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName)
            {
                operations.BeginDeleteAsync(resourceGroupName, ipGroupsName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes the specified ipGroups.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='ipGroupsName'>
            /// The name of the ipGroups.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BeginDeleteAsync(this IIpGroupsOperations operations, string resourceGroupName, string ipGroupsName, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BeginDeleteWithHttpMessagesAsync(resourceGroupName, ipGroupsName, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Gets all IpGroups in a resource group.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='nextPageLink'>
            /// The NextLink from the previous successful call to List operation.
            /// </param>
            public static IPage<IpGroup> ListByResourceGroupNext(this IIpGroupsOperations operations, string nextPageLink)
            {
                return operations.ListByResourceGroupNextAsync(nextPageLink).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets all IpGroups in a resource group.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='nextPageLink'>
            /// The NextLink from the previous successful call to List operation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IPage<IpGroup>> ListByResourceGroupNextAsync(this IIpGroupsOperations operations, string nextPageLink, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByResourceGroupNextWithHttpMessagesAsync(nextPageLink, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Gets all IpGroups in a subscription.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='nextPageLink'>
            /// The NextLink from the previous successful call to List operation.
            /// </param>
            public static IPage<IpGroup> ListNext(this IIpGroupsOperations operations, string nextPageLink)
            {
                return operations.ListNextAsync(nextPageLink).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets all IpGroups in a subscription.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='nextPageLink'>
            /// The NextLink from the previous successful call to List operation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IPage<IpGroup>> ListNextAsync(this IIpGroupsOperations operations, string nextPageLink, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListNextWithHttpMessagesAsync(nextPageLink, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
