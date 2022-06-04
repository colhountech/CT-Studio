using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TasksAppData;

namespace CloudStorage
{
    public class AzureStorageRepository : ICloudStorageRepository
    {
        private readonly ILogger<AzureStorageRepository> _logger;
        private readonly IConfiguration _config;
        private readonly string _azureConnectionString;
        private readonly string _azureBlobStore;        

        public AzureStorageRepository(
            ILogger<AzureStorageRepository> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _azureConnectionString = _config["AppConfig:AzureConnectionString"];
            _azureBlobStore = _config["AppConfig:AzureBlobStore"];
        }


        public async Task StoreBlobAsync(List<TodoItemData> blob)
        {
            BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureBlobStore);
            await container.CreateIfNotExistsAsync();
            BlobClient blobClient = container.GetBlobClient(_azureBlobStore);     
            var json = JsonSerializer.Serialize<List<TodoItemData>>(blob);
            await blobClient.UploadAsync(BinaryData.FromString(json), overwrite: true);
        }

        public async Task<List<TodoItemData>?> RestoreBlobAsync()
        {
            BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureBlobStore);
            BlobClient blobClient = container.GetBlobClient(_azureBlobStore);
            using (Stream downloadStream = (await blobClient.DownloadStreamingAsync()).Value.Content)
            {
                var options = new JsonSerializerOptions();
                var blob = await JsonSerializer.DeserializeAsync<List<TodoItemData>>(downloadStream, options);
                _logger.LogTrace($"Restored Message: Got ({blob?.Count}) Items");
                return blob;
            }
        }
    }
}