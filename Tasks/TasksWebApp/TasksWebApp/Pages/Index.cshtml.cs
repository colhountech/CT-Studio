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

        }

        public void OnGet()
        {
            var items = _service.GetItems();

            // Map TodoItemData --> TodoItemViewModel      
            this.TodoItems = _mapper.Map<List<TodoItemViewModel>>(_service.GetItems());         
        }
    }
}