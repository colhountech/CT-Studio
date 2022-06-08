using System;
using CloudStorage;
using Microsoft.Extensions.Logging;
using TasksAppData;

namespace TasksServices.Repository
{
	public class ToDoItemAzureBlobRepository : IToDoItemRepository
	{

        private readonly ILogger<ToDoItemAzureBlobRepository> _logger;
        private readonly ICloudStorageRepository _cloudStorageRepository;

        public ToDoItemAzureBlobRepository(
            ILogger<ToDoItemAzureBlobRepository> logger,
            ICloudStorageRepository cloudStorageRepository
            )
		{
            _logger = logger;
            _cloudStorageRepository = cloudStorageRepository;
		}

        public async Task<List<TodoItemData>> RestoreData()
        {
            var db = await _cloudStorageRepository.RestoreBlobAsync();
            if (db is null) throw (new ApplicationException("No Database: Check App Settings"));
            return db;
        }

        public async Task StoreData(List<TodoItemData>? db)
        {
            if (db is not null) await _cloudStorageRepository.StoreBlobAsync(db);
        }
    }
}

