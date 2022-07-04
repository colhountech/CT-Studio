# Multitenancy

MultiTenancy in a nutshell. Tailspin sells subscriptions to its SaaS
application. Contoso and Fabrikam sign up for the app. When Alice
(alice@contoso) signs in, the application should know that Alice is
part of Contoso.

* Alice should have access to Contoso data.
* Alice shouldn't have access to Fabrikam data.

Multitenacy is  the   software  design approach to make this happen. In
this   case we are using Azure  AD to  provide  this approach. It is an
architecture where multiple tenants share the same physically deployed
resources of an app but have their own logical instance of that app.

A single tenant architecture, each tenant has it's own deployment of an
app.

Horizontal scaling relies on a load balancer to route to appropriate
instances but any request can be routed to any instances.

* Authentication, when a user logins in the app knows which tenant the user belongs to 
* Authentication, Role assignments should be managed by the customer,not the SAAS appliation


# Introduction to Identity.Web

The following section is based on the [Tutorial - Enable your Web Apps to sign-in users and call APIs with the Microsoft identity platform for developers](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2)

In this tutorial, you will learn, incrementally, how to add sign-in
users to your Web App, and how to call Web APIs, both Microsoft APIs
or your own APIs. Finally, you'll learn best practices and how to
deploy your app to Azure



## Chapter 1: Enable your Web app to sign-in users using the Microsoft Identity Platform


* Ch 1-1 : An ASP.NET Core Web app signing-in users with the Microsoft identity platform in your organization


In this code, we Processing OpenID Connect sign-in responses by
extracting the user's claims from returned JWT, and putting the
claims in ClaimsPrincipal.Current.

You can trigger the middleware to send an OpenID Connect sign-in
request by decorating a class or method with the [Authorize] attribute
or by issuing a challenge (see the AccountController.cs file which is
part of ASP.NET Core):


You can add a policy via Controller Filter

```

 services.AddControllersWithViews(options =>
     {
         var policy = new AuthorizationPolicyBuilder()
	.RequireAuthenticatedUser()
	.Build();
         options.Filters.Add(new AuthorizeFilter(policy));
    })
```

or via specific policy such as the following customer
HQAuthoizationPolicy

```
 services.AddAuthorization(options =>
     {
         // options.AddPolicy("AllUsers", policy => policy.RequireAuthenticatedUser());
         options.AddPolicy(HQAuthorizationPolicy.Name,HQAuthorizationPolicy.Build);
     });
```





# OpenID Connect

This next section is specific to the OIDC protocol and maps AzureAD
claims to OpenID Connect Claims.


# Authenticate using Azure AD and OpenID Connect : Tailspin

A fictional SaaS application that allows organisation to create,edit and
publish surveys.

authenticated users can create/publish survey. They can view surveys
they have created or contributed to. e.g, the app will have the
folling sections:

* Surveys |I've Created
* Surveys |I've Contributed to
* Suveyss:|I've Published


Also:
- Each Survey has a title and 0 or more Multi-Choice Questions
- Users can also see other surveys from other users in this organisation
- Users can invite others outside organisation to contribute to survey (Cross-tenant Sharing of Resources)
- Contributors can only edit survey, but can't publish or delete survey

# Architecture

ASP.NET Core WebApp and WebApi. Azure AD to authorise. Get OAuth2
access token for web api, tokens cached in redis cache.

```
[Azure Ad]
   |
[Web App]---------[Web Api]
   |
[Token Cache]
   |
   +->[Redis]

```OA


# Authentication

- OpenID Connect (OIDC) protocol

Simple Workflow:

1. [User] Clicks Login
2. App [Controller] Request then Initiates a Challenge to [OpenID Middleware]
3. [App] Redirects to [Azure Ad]
4. [User] Authenticaes with [Azure Ad]
5. [AzureAD] Redirect back to [App] with Auth Token
6. [OpenID Middleware] validated Auth Token
7. [OpenID Middleware] Redirects to Request


More Details version:

1. The user clicks the "sign in" button in the app. This action is handled by an MVC controller.
2. The MVC controller returns a ChallengeResult action.
3. The middleware intercepts the ChallengeResult and creates a 302 response, which redirects the user to the Azure AD sign-in page.
4. The user authenticates with Azure AD.
5. Azure AD sends an ID token to the application.
6. The middleware validates the ID token. At this point, the user is now authenticated inside the application.
7. The middleware redirects the user back to application.


