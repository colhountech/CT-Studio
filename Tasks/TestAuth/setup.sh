#!/bin/bash

# show app registration 
az ad app list --query "[].{name:displayName,appId:appId}" --output table

#az ad app delete --id "00000000-000-0000-0000-00000000"
# rm -r Pages Program.cs Properties TestAuth.csproj appsettings.* bin obj wwwroot 

appName="Fulfilled HomePage"
clientid=$(az ad app create --display-name "$appName" --enable-id-token-issuance --query appId --output tsv)
tenantid=$(az account show --query tenantId --output tsv)
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
dotnet new webapp --auth SingleOrg --domain fulfilled.cc  --client-id $clientid --tenant-id $tenantid --framework net6.0

# Add to appsettings, if not already set
jq '.AzureAd.Instance="https://login.microsoftonline.com/"' > _.json < appsettings.json && mv  _.json appsettings.json
jq '.AzureAd.TenantId = "'$tenantid'" '                     > _.json < appsettings.json && mv  _.json appsettings.json
jq '.AzureAd.ClientId = "'$clientid'" '                     > _.json < appsettings.json && mv  _.json appsettings.json
jq '.AzureAd.CallbackPath = "/signin-oidc" '                > _.json < appsettings.json && mv  _.json appsettings.json
