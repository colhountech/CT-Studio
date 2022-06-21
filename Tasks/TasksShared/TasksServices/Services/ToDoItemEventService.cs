
using CloudStorage;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using TasksAppData;
using TasksServices.Repository;

namespace TasksServices.Services
{
    // Next commit, this files gets renamed to ToDoItemService

    public class ToDoItemEventService : IToDoItemEventService
    {
        private readonly ILogger<ToDoItemEventService> _logger;
        private readonly ICloudStorageRepository _cloudStorageRepository;

        public ToDoItemEventService(
            ILogger<ToDoItemEventService> logger,
            ICloudStorageRepository cloudStorageRepository
            )
        {
            _logger = logger;
            _cloudStorageRepository = cloudStorageRepository;
        }

          
        public async Task<IEnumerable<TodoItemData>> GetItemsAsync(bool archived)
        {
            return await _cloudStorageRepository.RestoreBlobAsync();
        }
      
        public TodoItemData? GetItemByID(Guid ID)
        {
            throw new NotImplementedException();

        }

        public async Task AddItemAsync(TodoItemData item)
        {
            throw new NotImplementedException();

        }

        public async Task UpdateItemAsync(Guid oldID, TodoItemData item)
        {
            throw new NotImplementedException();

        }

        public async Task DeleteItemAsync(TodoItemData item)
        {
            throw new NotImplementedException();

        }

        public async Task AddItemMessageAsync(Guid itemID, MessageData message)
        {
            throw new NotImplementedException();


        }

        public async Task MarkItemMessageRead(Guid itemID, Guid MessageID)
        {
            throw new NotImplementedException();

        }

        public async Task UpdateItems(List<TodoItemData> items)
        {
            throw new NotImplementedException();
        }
    }

}
