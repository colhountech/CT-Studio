
using TasksAppData;

namespace TasksServices.Services
{
    public interface IToDoItemService
    {
        // Queries
        TodoItemData? GetItemByID(Guid ID);
        IEnumerable<TodoItemData> GetItems(bool archived);

        // Commands
        Task AddItemAsync(TodoItemData item);
        Task UpdateItemAsync(Guid oldID, TodoItemData item);
        Task UpdateItems(List<TodoItemData> items);
        Task DeleteItemAsync(TodoItemData item);
        Task AddItemMessageAsync(Guid itemID, MessageData message);
        Task MarkItemMessageRead(Guid itemID, Guid MessageID);

    }
}