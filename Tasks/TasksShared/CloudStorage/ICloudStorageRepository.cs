using TasksAppData;

namespace CloudStorage
{
    public interface ICloudStorageRepository
    {
        Task ValidateSourceAsync();
        Task StoreBlobAsync(List<TodoItemData> blob);
        Task<List<TodoItemData>?> RestoreBlobAsync();
    }
}