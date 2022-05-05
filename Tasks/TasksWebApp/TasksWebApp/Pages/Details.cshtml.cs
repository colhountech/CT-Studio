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

        [BindProperty]
        public TodoItemViewModel? TodoItem { get; set; }

        public List<MessageViewModel> Messages = new List<MessageViewModel>();

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

        public IActionResult OnPostUnreadAsync(Guid? ID)
        {
            LoadDummyMessage(ID.Value, "unread");
            return OnGet(ID);

        }
        public IActionResult OnPostReadAsync(Guid? ID)
        {
            LoadDummyMessage(ID.Value, "read");
            LoadDummyMessage(ID.Value, "read");
            return OnGet(ID);
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
