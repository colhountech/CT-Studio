
using Azure;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using TasksAppData;
using TasksServices.Repository;

namespace TasksServices.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ILogger<TodoItemService> _logger;
        private readonly ITodoItemRepository _TodoItemRepository;

        private bool IsDirty { get; set; } = false;

        protected List<TodoItemData> ItemsDatabase = new List<TodoItemData>();
  
        public TodoItemService(
            ILogger<TodoItemService> logger,
            ITodoItemRepository TodoItemRepository
            )
        {
            _logger = logger;
            _TodoItemRepository = TodoItemRepository;
           
            if (ItemsDatabase.Count == 0)
            {
                   LoadAsync().GetAwaiter().GetResult();
            }
        }

        public async Task LoadAsync()
        {
            await _TodoItemRepository.ValidateSourceAsync();
            var db = await _TodoItemRepository.RestoreDataAsync();
            _logger.LogInformation($"Restored ({db?.Count}) items to db from Blob.");
            if (db is not null) ItemsDatabase = db;
            IsDirty = false;

        }

        public async Task SaveAsync()
        {

            var db = ItemsDatabase;
            _logger.LogInformation($"Storing {db?.Count} items.");
            if (db is not null) await _TodoItemRepository.StoreDataAsync(db);
            // this could throw a contention exceptions
            IsDirty = false;

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

        public void AddItem(TodoItemData item)
        {
            ItemsDatabase.Add(item);
            IsDirty = true;
        }


        public void UpdateItem(Guid oldID, TodoItemData item)
        {
            var oldItem = ItemsDatabase.Find(x => x.ID == oldID);

            if (oldItem is not null)
            {
                // Removes old item and replace with new item
                ItemsDatabase.Remove(oldItem);
                ItemsDatabase.Add(item);
                IsDirty = true;
            }
        }

        public void DeleteItem(TodoItemData item)
        {
            var oldItem = ItemsDatabase.Find(x => x.ID == item.ID);
            if (oldItem is not null)
            {
                var newItem = oldItem with { Archived = true };
                UpdateItem(oldItem.ID, newItem);
            }
        }

        public void AddItemMessage(Guid itemID, MessageData message)
        {
            var item = ItemsDatabase.Find(x => x.ID == itemID);

            if (item is not null)
            {
                message.TodoItemID = item.ID;
                item.Messages.Add(message);
                IsDirty = true;
            }
        }

        public void MarkItemMessageRead(Guid itemID, Guid MessageID)
        {
            var item = ItemsDatabase.Find(x => x.ID == itemID);

            if (item is null) return;

            var oldMessage = item.Messages.Where(x => x.ID == MessageID).FirstOrDefault();

            if (oldMessage is not null)
            { 
                var newMessage = oldMessage with { UnRead = false };
                item.Messages.Remove(oldMessage);
                item.Messages.Add(newMessage);
                IsDirty = true;
            }
        }

        public void UpdateItems(List<TodoItemData> items)
        {
            foreach (var item in items)
            {
                var oldItem = ItemsDatabase.Find(x => x.ID == item.ID);

                if (oldItem is not null)
                {
                    ItemsDatabase.Remove(oldItem);
                    ItemsDatabase.Add(item);
                    IsDirty = true;
                }
            }
        }
    }
}
