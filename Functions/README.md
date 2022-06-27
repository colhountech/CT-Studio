# Azure Function

## Runtime Version

several different versions from 
Azure Functions runtime version 1.0 to Azure Functions runtime version 4.0

to test is it installed type
```
func --version
```

if all goes well, you will get this:
```
% func --version
4.0.4590

```

To install azure functions core tools, go to [Azure Functions Core Tools version 4.x.](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=v4%2Cmacos%2Ccsharp%2Cportal%2Cbash#v2)

for mac it's

```
brew tap azure/functions
brew install azure-functions-core-tools@4
# if upgrading on a machine that has 2.x or 3.x installed:
brew link --overwrite azure-functions-core-tools@4
```

for windows, download hte msi installer
[v4.x - Windows 64-bit](https://go.microsoft.com/fwlink/?linkid=2174087)


We are only interested in version4.9 which support .NET Core 6.0

# Azure Function Isolation

- In Process (.NET class library) - cost less, possibly chatt neighbours
- Out of Proess (.NET Isolated) - costs more, no chatty neighbours 

# Setting Files

- host.json - affects both cloud and local test settings
- local.settings.json - for connection strings etc when running locally

To add setting to Azure Function, head to portal and Manage Azure App Service and use 
Application Settings

- can also use '--publish-local-setting' with cli 
- can also use Azure CLI - az

# MSFT recommend using Application Insights to monitor Azure functions

# Recommneded App Life Cycle

- Build Azure Function
- Build XUnit Test
- Test and Repeat

# (to setup logging, you need to buila a Null Scope, ListLogger and LoggerType. see example)


# prerequiiits to build Azure Functions

- `func --version` should be 4.0
- `dotnet --list-sdks` should have 6.0 installed
- `az --version` should be >= 2.4 ?? (this should be 2.37??)
- `az login` to get access to subscription


To install azure functions core tools, go to [Azure Functions Core Tools version 4.x.](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=v4%2Cmacos%2Ccsharp%2Cportal%2Cbash#v2)

for mac it's

```
brew tap azure/functions
brew install azure-functions-core-tools@4
# if upgrading on a machine that has 2.x or 3.x installed:
brew link --overwrite azure-functions-core-tools@4
```

for windows, download hte msi installer
[v4.x - Windows 64-bit](https://go.microsoft.com/fwlink/?linkid=2174087)


We are only interested in version4.9 which support .NET Core 6.0

# Azure Function Isolation

- In Process (.NET class library) - cost less, possibly chatt neighbours
- Out of Proess (.NET Isolated) - costs more, no chatty neighbours 

# Setting Files

- host.json - affects both cloud and local test settings
- local.settings.json - for connection strings etc when running locally

To add setting to Azure Function, head to portal and Manage Azure App Service and use 
Application Settings

- can also use '--publish-local-setting' with cli 
- can also use Azure CLI - az


xcode-select --install
      
      

# Available Function Trinnger


1. QueueTrigger
2. HttpTrigger
3. BlobTrigger
4. TimerTrigger
5. KafkaTrigger
6. KafkaOutput
7. DurableFunctionsOrchestration
8. SendGrid
9. EventHubTrigger
10. ServiceBusQueueTrigger
11. ServiceBusTopicTrigger
12. EventGridTrigger
13. CosmosDBTrigger
14. IotHubTrigger


#Setup a new local Function


func init af_demo1 --dotnet
cd af_demo1
func new  --name http_demo  --template "HttpTrigger --authlevel "anonymous""
func start

curl http://localhost:7071/api/http_demo
curl -X  POST  "http://localhost:7071/api/http_demo?name=micheal"



# Setup an Azure Function

az login
az account show --output table
az account set --subscription "MPN
az account show



# Setup a new Azure Functions Resource Groups

Note that Storage Account names have to be unqiue across all of the
Azure Cloud and can only contain lower case letters and numbers

az group create --name az-rg --location NorthEurope
az storage account create --name azsa1234 --location northeurope --resource-group az-rg --sku Standard_LRS

# this is fun verion 4
az functionapp create --resource-group az-rg --consumption-plan-location northeurope --runtime dotnet --functions-version 4 --name azfnv4 --storage-account azsa1234

# This create a FUNCTION APP (not an azure funciton) at  a host called:
azfnv4.azurewebsites.net

# list function

func azure functionapp list-functions azfnv3

# Note that functionapp is the context, it's either functionapp for local function or storage for function in storage

# Now publish the local function to function app

func azure functionapp publish azfnv3

# Get alomost real time log streaming of app

func azure functionapp logstream azfnv3

# to get function url
# Portal > Browse to Function App > Functions > http_demo > Get Function URL

browse to : https://azfnv3.azurewebsites.net/api/http_demo?

browse to : https://azfnv3.azurewebsites.net/api/http_demo?name=micheal


# QueueStorage Functions


Setup function app

```
az login
az account show --output table
az account set --subscription MPN
az account show
az group create --name ms-learn --location NorthEurope
az storage account create --name azsa1234 --location northeurope --resource-group ms-learn --sku Standard_LRS
az functionapp create --resource-group ms-learn --consumption-plan-location northeurope --runtime dotnet --functions-version 4 --name azfnv4 --storage-account azsa1234
curl -s -o /dev/null -w "%{http_code}\n"  azfnv4.azurewebsites.net

# create a new functions project
func init azfun_demos --dotnet
cd azfun_demos

# list templates
func templates list

# Add a demo
func new  --name http_demo  --template "HttpTrigger" --authlevel "anonymous"
func start

# push to azure

func azure functionapp publish azfnv4
#  Invoke url: https://azfnv4.azurewebsites.net/api/http_demo

func azure functionapp list-functions azfnv4
# log streaming
func azure functionapp logstream azfnv4 --browser
# or
func azure functionapp logstream azfnv4
# or
az webapp log tail --resource-group ms-learn  --name azfnv4

```
# List c# templates

  Azure Blob Storage trigger
  Azure Cosmos DB trigger
  Durable Functions activity
  Durable Functions HTTP starter
  Durable Functions orchestrator
  Azure Event Grid trigger
  Azure Event Hub trigger
  HTTP trigger
  IoT Hub (Event Hub)
  Azure Queue Storage trigger
  RabbitMQ trigger
  SendGrid
  Azure Service Bus Queue trigger
  Azure Service Bus Topic trigger
  SignalR negotiate HTTP trigger
  Timer trigger

# functions-add-output-binding-storage-queue-cli

```
# overwrite local.settings.json
func azure functionapp fetch-app-settings
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage

```
Add the following code

``` 

// Add to outQueue 
if (!string.IsNullOrEmpty(name)
{ 
    // Add a message to the output collection.
    output.Add(name);
    log.LogInformation($"Added {name} to outqueue.");
}
```

Get connection string from local.settings.json and add to env 

export AZURE_STORAGE_CONNECTION_STRING=""

az storage queue list --output tsv

az storage queue list --output tsv --account-name azsa1234 
Command group 'storage queue' is in preview and under development.
Reference and support levels: https://aka.ms/CLI_refstatus
None None outqueue
echo echo $(az storage message get --queue-name outqueue -o tsv
--query '[].{Message:content}') | base64 --decode

az storage message get --account-name azsa1234 --queue-name outqueue
echo bWljaGVhbA== | base64 --decode

[
  {
    "content": "bWljaGVhbA==",
    "dequeueCount": 1,
    "expirationTime": "2022-07-03T10:36:29+00:00",
    "id": "f6268167-60de-4ec4-82ad-335ec1c8a776",
    "insertionTime": "2022-06-26T10:36:29+00:00",
    "popReceipt": "AgAAAAMAAAAAAAAAIA3A00qJ2AE=",
    "timeNextVisible": "2022-06-26T10:52:28+00:00"
  }
]


func azure functionapp publish 

echo `echo $(az storage message get --account-name azsa1234 --queue-name outqueue -o tsv --query '[].{Message:content}') | base64 --decode`

# Next Steps 

look at complete Example projects in c#

https://docs.microsoft.com/en-us/samples/browse/?products=azure-functions&languages=csharp


# Templates examples

## QueueTrigger

```c#

func new --name queue_demo --template queueTrigger

[FunctionName("queue_demo")]
[QueueTrigger("myqueue-items", Connection = "")]string myQueueItem, ILogger log)

```

## HttpTrigger

```c#
func new  --name http_demo  --template "HttpTrigger" --authlevel "anonymous"

[FunctionName("http_demo")]
[Queue("outqueue"),StorageAccount("AzureWebJobsStorage")] ICollector<string> output,
[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]

```


## BlobTrigger


```c#

func new --name blob_demo --template BlobTrigger 

[FunctionName("blob_demo")]
[BlobTrigger("samples-workitems/{name}", Connection = "")]Stream myBlob, string name, ILogger log)

```


## TimerTrigger

```c#
func new --template TimerTrigger --name timer_demo  
[FunctionName("timer_demo")]
[TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)

```

## SendGrid

```c#
func new --template SendGrid --name sendgrid_demo 

 [FunctionName("sendgrid_demo")]
         [return: SendGrid(ApiKey = "", To = "{CustomerEmail}", From =
"SenderEmail@org.com")]
        public SendGridMessage Run([QueueTrigger("sampleMessages",
Connection = "")]Order order, ILogger log)
```
9. EventHubTrigger


```c#
```
10. ServiceBusQueueTrigger


```c#
```

11. ServiceBusTopicTrigger

```c#
```

12. EventGridTrigger

```c#
```

13. CosmosDBTrigger


```c#
```
14. IotHubTrigger


```c#
```


