using System;
using TasksAppData;

namespace TasksServices.Repository
{
    public interface IToDoItemRepository
    {
        Task ValidateSourceAsync();
        Task<List<TodoItemData>?> RestoreDataAsync();
        Task StoreData(List<TodoItemData>? db);
    }
}

