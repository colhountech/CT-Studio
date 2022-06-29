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

# list app registrations, including login and logout  uri
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


# The next few commands require jq
# to install jq on macos do:
brew install jq

# on ubuntu, it's
sudo apt install jq

# Remove api permissions: disable default exposed scope first
# add "oauth2Permissions": [ { "isEnabled": false  } ] to the json object
#
# default_scope=$(az ad app show --id $clientid | jq '.oauth2Permissions[0].isEnabled = false' | jq -r '.oauth2Permissions')
#
#
# az ad app update --id $clientid --set oauth2Permissions="$default_scope"
# az ad app update --id $clientid --set oauth2Permissions="[]"
# this does not work: Invalid property 'oauth2Permissions'.
# https://docs.microsoft.com/en-us/azure/healthcare-apis/register-application-cli-rest


# this is another way to create an app registration

# make sure you have set $appName first 
az rest -m post -u https://graph.microsoft.com/v1.0/applications --headers 'Content-Type=application/json' --body '{"displayName":"'$appName'"}'

### Use --query to obtain the client app id
clientid=$(az rest -m post -uhttps://graph.microsoft.com/v1.0/applications  --headers 'Content-Type=application/json' --body '{"displayName": "'$appName'"}' --query appId --output tsv)


# Add a client secret, not required for public api

###Add client secret with expiration. The default is one year.
#clientsecretname=mycert2
#clientsecretduration=2
#clientsecret=$(az ad app credential reset --id $clientid --append --credential-description $clientsecretname --years $clientsecretduration --query password --output tsv)
#echo $clientsecret
# Doesn't work 

# Add redirect URLs

###Update app registration using REST. az ad supports reply-urls only. 
#https://github.com/Azure/azure-cli/issues/9501
#redirecttype=spa | web | publicClient

redirecttype=publicClient
redirecturl=https://www.getpostman.com/oauth2/callback
graphurl=https://graph.microsoft.com/v1.0/applications/$objectid
az rest --method PATCH --uri $graphurl --headers 'Content-Type=application/json' --body '{"'$redirecttype'":{"redirectUris":["'$redirecturl'"]}}'
