using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using TasksAppData;

namespace CloudStorage
{
    public class AzureStorageRepository : ICloudStorageRepository
    {
        private readonly ILogger<AzureStorageRepository> _logger;
        private readonly IConfiguration _config;
        private readonly string _azureConnectionString;
        private readonly string _azureContainer;
        private readonly string _azureBlobStore;
        private static Azure.ETag _eTag;

        public AzureStorageRepository(
            ILogger<AzureStorageRepository> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _azureConnectionString = _config["AppConfig:AzureConnectionString"];
            _azureContainer = _config["AppConfig:AzureContainer"];
            _azureBlobStore = _config["AppConfig:AzureBlobStore"];
        }

        public async Task StoreBlobAsync(List<TodoItemData> blob)
        {
            BlobUploadOptions blobUploadOptions = setETag(_eTag);

            BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureContainer);
            await container.CreateIfNotExistsAsync();
            BlobClient blobClient = container.GetBlobClient(_azureBlobStore);
            var json = JsonSerializer.Serialize<List<TodoItemData>>(blob);
            try
            {               
                var azureResponse = await blobClient.UploadAsync(BinaryData.FromString(json), blobUploadOptions);
                _eTag = azureResponse.Value.ETag;
            }
            catch (RequestFailedException e)
            {
                if (e.Status == (int)HttpStatusCode.PreconditionFailed)
                {
                    _logger.LogError($"Blob's ETag does not match ETag provided.");
                }
            }
        }

        public async Task<List<TodoItemData>?> RestoreBlobAsync()
        {
            BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureContainer);
            BlobClient blobClient = container.GetBlobClient(_azureBlobStore);
            var azureResponse = await blobClient.DownloadStreamingAsync();
            _eTag = azureResponse.Value.Details.ETag;
            using (Stream downloadStream = azureResponse.Value.Content)
            {
                var options = new JsonSerializerOptions();
                var blob = await JsonSerializer.DeserializeAsync<List<TodoItemData>>(downloadStream, options);
                _logger.LogTrace($"Restored Message: Got ({blob?.Count}) Items");
                return blob;
            }
        }
        private BlobUploadOptions setETag(Azure.ETag eTag)
        {
            return new BlobUploadOptions()
            {
                Conditions = new BlobRequestConditions()
                {
                    IfMatch = eTag
                }
            };
        }
    }
}