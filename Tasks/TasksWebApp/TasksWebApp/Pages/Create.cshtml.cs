using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksWebApp.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IToDoItemService _service;

        public CreateModel(ILogger<IndexModel> logger, IToDoItemService service)
        {
            _logger = logger;
            _service = service;
        }


        [BindProperty]
        public TodoItemViewModel? TodoItem { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            return RedirectToPage("Index");
        }
    }
}
