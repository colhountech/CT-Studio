
namespace TasksWebApp.Services
{
    public interface IToDoItemService
    {
        Guid AddItem(TodoItemData item);
        bool DeleteItem(TodoItemData item);
        TodoItemData? GetItemByID(Guid ID);
        IEnumerable<TodoItemData> GetItems(bool archived);
        bool UpdateItem(Guid oldID, TodoItemData item);
    }
}