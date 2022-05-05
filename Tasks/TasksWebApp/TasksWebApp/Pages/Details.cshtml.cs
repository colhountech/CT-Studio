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

            LoadDummyMessage(ID.Value);

            return Page();
        }

        private void LoadDummyMessage(Guid ID)
        {
            var m = new MessageViewModel
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
                Subject = @"Isn't this cool?",
                Body = "This is a super cool message.",
                DateCreated = "11 minutes ago",
                UnRead = false
            };

            Messages.Add(m);
            Messages.Add(read);
        }
    }
}
