using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksAppData;
using TasksWebApp.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IToDoItemService _service;
        private readonly IMapper _mapper;

        public EditModel(ILogger<IndexModel> logger, IToDoItemService service, IMapper mapper)
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
                var oldData = _service.GetItemByID(TodoItem.ID);
                if (oldData is null) return Page(); // edited item does not exist
                var itemData = _mapper.Map<TodoItemData>(TodoItem) with { Messages = oldData.Messages };
                await _service.UpdateItemAsync(TodoItem.ID, itemData);

            return RedirectToPage("Details", new { id = itemData?.ID });
        }
        return RedirectToPage("Index");
    }
}
}