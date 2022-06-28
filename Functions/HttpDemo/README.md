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

