
using TasksAppData;

namespace TasksWebApp.Services
{
    public interface IToDoItemService
    {
        Task<Guid> AddItemAsync(TodoItemData item);
        Task DeleteItemAsync(TodoItemData item);
        TodoItemData? GetItemByID(Guid ID);
        IEnumerable<TodoItemData> GetItems(bool archived);
        Task<bool> UpdateItemAsync(Guid oldID, TodoItemData item);
        Task<bool> AddItemMessageAsync(Guid itemID, MessageData message);
        Task<bool> MarkItemMessageRead(Guid itemID, Guid MessageID);

    }
}