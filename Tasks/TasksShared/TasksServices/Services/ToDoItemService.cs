
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using TasksAppData;
using TasksServices.Repository;

namespace TasksServices.Services
{
    // Next commit, this files gets renamed to ToDoItemService

    public class ToDoItemService : IToDoItemService
    {
        private readonly ILogger<ToDoItemService> _logger;
        private readonly IToDoItemRepository _toDoItemRepository;
        protected List<TodoItemData> ItemsDatabase = new List<TodoItemData>();
  
        public ToDoItemService(
            ILogger<ToDoItemService> logger,
            IToDoItemRepository toDoItemRepository
            )
        {
            _logger = logger;
            _toDoItemRepository = toDoItemRepository;
           
            // change from static, so db is loaded in memory on **every**  _service call
            if (ItemsDatabase.Count == 0)
            {
                LoadAsync().GetAwaiter().GetResult();
            }
        }

        private async void SetupDummyDataAsync()
        {
            // This is a new database. Setup some dummy data 
            await AddItemAsync(new TodoItemData { Title = "Tast Item 1", Description = "This is the first thing on my todo list" });
            await AddItemAsync(new TodoItemData { Title = "Tast Item 2", Description = "This is the second thing on my todo list" });
            await AddItemAsync(new TodoItemData { Title = "Tast Item 3", Description = "This is the third thing on my todo list" });
            await AddItemAsync(new TodoItemData { Title = "Tast Item 4", Description = "This is the fourth thing on my todo list" });
            await SaveAsync();
        }

        //private static readonly string _path = "Database.json";

        private async Task LoadAsync()
        {
           try
            {
                var db = await _toDoItemRepository.RestoreData();
                _logger.LogInformation($"Restored ({db?.Count}) items to db from Blob.");
                if (db is not null) ItemsDatabase = db;
            }
            catch (Azure.RequestFailedException ex)
            {                
                _logger.LogWarning($"LoadAsync: No Data: Uncomment LoadAsync() catch to setup Dummy Data: {ex.Message}");
                // new blob storage container is not the only reason that an exception happens
                //_logger.LogWarning($"LoadAsync: No Data: Setting up Dummy Data: {ex.Message}");
                //SetupDummyDataAsync();
                //await SaveAsync();
            }
        }
 
        private async Task SaveAsync()
        {
            var db = ItemsDatabase;
            _logger.LogInformation($"Storing {db?.Count} items");
            if (db is not null) await _toDoItemRepository.StoreData(db);
        }


        public IEnumerable<TodoItemData> GetItems(bool archived)
        {            
            return ItemsDatabase
                .Where(x => x.Archived == archived)
                .AsEnumerable();
        }
      
        public TodoItemData? GetItemByID(Guid ID)
        {
            return ItemsDatabase.FirstOrDefault(x => x.ID == ID);

        }

        public async Task<Guid> AddItemAsync(TodoItemData item)
        {
            ItemsDatabase.Add(item);
            await SaveAsync();
            return item.ID;
        }

        // Removes old item by ID and replace with new item
        public async Task<bool> UpdateItemAsync(Guid oldID, TodoItemData item)
        {
            // find old item
            var oldItem = ItemsDatabase.Find(x => x.ID == oldID);

            if (oldItem != null)
            {
                // only if we found item
                ItemsDatabase.Remove(oldItem);
                ItemsDatabase.Add(item);
                await SaveAsync();
                return true;
            }
            // didn't find item, cant update
            return false;
        }

        public async Task DeleteItemAsync(TodoItemData item)
        {
            // find old item
            var oldItem = ItemsDatabase.Find(x => x.ID == item.ID);
            // this will lose future any Mesages or other properties added in the future
            // var newItem = new TodoItemData { Archived = true, ID = item.ID, Title = item.Title, Description = item.Description};
            // instead use  with {} 
            if (oldItem is not null)
            {
                var newItem = oldItem with { Archived = true };
                var updateItem = UpdateItemAsync(oldItem.ID, newItem);
                await SaveAsync();
            }         
            //return updateItem;
            //return ItemsDatabase.Remove(item);

        }
        public async Task<bool> AddItemMessageAsync(Guid itemID, MessageData message)
        {
            var item = ItemsDatabase.Find(x => x.ID == itemID);

            if (item != null)
            {
                message.TodoItemID = item.ID;
                item.Messages.Add(message);
                await SaveAsync();
                return true;
            }
            // didn't find item, can't update
            return false;

        }

        public async Task<bool> MarkItemMessageRead(Guid itemID, Guid MessageID)
        {
            var item = ItemsDatabase.Find(x => x.ID == itemID);

            if (item is null) return false;

            var oldMessage = item.Messages.Where(x => x.ID == MessageID).FirstOrDefault();

            if (oldMessage is not null)
            { 
                var newMessage = oldMessage with { UnRead = false };
                item.Messages.Remove(oldMessage);
                item.Messages.Add(newMessage);
                //UpdateItem(itemID, item);
                await SaveAsync();
                return true;
            }
            // didn't find message, can't update
            return false;
        }

        public async Task UpdateItems(List<TodoItemData> items)
        {
            foreach (var item in items)
            {
                // find old item
                var oldItem = ItemsDatabase.Find(x => x.ID == item.ID);

                if (oldItem != null)
                {
                    // only if we found item
                    ItemsDatabase.Remove(oldItem);
                    ItemsDatabase.Add(item);
                }
            }
            await SaveAsync();

        }
    }

}
