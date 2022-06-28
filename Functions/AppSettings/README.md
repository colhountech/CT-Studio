# Azure App Settings 

Azure Functions have their own settings, discrete and seperate from
asp.net core app.

Some important settings include:

* AZURE_FUNCTIONS_ENVIRONMENT
* AzureWebJobsDisableHomepage
* AzureWebJobsFeatureFlags

## AZURE_FUNCTIONS_ENVIRONMENT

use this instead of ASPNETCORE_ENVIRONMENT for your apps.

If missing, takes on a default of 'Production'. Options are

* Production
* Development
* Staging

use the Azure Functions Code tools to change.


## AzureWebJobsDisableHomepage

Does exactly what it says on the tin. 


## AzureWebJobsFeatureFlags

A list of feature flags to enable using App Configuration resource.

Azure App Configuration allows developers to store, retrieve and
manage access to application settings without having to redeploy the
application

To enable feature flags, first deploy an Azure Function App.

Then Create a new resource: App Configuration, and select free plan

Go to Feature Manager > Add
Feature Flag Name : Beta
(this set the key name to appconfig.featureflag/Beta


In the Azure Functions app, add the following nuget pakages:

* Microsoft.Extensions.Configuration.AzureAppConfiguration
* Microsoft.FeatureManagement
* Microsoft.Azure.Functions.Extensions

Add a new Startup.cs file and register as a Startup file:

```c#

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]
class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
    }
		
    public override void Configure(IFunctionsHostBuilder builder)
    {
    }
}
```

add the following:

```c#

builder.ConfigurationBuilder.AddAzureAppConfiguration(
    options =>
    {
       options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
       //.Select("_")
       .UseFeatureFlags();
    });
					      
```

[!Tip]: if you don't want any configuration other than feature flags to be
loaded to your application, you can call Select("_") to only load a
nonexisting dummy key "_". By default, all configuration key-values in
your App Configuration store will be loaded if no Select method is
called.


Add `AddAzureAppConfiguration` and `AddFeaturemanagement` to Configure:


```c#
public override void Configure(IFunctionsHostBuilder builder)
{
    builder.Services.AddAzureAppConfiguration();
    builder.Services.AddFeatureManagement();
}
```

inject the following instances into the Function

* `IFeatureManagerSnapshot _featureManagerSnapshot`
* `IConfigurationRefresher _configurationRefresher`

To use Feature Flags, first check if there has been and update then
access the feature flag:

```c#
    await _configurationRefresher.TryRefreshAsync();
    var beta = await _featureManagerSnapshot.IsEnabledAsync("Beta")
    if (beta) { /* ... /* }
    
```

**Note:*** this is a test note


### Appendix: FREE Vs Standard App Configuraiton Plans


FREE

* 1 per sub
* 10 MB storage
* Revisions kept for 7 days
* Request per day quota: 10000
* SLA : None
* Cost: FREE

Standard

* unlimited app configs
* Revisions kepts for 30 days
* 1 GB storage
* 30,000 per hour
* SLA : 99.9%
* Cost : $1.20 per day + usage of $0.06 per 10k requests after 200k requests



