using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CloudStorage
{
    public class AzureStorageRepository : ICloudStorageRepository
    {
        private readonly ILogger<AzureStorageRepository> _logger;
        private readonly IConfiguration _config;
        private string _azureConnectionString;
        private string _azureBlobStore;
        

        public AzureStorageRepository(
            ILogger<AzureStorageRepository> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _azureConnectionString = _config["AppConfig:AzureConnectionString"];
            _azureBlobStore = _config["AppConfig:AzureBlobStore"];
        }


        public async Task StoreBlobAsync(MyMessage message)
        {
            BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureBlobStore);
            await container.CreateIfNotExistsAsync();
            BlobClient blobClient = container.GetBlobClient(_azureBlobStore);     
            var json = JsonSerializer.Serialize<MyMessage>(message);
            await blobClient.UploadAsync(BinaryData.FromString(json), overwrite: true);
        }

        public async Task<MyMessage> RestoreBlobAsync()
        {
            BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureBlobStore);
            BlobClient blobClient = container.GetBlobClient(_azureBlobStore);
            using (Stream downloadStream = (await blobClient.DownloadStreamingAsync()).Value.Content)
            {
                var options = new JsonSerializerOptions();
                var myMessage = await JsonSerializer.DeserializeAsync<MyMessage>(downloadStream, options);
                _logger.LogTrace($"Restored Message:{myMessage?.Subject} {myMessage?.Body}");
                return myMessage;
            }
        }
    }
}