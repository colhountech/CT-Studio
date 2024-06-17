using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoMapper;
using TasksServices.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<TodoItemViewModel> TodoItems = new List<TodoItemViewModel>();

        private readonly ILogger<IndexModel> _logger;
        private readonly ITodoItemService _service;
        private readonly IMapper _mapper;

        public IndexModel(ILogger<IndexModel> logger, ITodoItemService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper; 

        }

        public async Task OnGetAsync()
        {
            await _service.LoadAsync();
            var items = _service.GetItems(archived: false);

            // Map TodoItemData --> TodoItemViewModel      
            this.TodoItems = _mapper.Map<List<TodoItemViewModel>>(items.OrderBy(x => x.Order));         
        }

    }
}