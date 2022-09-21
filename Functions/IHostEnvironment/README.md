# IHostEnvironment Extensions

To determinethe Hosting environment, use the `HostingEnvironmentExtensions` package.

The environment names "Development", "Production" and "Staging" are
provided, and can be extended to add others.

For example:

```
Injecting IHostEnvironment into a dotnet app.

E.g. in a razor Page

```html

@inject IHostEnvironment environment

```

E,g. in an Azure Function (Isolated), inject into the Function
constructor and use a readonly field to store the injected service.

```c#

public class DoWork
{
    private readonly IHostEnvironment environment;
		      
    public DoWork(
        IHostEnvironment environment
	)
    {
	 this.environment = environment;
    }

```

You can then access from the class Methods


environment.IsDevelopment() - Checks if the current host environment name is
Development. 

IsStaging(IHostEnvironment) - Checks if the current host environment
name is Staging.

environment.IsProduction(); - Checks if the current host environment name is Production. 

```

you can also add your own environment checks, e.g.

```

environment.IsEnvironment("UAT") - Compares the current host environment name against the specified value e.g. "UAT" or "INT" 

```

# Some Points on 'AZURE_FUNCTIONS_ENVIRONMENT'

* `AZURE_FUNCTIONS_ENVIRONMENT` is set to 'Development' by default on localhost
* You *can* (but shouldn't) override this by setting a value in the environment varible. 
* If you do this, you will get a warning:

```
    Skipping 'AZURE_FUNCTIONS_ENVIRONMENT' from local settings as it's already defined in current environment variables.

```

For WebAppp, use the ASPNETCORE_ENVIRONMENT Variable to set the
environment settings.


e.g. via the Properite/launchsettings.json file:

```
 "ASPNETCORE_ENVIRONMENT" : "UAT"
```


# Reserved Environment Variables

* Azure Functions : AZURE_FUNCTIONS_ENVIRONMENT
* WebApplications : ASPNETCORE_ENVIRONMENT
