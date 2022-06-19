using System.Diagnostics;
using Azure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TasksDesktopApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string? ExceptionMessage { get; set; }

    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    // this will only get hit on Proudction Page
    // so How do you test this in Development code??

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var exHPF = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exHPF?.Error is ApplicationException)
       
        {
            ExceptionMessage ??= string.Empty;
            ExceptionMessage += $" Something Went Wrong: {exHPF.Error.Message}";

           
        }
        if (exHPF?.Path == "/")
        {
            ExceptionMessage += " Page: Home.";
        }
        else
        {
            ExceptionMessage += $" Page: {exHPF?.Path}";
        }

        _logger.LogCritical($"Request Id: {RequestId}  {ExceptionMessage ?? string.Empty}");
    }
}


