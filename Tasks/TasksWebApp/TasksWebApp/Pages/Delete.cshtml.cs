using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksAppData;
using TasksServices.Services;
using TasksWebApp.ViewModels;
using TasksWebApp.Pages.Extensions;

namespace TasksWebApp.Pages
{
    public class DeleteModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        private readonly ITodoItemService _service;
        private readonly IMapper _mapper;

        public DeleteModel(ILogger<IndexModel> logger, ITodoItemService service, IMapper mapper)
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
                var itemData = _mapper.Map<TodoItemData>(TodoItem);

                await this.OptimisticConcurrencyControl(
                   async () => _service.DeleteItem(itemData),
                   _service,
                   _logger);
            }


            return RedirectToPage("Index");
        }
    }
}