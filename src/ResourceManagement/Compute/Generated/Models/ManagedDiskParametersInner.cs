// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.Compute.Fluent.Models
{
    using Microsoft.Azure;
    using Microsoft.Azure.Management;
    using Microsoft.Azure.Management.Compute;
    using Microsoft.Azure.Management.Compute.Fluent;
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// The parameters of a managed disk.
    /// </summary>
    public partial class ManagedDiskParametersInner : Microsoft.Azure.Management.ResourceManager.Fluent.SubResource
    {
        /// <summary>
        /// Initializes a new instance of the ManagedDiskParametersInner class.
        /// </summary>
        public ManagedDiskParametersInner()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ManagedDiskParametersInner class.
        /// </summary>
        /// <param name="storageAccountType">The Storage Account type. Possible
        /// values include: 'Standard_LRS', 'Premium_LRS'</param>
        public ManagedDiskParametersInner(string id = default(string), StorageAccountTypes? storageAccountType = default(StorageAccountTypes?))
            : base(id)
        {
            StorageAccountType = storageAccountType;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the Storage Account type. Possible values include:
        /// 'Standard_LRS', 'Premium_LRS'
        /// </summary>
        [JsonProperty(PropertyName = "storageAccountType")]
        public StorageAccountTypes? StorageAccountType { get; set; }

    }
}