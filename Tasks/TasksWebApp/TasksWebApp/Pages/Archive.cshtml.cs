using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksServices.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class ArchiveModel : PageModel
    {
        public List<TodoItemViewModel> TodoItems = new List<TodoItemViewModel>();

        private readonly ILogger<IndexModel> _logger;
        private readonly ITodoItemService _service;
        private readonly IMapper _mapper;

        public ArchiveModel(ILogger<IndexModel> logger, ITodoItemService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper; 

        }

        public void OnGet()
        {
            var items = _service.GetItems(archived: true);

            // Map TodoItemData --> TodoItemViewModel      
            this.TodoItems = _mapper.Map<List<TodoItemViewModel>>(items);

        }
    }
}