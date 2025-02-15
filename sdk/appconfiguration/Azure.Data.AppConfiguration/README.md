# Azure App Configuration client library for .NET

Azure App Configuration is a managed service that helps developers centralize their application and feature settings simply and securely.

Use the client library for App Configuration to:

* [Create and manage centrally stored application configuration settings][azconfig_setting_concepts]
* [Retrieve configuration settings from a specific point in time][azconfig_asof_snapshot]

[Source code][source_root] | [Package (NuGet)][package] | [API reference documentation][reference_docs] | [Product documentation][azconfig_docs] | [Samples][source_samples]

## Getting started

### Install the package

Install the Azure App Configuration client library for .NET with [NuGet][nuget]:

```PowerShell
Install-Package Azure.Data.AppConfiguration -Version 1.0.0-preview.2
```

**Prerequisites** 
* An [Azure subscription][azure_sub].
* An existing [Configuration Store][configuration_store].  

If you need to create a Configuration Store, you can use the Azure Portal or [Azure CLI][azure_cli].

You can use the Azure CLI to create the Configuration Store with the following command:

```PowerShell
az appconfig create --name <config-store-name> --resource-group <resource-group-name> --location eastus
```

### Authenticate the client

In order to interact with the App Configuration service, you'll need to create an instance of the [Configuration Client][configuration_client_class] class. To make this possible, you'll need the connection string of the Configuration Store.

#### Get credentials

Use the [Azure CLI][azure_cli] snippet below to get the connection string from the Configuration Store.

```PowerShell
az appconfig credential list --name <config-store-name>
```

Alternatively, get the connection string from the Azure Portal.

#### Create ConfigurationClient

Once you have the value of the connection string, you can create the ConfigurationClient:

```c#
string connectionString = <connection_string>;
var client = new ConfigurationClient(connectionString);
```

## Key concepts

### Configuration Setting

A Configuration Setting is the fundamental resource within a Configuration Store. In its simplest form, it is a key and a value. However, there are additional properties such as the modifiable content type and tags fields that allow the value to be interpreted or associated in different ways.

The [Label][label_concept] property of a Configuration Setting provides a way to separate Configuration Settings into different dimensions. These dimensions are user defined and can take any form. Some common examples of dimensions to use for a label include regions, semantic versions, or environments. Many applications have a required set of configuration keys that have varying values as the application exists across different dimensions.

For example, MaxRequests may be 100 in "NorthAmerica" and 200 in "WestEurope". By creating a Configuration Setting named MaxRequests with a label of "NorthAmerica" and another, only with a different value, with a "WestEurope" label, an application can seamlessly retrieve Configuration Settings as it runs in these two dimensions.

Properties of a Configuration Setting:

```C# Snippet:SettingProperties
/// <summary>
/// The primary identifier of the configuration setting.
/// A <see cref="Key"/> is used together with a <see cref="Label"/> to uniquely identify a configuration setting.
/// </summary>
public string Key { get; set; }

/// <summary>
/// A value used to group configuration settings.
/// A <see cref="Label"/> is used together with a <see cref="Key"/> to uniquely identify a configuration setting.
/// </summary>
public string Label { get; set; }

/// <summary>
/// The configuration setting's value.
/// </summary>
public string Value { get; set; }

/// <summary>
/// The content type of the configuration setting's value.
/// Providing a proper content-type can enable transformations of values when they are retrieved by applications.
/// </summary>
public string ContentType { get; set; }

/// <summary>
/// An ETag indicating the state of a configuration setting within a configuration store.
/// </summary>
public ETag ETag { get; internal set; }

/// <summary>
/// The last time a modifying operation was performed on the given configuration setting.
/// </summary>
public DateTimeOffset? LastModified { get; internal set; }

/// <summary>
/// A value indicating whether the configuration setting is read only.
/// A read only configuration setting may not be modified until it is made writable.
/// </summary>
public bool? IsReadOnly { get; internal set; }

/// <summary>
/// A dictionary of tags used to assign additional properties to a configuration setting.
/// These can be used to indicate how a configuration setting may be applied.
/// </summary>
public IDictionary<string, string> Tags
```

## Examples

The following sections provide several code snippets covering some of the most common Configuration Service tasks. Note that there are sync and async methods available for both.

