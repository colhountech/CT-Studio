using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksWebApp.Services;
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

        public List<MessageViewModel> Messages = new List<MessageViewModel>();

        public IActionResult OnGet(Guid? ID)
        {
            TodoItem = LoadItem(ID);

            if (TodoItem is null)
            {
                return NotFound();
            }
            LoadDummyMessage(ID.Value, "unread");
            return Page();
        }

        public IActionResult OnPostUnreadAsync(Guid? ID)
        {
          
            TodoItem = LoadItem(ID);

            if (TodoItem is null)
            {
                return NotFound();
            }
            LoadDummyMessage(ID.Value, "unread");
            return Page();
        }

        public IActionResult OnPostReadAsync(Guid? ID)
        {
            TodoItem = LoadItem(ID);

            if (TodoItem is null)
            {
                return NotFound();
            }
            LoadDummyMessage(ID.Value, "read");
            return Page();
        }

        public IActionResult OnPostMessageAsync(Guid? ID)
        {
            if (!ModelState.IsValid)
            {
                return Page(); // This does not hold ID so won't load details
            }
            TodoItem = LoadItem(ID);

            if (TodoItem is null)
            {
                return NotFound();
            }
            // Check did Item bind correctly
            // check did Message bid correctly
            // Add Message to Item
            // Save Item
            // Redirect to Page("Details");
            return RedirectToPage("Details", new { id = ID });
        }
        

        private  TodoItemViewModel? LoadItem(Guid? ID)
        {
            if (ID == null)
            {
                return null;
            }

            var itemData = _service.GetItemByID(ID.Value);

            if (itemData == null)
            {
                return null;
            }

            return _mapper.Map<TodoItemViewModel>(itemData);

        }

        private void LoadDummyMessage(Guid ID, string filter = "unread" )
        {

            var unread = new MessageViewModel
            {
                ID = Guid.NewGuid(),
                TodoItemID = ID,
                Subject = @"Isn't this cool?",
                Body = "This is a super cool message.",
                DateCreated = "11 minutes ago",
                UnRead = true
            };
            var read = new MessageViewModel
            {
                ID = Guid.NewGuid(),
                TodoItemID = ID,
                Subject = @"This is cool too!",
                Body = "This is a super cool message.",
                DateCreated = "11 minutes ago",
                UnRead = false
            };


            switch (filter)
            {
              
                case "read":
                    
                    {
                        Messages.Add(read);
                        break;
                    }
                case "unread":
                default:
                    {
                        Messages.Add(unread);
                        break;
                    }
            }
        }
    }
}
