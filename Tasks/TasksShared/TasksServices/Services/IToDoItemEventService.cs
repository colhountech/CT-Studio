using TasksAppData;

namespace TasksServices.Services
{
    public interface ITodoItemEventService
    {
        // Queries
        Task<IEnumerable<TodoItemData>> GetItemsAsync(bool archived);

        // Commands
        Task AddItemAsync(TodoItemData item);

    }
}
