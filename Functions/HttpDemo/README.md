# HttpDemo

az commands to build this project from scratch (no code)

```
func init --worker-runtime dotnet 
func new --name HttpDemo --template HttpTrigger --authlevel anonymous
func start
az login
az resource list --resource-group ms-learn --output table
```

I previously used this resource group and when I deleted the azure
function, it left behind some app-insights, smart detection and
failure anomolie alert rules, so I killed them with this script
(note: update your subscription id)

```
az functionapp delete --name azfnv4 --resource-group ms-learn
az monitor app-insights component delete --app azfnv4 -g ms-learn
az resource delete --ids "/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/ms-learn/providers/microsoft.alertsmanagement/smartDetectorAlertRules/Failure Anomalies - azfnv4"
az resource delete --ids "/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/ms-learn/providers/microsoft.insights/actiongroups/Application Insights Smart Detection"
```
Now I can create the Azure Function app

```
az functionapp create --resource-group ms-learn --consumption-plan-location northeurope --runtime dotnet --functions-version 4 --name azfnv4 --storage-account azsa7
func azure functionapp publish azfnv4
```

If I need to pull the azure function app setting from azure to local,
do this:

```
 func azure functionapp fetch-app-settings azfnv4
```

this will update `local.settings.json' and replace `AzureWebJobsStorage": "UseDevelopmentStorage=true"` with the full Storage Accocunt Connection String for that Function App.

this fetch will also add 5 more key-value pairs for the `Values` object:

    * FUNCTIONS_EXTENSION_VERSION - now set to 4
    * WEBSITE_CONTENTAZUREFILECONNECTIONSTRING - seems to be the same as the `AzureWeJobsStorage`
    * WEBSITE_CONTENTSHARE
    * APPINSIGHTS_INSTRUMENTATIONKEY
    * WEBSITE_RUN_FROM_PACKAGE - set to 1
    
 also adds a new object which is empty called
 
    * ConnectionStrings
    
Note: all the value are not encrypted. If you change the `IsEncrypted`
to `true`, the fetch will download the value encrypted