# Steps

1. Register the app with Azure AD

1. Configure the auth middleware

Notes:

* `CookieAuthenticationDefaults.AuthenticationScheme` - after the user
is authenticated, the user claims are stored locally in a cookie. This
cookie is how the user stays logged in during the browser session.

* 'options.Events' During the authentication process, the OpenID
Connect middleware raises a series of events, see [Authentication
events](#Authentication events).  At the very least, add the
[Configure the auth middleware](#Configure the auth middleware) gist
and add the `public class SurveyAuthenticationEvents :
OpenIdConnectEvents` from the `Tailspin.Surveys.Web.Security`
namespace



1. Initiate the authentication flow

```c#
[AllowAnonymous]
public IActionResult SignIn()
{
    return new ChallengeResult(
        OpenIdConnectDefaults.AuthenticationScheme,
	new AuthenticationProperties
	{
	    IsPersistent = true,
	    RedirectUri = Url.Action("SignInCallback", "Account")
        });
}
```

This causes the middleware to return a 302 (Found) response that
redirects to the authentication endpoint.

1. User login sessions

when the user first signs in, the Cookie Authentication middleware
writes the user claims to a cookie. After that, HTTP requests are
authenticated by reading the cookie.

By default, the cookie middleware writes a session cookie, which gets
deleted once the user closes the browser. The next time the user next
visits the site, they will have to sign in again. However, if you set
IsPersistent to true in the ChallengeResult, the middleware writes a
persistent cookie, so the user stays logged in after closing the
browser. You can configure the cookie expiration; see Controlling
cookie options. Persistent cookies are more convenient for the user,
but may be inappropriate for some applications (say, a banking
application) where you want the user to sign in every time.


1. Authentication ticket

If authentication succeeds, the OIDC middleware creates an
authentication ticket, which contains a claims principal that holds
the user's claims.

>**Note**
> Until the entire authentication flow is completed, HttpContext.User
> still holds an anonymous principal, not the authenticated user. The
> anonymous principal has an empty claims collection. After
> authentication completes and the app redirects, the cookie middleware
> deserializes the authentication cookie and sets HttpContext.User to a
> claims principal that represents the authenticated user.















# Gists

# Configure the auth middleware
```
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(
                    options =>
                    {
                        Configuration.Bind("AzureAd", options);
                        options.Events = new SurveyAuthenticationEvents(loggerFactory);
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.Events.OnTokenValidated += options.Events.TokenValidated;
                    })
               .EnableTokenAcquisitionToCallDownstreamApi()
               .AddDownstreamWebApi(configOptions.SurveyApi.Name, Configuration.GetSection("SurveyApi"))
               .AddDistributedTokenCaches();

```


# Authentication events

During the authentication process, the OpenID Connect middleware
raises a series of events including the following:

* **TokenValidated**. Called after the middleware validates the ID token.
At this point, the application has a set of validated claims about the
user. You can use this event to perform additional validation on the
claims, or to transform claims. See Working with claims.

* **RedirectToIdentityProvider**. Called right before the middleware
redirects to the authentication endpoint. You can use this event to
modify the redirect URL; for example, to add request parameters. See
Adding the admin consent prompt for an example.

* **AuthenticationFailed**. Called if authentication fails. Use this event
to handle authentication failures — for example, by redirecting to an
error page. 

To provide callbacks for these events, set the Events option on the
middleware. There are two different ways to declare the event
handlers: Inline with lambdas, or in a class that derives from
OpenIdConnectEvents. The second approach is recommended if your event
callbacks have any substantial logic, so they don't clutter your
startup class. Our reference implementation uses this approach. see
`public class SurveyAuthenticationEvents : OpenIdConnectEvents` in the
`Tailspin.Surveys.Web.Security` Namespace


# Reference

 * [Identity Management for Multitenant Applications in Microsoft Azure](https://github.com/mspnp/multitenant-saas-guidance)
 * [TailSpin Survey Sample Application](https://github.com/mspnp/multitenant-saas-guidance/blob/master/get-started.md)
 * [Identity management in multitenant applications](https://docs.microsoft.com/en-gb/azure/architecture/multitenant-identity/)
 * [Tutorial - Enable your Web Apps to sign-in users and call APIs with the Microsoft identity platform for developers](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2)
