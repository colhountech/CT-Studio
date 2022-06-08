using System;
using TasksAppData;

namespace TasksServices.Repository
{
    public interface IToDoItemRepository
    {
        Task<List<TodoItemData>> RestoreData();
        Task StoreData(List<TodoItemData>? db);
    }
}