- [Create a Configuration Setting](#create-a-configuration-setting)
- [Retrieve a Configuration Setting](#retrieve-a-configuration-setting)
- [Update an existing Configuration Setting](#update-an-existing-configuration-setting)
- [Delete a Configuration Setting](#delete-a-configuration-setting)

### Create a Configuration Setting

Create a Configuration Setting to be stored in the Configuration Store. There are two ways to store a Configuration Setting:

- Add creates a setting only if the setting does not already exist in the store.
- Set creates a setting if it doesn't exist or overrides an existing setting.

```c#
string connectionString = <connection_string>;
var client = new ConfigurationClient(connectionString);
var setting = new ConfigurationSetting("some_key", "some_value");
client.SetConfigurationSetting(setting);
```

### Retrieve a Configuration Setting

Retrieve a previously stored Configuration Setting by calling Get.  This snippet assumes the setting "some_key" exists in the configuration store.

```c#
string connectionString = <connection_string>;
var client = new ConfigurationClient(connectionString);
ConfigurationSetting setting = client.GetConfigurationSetting("some_key");
```

### Update an existing Configuration Setting

Update an existing Configuration Setting by calling Set.  This snippet assumes the setting "some_key" exists in the configuration store.

```c#
string connectionString = <connection_string>;
var client = new ConfigurationClient(connectionString);
ConfigurationSetting setting = client.SetConfigurationSetting("some_key", "new_value");
```

### Delete a Configuration Setting

Delete an existing Configuration Setting by calling Delete.  This snippet assumes the setting "some_key" exists in the configuration store.

```c#
string connectionString = <connection_string>;
var client = new ConfigurationClient(connectionString);
ConfigurationSetting setting = client.DeleteConfigurationSetting("some_key");
```

## Troubleshooting

When you interact with Azure App Configuration using the .NET SDK, errors returned by the service correspond to the same HTTP status codes returned for [REST API][azconfig_rest] requests.

For example, if you try to retrieve a Configuration Setting that doesn't exist in your Configuration Store, a `404` error is returned, indicating `Not Found`.

```c#
string connectionString = <connection_string>;
var client = new ConfigurationClient(connectionString);
ConfigurationSetting setting = client.GetConfigurationSetting("nonexistent_key");
```

You will notice that additional information is logged, like the Client Request ID of the operation.

```
Message: Azure.RequestFailedException : StatusCode: 404, ReasonPhrase: 'Not Found', Version: 1.1, Content: System.Net.Http.NoWriteNoSeekStreamContent, Headers:
{
  Connection: keep-alive
  Date: Thu, 11 Apr 2019 00:16:57 GMT
  Server: nginx/1.13.9
  x-ms-client-request-id: cc49570c-9143-411e-a6c8-3287dd114034
  x-ms-request-id: 2ad025f7-1fe8-4da0-8648-8290e774ed61
  x-ms-correlation-request-id: 2ad025f7-1fe8-4da0-8648-8290e774ed61
  Strict-Transport-Security: max-age=15724800; includeSubDomains;
  Content-Length: 0
}
```

## Next Steps

### More sample code

Several App Configuration client library samples are available to you in this GitHub repository.  These include: 
- [Hello world](samples/Sample1_HelloWorld.cs): Create and delete a configuration setting.
- [Hello world async with labels](samples/Sample2_HelloWorldExtended.cs): Asynchronously create, update and delete configuration settings with labels.
- [Make a configuration setting readonly](samples/Sample3_SetClearReadOnly.cs): Make a configuration setting read only, and then return it to a writeable state.


## Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the Microsoft Open Source Code of Conduct. For more information see the Code of Conduct FAQ or contact opencode@microsoft.com with any additional questions or comments.

![Impressions](https://azure-sdk-impressions.azurewebsites.net/api/impressions/azure-sdk-for-net%2Fsdk%2Fappconfiguration%2FAzure.ApplicationModel.Configuration%2FREADME.png)

<!-- LINKS -->
[azconfig_docs]: https://docs.microsoft.com/azure/azure-app-configuration/
[azconfig_setting_concepts]: https://docs.microsoft.com/en-us/azure/azure-app-configuration/concept-key-value
[azconfig_asof_snapshot]: https://docs.microsoft.com/en-us/azure/azure-app-configuration/concept-point-time-snapshot
[source_root]: https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/appconfiguration/Azure.Data.AppConfiguration
[source_samples]: https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/appconfiguration/Azure.Data.AppConfiguration/samples
[reference_docs]: https://azure.github.io/azure-sdk-for-net/api/Azure.Data.AppConfiguration.html
[azconfig_rest]: https://github.com/Azure/AppConfiguration#rest-api-reference
[azure_cli]: https://docs.microsoft.com/cli/azure
[azure_sub]: https://azure.microsoft.com/free/
[configuration_client_class]: src/ConfigurationClient.cs
[configuration_store]: https://docs.microsoft.com/azure/azure-app-configuration/quickstart-dotnet-core-app#create-an-app-configuration-store
[label_concept]: https://docs.microsoft.com/en-us/azure/azure-app-configuration/concept-key-value#label-keys
[nuget]: https://www.nuget.org/
[package]: https://www.nuget.org/packages/Azure.ApplicationModel.Configuration/
