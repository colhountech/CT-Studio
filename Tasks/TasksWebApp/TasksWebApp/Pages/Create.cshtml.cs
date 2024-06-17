using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using TasksAppData;
using TasksServices.Services;
using TasksWebApp.Pages.Extensions;
using TasksWebApp.ViewModels;
using Polly;
using Polly.Retry;

namespace TasksWebApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITodoItemService _service;
        private readonly IMapper _mapper;

        public CreateModel(ILogger<IndexModel> logger, ITodoItemService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;

           
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

            if (TodoItem != null)
            {
                var itemData = _mapper.Map<TodoItemData>(TodoItem);
                await this.OptimisticConcurrencyControl(
                async () => _service.AddItem(itemData),
                _service,
                _logger);
            }


            return RedirectToPage("Index");
        }
    }
}

