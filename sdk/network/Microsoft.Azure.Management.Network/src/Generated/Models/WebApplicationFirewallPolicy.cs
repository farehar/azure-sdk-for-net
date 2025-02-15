// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Network.Models
{
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines web application firewall policy.
    /// </summary>
    [Rest.Serialization.JsonTransformation]
    public partial class WebApplicationFirewallPolicy : Resource
    {
        /// <summary>
        /// Initializes a new instance of the WebApplicationFirewallPolicy
        /// class.
        /// </summary>
        public WebApplicationFirewallPolicy()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the WebApplicationFirewallPolicy
        /// class.
        /// </summary>
        /// <param name="managedRules">Describes the managedRules
        /// structure</param>
        /// <param name="id">Resource ID.</param>
        /// <param name="name">Resource name.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="location">Resource location.</param>
        /// <param name="tags">Resource tags.</param>
        /// <param name="policySettings">Describes policySettings for
        /// policy.</param>
        /// <param name="customRules">Describes custom rules inside the
        /// policy.</param>
        /// <param name="applicationGateways">A collection of references to
        /// application gateways.</param>
        /// <param name="provisioningState">The provisioning state of the web
        /// application firewall policy resource. Possible values include:
        /// 'Succeeded', 'Updating', 'Deleting', 'Failed'</param>
        /// <param name="resourceState">Resource status of the policy.</param>
        /// <param name="httpListeners">A collection of references to
        /// application gateway http listeners.</param>
        /// <param name="pathBasedRules">A collection of references to
        /// application gateway path rules.</param>
        /// <param name="etag">A unique read-only string that changes whenever
        /// the resource is updated.</param>
        public WebApplicationFirewallPolicy(ManagedRulesDefinition managedRules, string id = default(string), string name = default(string), string type = default(string), string location = default(string), IDictionary<string, string> tags = default(IDictionary<string, string>), PolicySettings policySettings = default(PolicySettings), IList<WebApplicationFirewallCustomRule> customRules = default(IList<WebApplicationFirewallCustomRule>), IList<ApplicationGateway> applicationGateways = default(IList<ApplicationGateway>), string provisioningState = default(string), string resourceState = default(string), IList<SubResource> httpListeners = default(IList<SubResource>), IList<SubResource> pathBasedRules = default(IList<SubResource>), string etag = default(string))
            : base(id, name, type, location, tags)
        {
            PolicySettings = policySettings;
            CustomRules = customRules;
            ApplicationGateways = applicationGateways;
            ProvisioningState = provisioningState;
            ResourceState = resourceState;
            ManagedRules = managedRules;
            HttpListeners = httpListeners;
            PathBasedRules = pathBasedRules;
            Etag = etag;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets describes policySettings for policy.
        /// </summary>
        [JsonProperty(PropertyName = "properties.policySettings")]
        public PolicySettings PolicySettings { get; set; }

        /// <summary>
        /// Gets or sets describes custom rules inside the policy.
        /// </summary>
        [JsonProperty(PropertyName = "properties.customRules")]
        public IList<WebApplicationFirewallCustomRule> CustomRules { get; set; }

        /// <summary>
        /// Gets a collection of references to application gateways.
        /// </summary>
        [JsonProperty(PropertyName = "properties.applicationGateways")]
        public IList<ApplicationGateway> ApplicationGateways { get; private set; }

        /// <summary>
        /// Gets the provisioning state of the web application firewall policy
        /// resource. Possible values include: 'Succeeded', 'Updating',
        /// 'Deleting', 'Failed'
        /// </summary>
        [JsonProperty(PropertyName = "properties.provisioningState")]
        public string ProvisioningState { get; private set; }

        /// <summary>
        /// Gets resource status of the policy.
        /// </summary>
        /// <remarks>
        /// Resource status of the policy. Possible values include: 'Creating',
        /// 'Enabling', 'Enabled', 'Disabling', 'Disabled', 'Deleting'
        /// </remarks>
        [JsonProperty(PropertyName = "properties.resourceState")]
        public string ResourceState { get; private set; }

        /// <summary>
        /// Gets or sets describes the managedRules structure
        /// </summary>
        [JsonProperty(PropertyName = "properties.managedRules")]
        public ManagedRulesDefinition ManagedRules { get; set; }

        /// <summary>
        /// Gets a collection of references to application gateway http
        /// listeners.
        /// </summary>
        [JsonProperty(PropertyName = "properties.httpListeners")]
        public IList<SubResource> HttpListeners { get; private set; }

        /// <summary>
        /// Gets a collection of references to application gateway path rules.
        /// </summary>
        [JsonProperty(PropertyName = "properties.pathBasedRules")]
        public IList<SubResource> PathBasedRules { get; private set; }

        /// <summary>
        /// Gets a unique read-only string that changes whenever the resource
        /// is updated.
        /// </summary>
        [JsonProperty(PropertyName = "etag")]
        public string Etag { get; private set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ManagedRules == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ManagedRules");
            }
            if (PolicySettings != null)
            {
                PolicySettings.Validate();
            }
            if (CustomRules != null)
            {
                foreach (var element in CustomRules)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
            if (ApplicationGateways != null)
            {
                foreach (var element1 in ApplicationGateways)
                {
                    if (element1 != null)
                    {
                        element1.Validate();
                    }
                }
            }
            if (ManagedRules != null)
            {
                ManagedRules.Validate();
            }
        }
    }
}
