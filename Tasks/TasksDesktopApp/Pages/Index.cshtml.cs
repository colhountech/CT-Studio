using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoMapper;
using TasksServices.Services;
using TasksDesktopApp.ViewModels;

namespace TasksDesktopApp.Pages;

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

        var items = _service.GetItems(archived: false);
        // Map TodoItemData --> TodoItemViewModel      
        this.TodoItems = _mapper.Map<List<TodoItemViewModel>>(items.OrderBy(x => x.Order));
    }
}

