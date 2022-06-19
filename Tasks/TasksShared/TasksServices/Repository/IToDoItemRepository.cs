using System;
using TasksAppData;

namespace TasksServices.Repository
{
    public interface IToDoItemRepository
    {
        Task ValidateSourceAsync();
        Task<List<TodoItemData>?> RestoreData();
        Task StoreData(List<TodoItemData>? db);
    }
}

