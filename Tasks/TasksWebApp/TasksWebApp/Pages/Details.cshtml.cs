using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoMapper;
using TasksWebApp.ViewModels;
using TasksWebApp.Pages.Extensions;
using Tasks.Services;
using Tasks.AppData;

namespace TasksWebApp.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITodoItemService _service;
        private readonly IMapper _mapper;

        public DetailsModel(ILogger<IndexModel> logger, ITodoItemService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        public TodoItemViewModel? TodoItem { get; set; }

        [BindProperty]
        public MessageViewModel? Message { get; set; }

        public IEnumerable<MessageViewModel> Messages = new List<MessageViewModel>();

        public async Task<IActionResult> OnGetAsync(Guid? ID)
        {
            if (!await LoadItemAsync(ID))
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUnreadAsync(Guid? ID)
        {

            if (!await LoadItemAsync(ID))
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostReadAsync(Guid? ID)
        {
            if (!await LoadItemAsync(ID, unread: false))
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostMessageAsync(Guid? ID)
        {
            if (!ModelState.IsValid)
            {
                return Page(); // This does not hold ID so won't load details
            }

            if (ID == null)
            {
                return NotFound();
            }

            var messageData = _mapper.Map<MessageData>(Message) with
            {
                UnRead = true,
                ID = Guid.NewGuid()
            };

            await this.OptimisticConcurrencyControl(
              async () => _service.AddItemMessage(ID.Value, messageData), 
              _service,
              _logger);


            return RedirectToPage("Details", new { id = ID });
        }


        /// <summary>
        /// Handles the Post when the Close x is clicked on a Message to mark it Read
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostCloseAsync(Guid? ID)
        {
            if (!await LoadItemAsync(ID))
            {
                return NotFound();
            }

            await this.OptimisticConcurrencyControl(
            async () => _service.MarkItemMessageRead(ID!.Value, Message!.ID),
            _service,
            _logger);


            //if (!ok) return NotFound();

            return RedirectToPage("Details", new { id = ID });

        }


        private async  Task<bool> LoadItemAsync(Guid? ID, bool unread = true)
        {
            if (ID == null)
            {
                return false;
            }

            await _service.LoadAsync();

            var itemData = _service.GetItemByID(ID.Value);

            if (itemData == null)
            {
                return false;
            }
            TodoItem = _mapper.Map<TodoItemViewModel>(itemData);

            // Mapper does not map Messages because there is no Messages collection in the TodoItem ViewModel
            Messages = _mapper.Map<IEnumerable<MessageViewModel>>(
                itemData.Messages
                .Where(x => x.UnRead == unread)
            );

            return true;

        }

       
    }
}
