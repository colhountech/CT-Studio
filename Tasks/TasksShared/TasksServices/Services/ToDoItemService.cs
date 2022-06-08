using System.Text.Json;
using System.Text.Json.Serialization;
using TasksAppData;

namespace TasksServices.Services
{

    // Next commit, this files gets deleted

    public class ToDoItemService : IToDoItemService
    {
        protected static List<TodoItemData> ItemsDatabase = new List<TodoItemData>();

        public ToDoItemService()
        {

            FileInfo fi = new FileInfo(_path); // TODO: Really don't want this check every time ctor run
            if (!fi.Exists)
            {
                SetupDummyData();
            }
            else
            {
                LoadAsync().GetAwaiter().GetResult();
            }
        }

        private async void SetupDummyData()
        {
            // This is a new database. Setup some dummy data 
            await AddItemAsync(new TodoItemData { Title = "Tast Item 1", Description = "This is the first thing on my todo list" });
            await AddItemAsync(new TodoItemData { Title = "Tast Item 2", Description = "This is the second thing on my todo list" });
            await AddItemAsync(new TodoItemData { Title = "Tast Item 3", Description = "This is the third thing on my todo list" });
            await AddItemAsync(new TodoItemData { Title = "Tast Item 4", Description = "This is the fourth thing on my todo list" });
            await SaveAsync();
        }

        private static readonly string _path = "Database.json";

        private async Task LoadAsync()
        {
            if (ItemsDatabase.Count == 0)
            {
                using (var fs = File.OpenRead(_path))
                {
                    var options = new JsonSerializerOptions();
                    var db = await JsonSerializer.DeserializeAsync<List<TodoItemData>>(fs, options);
                    if (db is not null) ItemsDatabase = db;
                }
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                var backup = $"{ _path }~";
                File.Move(_path, backup, true);
            }
            catch (Exception)
            {
                //   _logger.LogError($"CommitChangesAsync Backup FAILED: {ex.Message}");
            }

            using (var fs = File.Open(_path, FileMode.Create))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                };
                await JsonSerializer.SerializeAsync(fs, ItemsDatabase, options);
            }
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
            var oldItem  = ItemsDatabase.Find(x => x.ID == item.ID);
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
