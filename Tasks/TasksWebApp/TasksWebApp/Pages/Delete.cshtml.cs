using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksWebApp.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class DeleteModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        private readonly IToDoItemService _service;
        private readonly IMapper _mapper;

        public DeleteModel(ILogger<IndexModel> logger, IToDoItemService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public TodoItemViewModel? TodoItem { get; set; }


        public IActionResult OnGet(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var itemData = _service.GetItemByID(ID.Value);

            if (itemData == null)
            {
                return NotFound();
            }

            TodoItem = _mapper.Map<TodoItemViewModel>(itemData);

            return Page();

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
                 _service.DeleteItem(itemData);
            }


            return RedirectToPage("Index");
        }
    }
}