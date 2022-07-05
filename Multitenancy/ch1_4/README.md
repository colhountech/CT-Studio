# Ch1-3

In this Chapter we will authorize sign-in from Soverign Clouds for
example:

* Azure US Government
* Azure China 21Vianet
* Azure Germany

Change the value of "Instance" to one of 

|National cloudAzure |AD authentication endpoint
|--------------------|----------------------------
| Azure AD for US Government | https://login.microsoftonline.us
| Azure AD China operated by 21Vianet | https://login.partner.microsoftonline.cn
| Azure AD (global service)|https://login.microsoftonline.com


Set the tenant id to the correct value:

You many need to change:
```c#
  services
      .AddAuthentication(AzureADDefaults.AuthenticationScheme)
      .AddAzureAD(options => Configuration.Bind("AzureAd",options));
```

to this line:

```c#
    services.AddMicrosoftIdentityWebAppAuthentication(Configuration);
```


That's all. Now you can login into a soverign cloud



