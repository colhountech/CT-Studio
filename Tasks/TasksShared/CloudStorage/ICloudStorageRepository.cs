using System.Reflection.Metadata;
using TasksAppData;

namespace CloudStorage
{
    public interface ICloudStorageRepository
    {
        Task ValidateSourceAsync();
        Task<bool> StoreBlobAsync(List<TodoItemData> blob);
        Task<List<TodoItemData>> RestoreBlobAsync();

        // Commands
        Task Send(ICmd cmd);
    }
}