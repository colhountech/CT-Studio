using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksAppData;
using TasksServices.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IToDoItemService _service;
        private readonly IMapper _mapper;

        public CreateModel(ILogger<IndexModel> logger, IToDoItemService service, IMapper mapper)
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
                await _service.AddItemAsync(itemData);
            }


            return RedirectToPage("Index");
        }
    }
}
