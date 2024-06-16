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
        // Queues
        private readonly string _azureQueueName;
        private QueueClient _client;
        private string _accountName;
        private Azure.ETag _eTag;



        public AzureStorageRepository(
            ILogger<AzureStorageRepository> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _azureConnectionString = _config["AppConfig:AzureConnectionString"] ?? string.Empty;
            _azureContainer = _config["AppConfig:AzureContainer"] ?? string.Empty;
            _azureBlobStore = _config["AppConfig:AzureBlobStore"] ?? string.Empty;
            _azureQueueName = _config["AppConfig:AzureQueueName"] ?? string.Empty;
            _client = new QueueClient(_azureConnectionString, _azureQueueName);
            _accountName = _client.AccountName;

        }
        public async Task InitAzureQueue()
        {
            await _client.CreateIfNotExistsAsync();
            _logger.LogDebug($"Queue Account : {_accountName}, Queue Name :{_azureQueueName} ");
        }


        public async Task ValidateSourceAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_azureConnectionString))
                    throw new ApplicationException("You don't have an AzureConnectionString defined");
                if (string.IsNullOrEmpty(_azureContainer))
                    throw new ApplicationException("You don't have a AzureContainer");
                if (string.IsNullOrEmpty(_azureBlobStore))
                    throw new ApplicationException("Your AzureBlobStore is missing");

                BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureContainer);
                BlobClient blobClient = container.GetBlobClient(_azureBlobStore);
                var exists = await blobClient.ExistsAsync();

                if (!exists)
                {
                    throw new ApplicationException("Can't find Your Database blob. Have you changed something?");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("You don't have valid Database", ex);
            }
        }

        public async Task<bool> StoreBlobAsync(List<TodoItemData> blob)
        {

            BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureContainer);
            var creatResonse = await container.CreateIfNotExistsAsync();
                
            BlobClient blobClient = container.GetBlobClient(_azureBlobStore);
            var json = JsonSerializer.Serialize<List<TodoItemData>>(blob);
            try
            {
                // if _eTag is different between Restore and Store we get a conflict exceptions
                BlobUploadOptions blobUploadOptions = setETag(_eTag);
                var azureResponse = await blobClient.UploadAsync(BinaryData.FromString(json), blobUploadOptions);
                _eTag = azureResponse.Value.ETag;
            }
            catch (RequestFailedException e)
            {
                if (e.Status == (int)HttpStatusCode.PreconditionFailed)
                {
                    _logger.LogError($"Blob has Changed : Blob's ETag does not match ETag provided.");
                    // We probably want to mark database as IsDirty and allow the
                    // calling app to fallback and try again
                }
                throw;

            }
            // Success
            return true; 
        }

        public async Task<List<TodoItemData>> RestoreBlobAsync()
        {
            BlobContainerClient container = new BlobContainerClient(_azureConnectionString, _azureContainer);
            BlobClient blobClient = container.GetBlobClient(_azureBlobStore);
            var azureResponse = await blobClient.DownloadStreamingAsync();
            // if _eTag is different between Restore and Store we get a conflict exceptions
            _eTag = azureResponse.Value.Details.ETag;
            using (Stream downloadStream = azureResponse.Value.Content)
            {
                var options = new JsonSerializerOptions();
                var blob = await JsonSerializer.DeserializeAsync<List<TodoItemData>>(downloadStream, options);
                _logger.LogTrace($"Restored Message: Got ({blob?.Count}) Items");
                return blob ?? new List<TodoItemData>();
            }
        }


        public async Task Send(ICmd cmd)
        {
            _logger.LogTrace($"AddToQueueAsync {_azureQueueName} ");

            var message = cmd switch
            {
                CmdAdditem<TodoItemData> cmdAddItem => JsonSerializer.Serialize(cmdAddItem),
                _ => throw new NotImplementedException($"ICmd type {cmd.GetType()} not known yet")
            };
            _logger.LogTrace($"Serialized message to {message}");

            await _client.SendMessageAsync(message);

            _logger.LogTrace($"AddToQueueAsync Done");
        }

        #region private methods
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
        #endregion
    }
}