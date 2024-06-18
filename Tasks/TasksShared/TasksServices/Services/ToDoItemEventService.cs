
using CloudStorage;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using TasksAppData;
using TasksServices.Repository;

namespace TasksServices.Services
{
    // Next commit, this files gets renamed to TodoItemService

    public class TodoItemEventService : ITodoItemEventService
    {
        private readonly ILogger<TodoItemEventService> _logger;
        private readonly ICloudStorageRepository<TodoItemData> _cloudStorageRepository;

        public TodoItemEventService(
            ILogger<TodoItemEventService> logger,
            ICloudStorageRepository<TodoItemData> cloudStorageRepository
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
            CmdAdditem<TodoItemData> cmd = new CmdAdditem<TodoItemData>(item);
            //await _cloudStorageRepository.Send(cmd);
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
