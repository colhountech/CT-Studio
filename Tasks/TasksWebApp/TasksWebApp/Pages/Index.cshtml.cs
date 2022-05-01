using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksWebApp.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<TodoItemViewModel> TodoItems = new List<TodoItemViewModel>();

        private readonly ILogger<IndexModel> _logger;
        private readonly IToDoItemService _service;
        private readonly IMapper _mapper;

        public IndexModel(ILogger<IndexModel> logger, IToDoItemService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper; 


            if (! service.GetItems().Any())
            {
                _service.AddItem(new TodoItemData { Title = "Tast Item 1", Description = "This is the first thing on my todo list" });
                _service.AddItem(new TodoItemData { Title = "Tast Item 2", Description = "This is the second thing on my todo list" });
                _service.AddItem(new TodoItemData { Title = "Tast Item 3", Description = "This is the third thing on my todo list" });
                _service.AddItem(new TodoItemData { Title = "Tast Item 4", Description = "This is the fourth thing on my todo list" });
            }
        }

        public void OnGet()
        {
            var items = _service.GetItems();

            // Map TodoItemData --> TodoItem ViewModel      
            this.TodoItems = _mapper.Map<List<TodoItemViewModel>>(_service.GetItems());         
        }
    }
}