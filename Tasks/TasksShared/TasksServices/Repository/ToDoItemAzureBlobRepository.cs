using System;
using Azure;
using CloudStorage;
using Microsoft.Extensions.Logging;
using TasksAppData;

namespace TasksServices.Repository
{
	public class TodoItemAzureBlobRepository : ITodoItemRepository
	{

        private readonly ILogger<TodoItemAzureBlobRepository> _logger;
        private readonly ICloudStorageRepository _cloudStorageRepository;
        private static bool validated = false;
        private string myEtag = string.Empty;


        public TodoItemAzureBlobRepository(
            ILogger<TodoItemAzureBlobRepository> logger,
            ICloudStorageRepository cloudStorageRepository
            )
		{
            _logger = logger;
            _cloudStorageRepository = cloudStorageRepository;
		}

        /// <summary>
        /// Check that the Database Souce Exists and is Valid.
        /// Application Exceptions are caught in the ExceptionHandler
        /// and are handled by the /Error Razor page.
        /// Is only checked on startup
        /// </summary>
        /// <exception cref="ApplicationException">Throws an Application Exception with
        /// a human readable message for the end user if source is not valid</exception>
        public async Task ValidateSourceAsync()
        {
            if (!validated)
            {
                await _cloudStorageRepository.ValidateSourceAsync();
                validated = true;
            }
        }

        public async Task<List<TodoItemData>?> RestoreDataAsync()
        {
            return await _cloudStorageRepository.RestoreBlobAsync();
        }

        public async Task StoreDataAsync(List<TodoItemData>? db)
        {
            if (db is not null) await _cloudStorageRepository.StoreBlobAsync(db);
        }
    }
}

