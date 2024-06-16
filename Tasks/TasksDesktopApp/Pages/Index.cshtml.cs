using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoMapper;
using TasksServices.Services;
using TasksDesktopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace TasksDesktopApp.Pages;

public class IndexModel : PageModel
{
   
    private readonly ILogger<IndexModel> _logger;
    private readonly ITodoItemEventService _service;
    private readonly IMapper _mapper;

    public IndexModel(ILogger<IndexModel> logger, ITodoItemEventService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    public List<TodoItemViewModel> TodoItems { get; set; } = new List<TodoItemViewModel>();

    public async Task OnGetAsync()
    {
        var items = await _service.GetItemsAsync(archived: false);
        this.TodoItems = _mapper.Map<List<TodoItemViewModel>>(items.OrderBy(x => x.Order));
    }

  
}

