# Adding Azure AD to ASP.NET Core 6.0 c# WebApps

Sample project on how to implement Azure AD Authentication in ASP.NET
Core 6.0 Web application, starting with a blank ASP.NET Core 6.0
project and step by step add components for authentication and
authorization against Azure AD.


# App Registration

Register a new app in portal.

```sh

# login and choose the Azure AD domain you wish to authenticate against
az login

# if this Azure AD does not have any subscriptions (because it's used
# for business, not azure devops), then login as follows
az login --allow-no-subscriptions 

# list existing apps
az ad app list --query "[].displayName"

# list app registrations and appID
az ad app list --query "[].{name:displayName, appId:appId}"

# list app registrations, including login and logout  uri, as a table
az ad app list --query "[].{name:displayName,appId:appId,publish:publisherDomain, logout:web.logoutUrl, redirect:web.redirectUris[0]}" --output table

# Create an App Registration
az ad app create --display-name removethis

# show app registration 
az ad app list --query "[].{name:displayName,appId:appId}" --output table

# find id from previous and add to id arg
az ad app delete --id "00000000-000-0000-0000-00000000"

# define app name for reference later
appName=appRemoveThis

clientid=$(az ad app create --display-name $appName --query appId --output tsv)
echo "clientId is $clientid"

objectid=$(az ad app show --id $clientid --query id --output tsv)
echo "objectId is $objectid"

# this is another way to create an app registration
# make sure you have set $appName first 
az rest -m post -u https://graph.microsoft.com/v1.0/applications --headers 'Content-Type=application/json' --body '{"displayName":"'$appName'"}'

### Use --query to obtain the client app id
clientid=$(az rest -m post -uhttps://graph.microsoft.com/v1.0/applications  --headers 'Content-Type=application/json' --body '{"displayName": "'$appName'"}' --query appId --output tsv)

# Add a client secret, not required for public api
# todo

# Add redirect URLs
redirecttype=publicClient
redirecturl=https://www.getpostman.com/oauth2/callback
graphurl=https://graph.microsoft.com/v1.0/applications/$objectid
az rest --method PATCH --uri $graphurl --headers 'Content-Type=application/json' --body '{"'$redirecttype'":{"redirectUris":["'$redirecturl'"]}}'

