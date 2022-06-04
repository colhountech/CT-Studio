namespace CloudStorage
{
    public interface ICloudStorageRepository
    {
        Task StoreBlobAsync(MyMessage message);
        Task<MyMessage> RestoreBlobAsync();
    }
}