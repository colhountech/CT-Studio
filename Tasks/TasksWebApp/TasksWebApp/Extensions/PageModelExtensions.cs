using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Polly;
using System.Net;
using Tasks.Services;

namespace TasksWebApp.Pages.Extensions;

public static class PageModelExtensions
{
    public static async Task OptimisticConcurrencyControl
        (this PageModel pageModel, 
        Func<Task> operation, ITodoItemService service, ILogger logger)
    {
        // The sleep time doubles with each retry attempt, starting from 2 seconds.
        // We use a Func becuase it returns an int
        Func<int, TimeSpan> sleepTimeProvider = (retryCount) => TimeSpan.FromSeconds(Math.Pow(2, retryCount));
        
        // What we log for each retry
        // We use an Action instead of a Func because it returns void
        Action<Exception, TimeSpan, int, Context> onRetry = (exception, timeSpan, retryCount, context) =>
        {
            string? firstLine = exception.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None).FirstOrDefault();

            logger.LogWarning($"Warning: Attempt {retryCount} failed with exception: {firstLine}. Next retry in {timeSpan}.");
        };

         // Define the retry policy with exponential backoff
         var retryPolicy = Policy
           .Handle<RequestFailedException>(ex => ex.Status == (int)HttpStatusCode.PreconditionFailed)
           .WaitAndRetryAsync(5, sleepTimeProvider,onRetry);


        // Use the retry policy
         await retryPolicy.ExecuteAsync( async () =>
        {
            await service.LoadAsync();
            await operation();
            await service.SaveAsync();
        });
    }
}
