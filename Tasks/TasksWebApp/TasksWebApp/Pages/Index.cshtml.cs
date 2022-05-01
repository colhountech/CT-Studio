using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksWebApp.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<TodoItemViewModel> TodoItems = new List<TodoItemViewModel>();
        public ToDoItemService service = new ToDoItemService();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            if (! service.GetItems().Any())
            {

                service.AddItem(new TodoItemData { Title = "Tast Item 1", Description = "This is the first thing on my todo list" });
                service.AddItem(new TodoItemData { Title = "Tast Item 2", Description = "This is the second thing on my todo list" });
                service.AddItem(new TodoItemData { Title = "Tast Item 3", Description = "This is the third thing on my todo list" });
                service.AddItem(new TodoItemData { Title = "Tast Item 4", Description = "This is the fourth thing on my todo list" });
            }
        }

        public void OnGet()
        {
            var items = service.GetItems();

            // Map TodoItemData --> TodoItem ViewModel

            // Every Time you refresh, you get 4 more items added
            // this is not the place to setup the list

            foreach (var item in items)
            {
                TodoItems.Add(new TodoItemViewModel  ( item.Title, item.Description ));
            }
         
        }
    }
}