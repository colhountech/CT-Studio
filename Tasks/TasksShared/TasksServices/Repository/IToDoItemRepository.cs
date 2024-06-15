using System;
using TasksAppData;

namespace TasksServices.Repository
{
    public interface IToDoItemRepository
    {
        Task ValidateSourceAsync();
        Task<List<TodoItemData>?> RestoreDataAsync();
        Task StoreDataAsync(List<TodoItemData>? db);
    }
}

