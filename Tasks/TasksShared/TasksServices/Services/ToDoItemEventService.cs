
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

        private static bool IsAvailable { get; set; } = false;


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

        private async Task LoadAsync()
        {
            _toDoItemRepository.ValidateSourceAsync().GetAwaiter().GetResult();

            var db = await _toDoItemRepository.RestoreData();
            _logger.LogInformation($"Restored ({db?.Count}) items to db from Blob.");
            if (db is not null) ItemsDatabase = db;
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

        public async Task AddItemAsync(TodoItemData item)
        {
            ItemsDatabase.Add(item);
            await SaveAsync();
        }

        public async Task UpdateItemAsync(Guid oldID, TodoItemData item)
        {
            var oldItem = ItemsDatabase.Find(x => x.ID == oldID);

            if (oldItem is not null)
            {
                // Removes old item and replace with new item
                ItemsDatabase.Remove(oldItem);
                ItemsDatabase.Add(item);
                await SaveAsync();
            }
        }

        public async Task DeleteItemAsync(TodoItemData item)
        {
            var oldItem = ItemsDatabase.Find(x => x.ID == item.ID);
            if (oldItem is not null)
            {
                var newItem = oldItem with { Archived = true };
                var updateItem = UpdateItemAsync(oldItem.ID, newItem);
                await SaveAsync();
            }
        }

        public async Task AddItemMessageAsync(Guid itemID, MessageData message)
        {
            var item = ItemsDatabase.Find(x => x.ID == itemID);

            if (item is not null)
            {
                message.TodoItemID = item.ID;
                item.Messages.Add(message);
                await SaveAsync();
            }

        }

        public async Task MarkItemMessageRead(Guid itemID, Guid MessageID)
        {
            var item = ItemsDatabase.Find(x => x.ID == itemID);

            if (item is null) return;

            var oldMessage = item.Messages.Where(x => x.ID == MessageID).FirstOrDefault();

            if (oldMessage is not null)
            { 
                var newMessage = oldMessage with { UnRead = false };
                item.Messages.Remove(oldMessage);
                item.Messages.Add(newMessage);
                await SaveAsync();
            }
        }

        public async Task UpdateItems(List<TodoItemData> items)
        {
            foreach (var item in items)
            {
                var oldItem = ItemsDatabase.Find(x => x.ID == item.ID);

                if (oldItem is not null)
                {
                    ItemsDatabase.Remove(oldItem);
                    ItemsDatabase.Add(item);
                }
            }
            await SaveAsync();

        }
    }

}
