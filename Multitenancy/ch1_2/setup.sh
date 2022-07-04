#!/bin/bash

# show app registration 
echo "Here is a list of all your Azure App Registrations"
az ad app list --query "[].{name:displayName,appId:appId}" --output table

#az ad app delete --id "00000000-000-0000-0000-00000000"
# rm -r Pages Program.cs Properties TestAuth.csproj appsettings.* bin obj wwwroot

appName="ch1_2"
clientid=$(az ad app create --sign-in-audience AzureADandPersonalMicrosoftAccount --display-name "$appName" --enable-id-token-issuance --query appId --output tsv)
#tenantid=$(az account show --query tenantId --output tsv)
# Allow Access from AnyOrg
tenantid="organizations" 
objectid=$(az ad app show --id $clientid --query id --output tsv)
echo "AppName = "$appName 
echo "ClientId = "$clientid 
echo "TenantId = "$tenantid 
echo "ObjectID = "$objectid

# Add redirect URLs (replace localhost with your local/azure settings)
redirecttype=web
redirecturl=https://localhost:7204/signin-oidc
graphurl=https://graph.microsoft.com/v1.0/applications/$objectid
az rest --method PATCH --uri $graphurl --headers 'Content-Type=application/json' --body '{"'$redirecttype'":{"redirectUris":["'$redirecturl'"]}}'

# create app
dotnet new webapp --auth SingleOrg --domain colhountech.com  --client-id $clientid --tenant-id $tenantid --framework net6.0

# Add to appsettings, if not already set
jq '.AzureAd.Instance="https://login.microsoftonline.com/"' > _.json < appsettings.json && mv  _.json appsettings.json
jq '.AzureAd.TenantId = "'$tenantid'" '                     > _.json < appsettings.json && mv  _.json appsettings.json
jq '.AzureAd.ClientId = "'$clientid'" '                     > _.json < appsettings.json && mv  _.json appsettings.json
jq '.AzureAd.CallbackPath = "/signin-oidc" '                > _.json < appsettings.json && mv  _.json appsettings.json

# fix applicationUrl for this demo
jq '.profiles.'$appName'.applicationUrl="https://localhost:7204"'> _.json < Properties/launchSettings.json && mv _.json Properties/launchSettings.json



echo ""
echo "to delete application from azure ad:"
echo "az ad app delete --id $clientid"
echo ""
echo "To remove all generated pages"
echo "rm -r Pages Program.cs Properties $appName.csproj appsettings.* bin obj wwwroot"
echo ""
echo "Here is a list of all your Azure App Registrations"
az ad app list --query "[].{name:displayName,appId:appId}" --output table
echo ""
echo "to run application:"
echo "dotnet run"
