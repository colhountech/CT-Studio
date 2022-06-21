using TasksAppData;

namespace TasksServices.Services
{
    public interface IToDoItemEventService
    {
        // Queries
        Task<IEnumerable<TodoItemData>> GetItemsAsync(bool archived);
    }
}
