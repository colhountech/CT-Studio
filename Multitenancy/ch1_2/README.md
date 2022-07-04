# Ch1-2

In this Chapter we change your ASP.NET Core Web app to sign-in users
in any org with the Microsoft identity platform (but not Microsoft
Personal Accounts -that's nexT) 

update the appName:
```
appName="ch1_2"
```

when creating the app registration, add the following to the `az ad app create command`

```
--sign-in-audience AzureADandPersonalMicrosoftAccount
```

set the Tenantid to "organisations" or "common"

Run setup.sh again

Add a new token validator to .AddMicrosoftIdentityWebApp

```c#
    .AddMicrosoftIdentityWebApp (options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.TokenValidationParameters.IssuerValidator = ValidateSpecificIssuers;
    });
```

Introduce a new ValidatorSpecificedIssuers method:

```c#

 string ValidateSpecificIssuers(string issuer, SecurityToken securityToken,
                                          TokenValidationParameters validationParameters)
    {
        var validIssuers = GetAcceptedTenantIds()
                             .Select(tid => $"https://login.microsoftonline.com/{tid}/v2.0");
        if (validIssuers.Contains(issuer))
        {
            return issuer;
        }
        else
        {
            throw new SecurityTokenInvalidIssuerException("The sign-in user's account does not belong to one of the tenants that this Web App accepts users from.");
        }
    }

     string[] GetAcceptedTenantIds()
    {
        return new string[]
        {
	  "" // Add guid here
        };
    }


```




