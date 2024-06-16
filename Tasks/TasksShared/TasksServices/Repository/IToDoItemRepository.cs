using System;
using TasksAppData;

namespace TasksServices.Repository
{
    public interface ITodoItemRepository
    {
        Task ValidateSourceAsync();
        Task<List<TodoItemData>?> RestoreDataAsync();
        Task StoreDataAsync(List<TodoItemData>? db);
    }
}

