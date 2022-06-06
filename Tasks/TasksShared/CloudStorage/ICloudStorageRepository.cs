using TasksAppData;

namespace CloudStorage
{
    public interface ICloudStorageRepository
    {
        Task StoreBlobAsync(List<TodoItemData> blob);
        Task<List<TodoItemData>?> RestoreBlobAsync();
    }
}