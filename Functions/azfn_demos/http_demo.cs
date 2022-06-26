using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace azfn_demos
{
    public static class http_demo
    {
        [FunctionName("http_demo")]
        public static async Task<IActionResult> Run(
            [Queue("outqueue"),StorageAccount("AzureWebJobsStorage")] ICollector<string> output,
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            // Add to outQueue 
            if (!string.IsNullOrEmpty(name))
            {
                // Add a message to the output collection.
                output.Add(name);
                log.LogInformation($"Added {name} to outqueue.");

            }

            // send response
            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";
                
           

            return new OkObjectResult(responseMessage);
        }
    }
}
