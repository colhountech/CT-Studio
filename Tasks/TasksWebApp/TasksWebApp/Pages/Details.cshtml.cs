using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoMapper;
using TasksAppData;
using TasksServices.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IToDoItemService _service;
        private readonly IMapper _mapper;

        public DetailsModel(ILogger<IndexModel> logger, IToDoItemService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        public TodoItemViewModel? TodoItem { get; set; }

        [BindProperty]
        public MessageViewModel? Message { get; set; }

        public IEnumerable<MessageViewModel> Messages = new List<MessageViewModel>();

        public IActionResult OnGet(Guid? ID)
        {

            if (!LoadItem(ID))
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostUnreadAsync(Guid? ID)
        {
          
            if (!LoadItem(ID))
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostReadAsync(Guid? ID)
        {

            if (!LoadItem(ID, unread: false))
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
            _service.AddItemMessage(ID.Value, messageData);
            await _service.SaveAsync();
            return RedirectToPage("Details", new { id = ID });
        }


        /// <summary>
        /// Handles the Post when the Close x is clicked on a Message to mark it Read
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostCloseAsync(Guid? ID)
        {
            if (!LoadItem(ID))
            {
                return NotFound();
            }

            _service.MarkItemMessageRead(ID!.Value, Message!.ID);
            await _service.SaveAsync();

            //if (!ok) return NotFound();

            return RedirectToPage("Details", new { id = ID });

        }


        private  bool LoadItem(Guid? ID, bool unread = true)
        {
            if (ID == null)
            {
                return false;
            }

            var itemData = _service.GetItemByID(ID.Value);

            if (itemData == null)
            {
                return false;
            }
            TodoItem = _mapper.Map<TodoItemViewModel>(itemData);

            // Mapper does not map Messages because there is no Messages collection in the todoItem ViewModel
            Messages = _mapper.Map<IEnumerable<MessageViewModel>>(
                itemData.Messages
                .Where(x => x.UnRead == unread)
            );

            return true;

        }

       
    }
}
