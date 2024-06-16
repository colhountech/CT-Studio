using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using TasksServices.Services;

namespace TasksWebApp.Pages.Extensions;

public static class PageModelExtensions
{
    public static async Task OptimisticConcurrencyControl
        (this PageModel pageModel, 
        Action operation, ITodoItemService service, ILogger logger)
    {
        try
        {
            operation();
            await service.SaveAsync();
        }
        catch (RequestFailedException e)
        {
            if (e.Status == (int)HttpStatusCode.PreconditionFailed)
            {
                logger.LogInformation("Azure Blob changed. Reloading the DB");
                await service.LoadAsync();
                // try again. 
                // TODO: This will try forever if it fails
                // should have expontential backoff waits here
                operation();
                await service.SaveAsync();
            }
        }
    }
}
