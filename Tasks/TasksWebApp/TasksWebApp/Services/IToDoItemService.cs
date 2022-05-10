
namespace TasksWebApp.Services
{
    public interface IToDoItemService
    {
        Task<Guid> AddItem(TodoItemData item);
        Task DeleteItem(TodoItemData item);
        TodoItemData? GetItemByID(Guid ID);
        IEnumerable<TodoItemData> GetItems(bool archived);
        Task<bool> UpdateItem(Guid oldID, TodoItemData item);
        Task<bool> AddItemMessage(Guid itemID, MessageData message);
        Task<bool> SeItemMessageRead(Guid itemID, Guid MessageID);

    }
}