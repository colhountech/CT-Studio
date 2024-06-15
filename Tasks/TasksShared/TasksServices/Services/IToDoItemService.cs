
using TasksAppData;

namespace TasksServices.Services
{
    public interface IToDoItemService
    {
        // Queries
        TodoItemData? GetItemByID(Guid ID);
        IEnumerable<TodoItemData> GetItems(bool archived);

        // Commands
        void AddItem(TodoItemData item);
        void UpdateItem(Guid oldID, TodoItemData item);
        void UpdateItems(List<TodoItemData> items);
        void DeleteItem(TodoItemData item);
        void AddItemMessage(Guid itemID, MessageData message);
        void MarkItemMessageRead(Guid itemID, Guid MessageID);

        Task LoadAsync();

        Task SaveAsync();
    }
}