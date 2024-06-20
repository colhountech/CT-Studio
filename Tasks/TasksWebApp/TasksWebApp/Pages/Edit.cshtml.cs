using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoMapper;
using TasksWebApp.ViewModels;
using TasksWebApp.Pages.Extensions;
using Tasks.Services;
using Tasks.AppData;

namespace TasksWebApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITodoItemService _service;
        private readonly IMapper _mapper;

        public EditModel(ILogger<IndexModel> logger, ITodoItemService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public TodoItemViewModel? TodoItem { get; set; }


        public async Task<IActionResult> OnGetAsync(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            await _service.LoadAsync();
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
                await _service.LoadAsync();
                var oldData = _service.GetItemByID(TodoItem.ID);
                if (oldData is null) return Page(); // edited item does not exist
                var itemData = _mapper.Map<TodoItemData>(TodoItem) with { Messages = oldData.Messages };

                await this.OptimisticConcurrencyControl(
                async () =>  _service.UpdateItem(TodoItem.ID, itemData),
                _service,
                _logger);

                return RedirectToPage("Details", new { id = itemData?.ID });
        }
        return RedirectToPage("Index");
    }
}
}