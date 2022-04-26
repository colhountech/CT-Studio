using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<TodoItem> todoItems = new List<TodoItem>();


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            todoItems.Add(new TodoItem("Task Item 1", "This is the first thing on my todo list"));
            todoItems.Add(new TodoItem("Task Item 2", "This is the second thing on my todo list"));
            todoItems.Add(new TodoItem("Task Item 3", "This is the third thing on my todo list"));
            todoItems.Add(new TodoItem("Task Item 4", "This is the fourth thing on my todo list"));

        }

        public void OnGet()
        {

        }
    }
}