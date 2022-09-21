using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HttpTriggerIsolated
{
    public class DoWork
    {
        private readonly ILogger _logger;
        private readonly IHostEnvironment environment;

        public DoWork(
            IHostEnvironment environment,
            ILoggerFactory loggerFactory
            )
        {
            _logger = loggerFactory.CreateLogger<DoWork>();
            this.environment = environment;
        }

        [Function("DoWork")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (environment.IsProduction())
            {
                _logger.LogInformation($"C# HTTP trigger in Production environment.");
            }

            if (environment.IsStaging())
            {
                _logger.LogInformation($"C# HTTP trigger in Staging  environment.");
            }

            if (environment.IsEnvironment("UAT"))
            {
                _logger.LogInformation($"C# HTTP trigger in UAT environment.");
            }


            if (environment.IsDevelopment())
            {
                _logger.LogInformation($"C# HTTP trigger in Development  environment.");
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
