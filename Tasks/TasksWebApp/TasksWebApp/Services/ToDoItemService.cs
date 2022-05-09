
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TasksWebApp.Services
{


    public class ToDoItemService : IToDoItemService
    {
        protected static List<TodoItemData> ItemsDatabase = new List<TodoItemData>();


        public ToDoItemService()
        {

            FileInfo fi = new FileInfo(_path);
            if (!fi.Exists)
            {
                SetupDummyData();
            }
            else
            {
                LoadAsync();
            }
        }

        private void SetupDummyData()
        {
            // This is a new database. Setup some dummy data 
            AddItem(new TodoItemData { Title = "Tast Item 1", Description = "This is the first thing on my todo list" });
            AddItem(new TodoItemData { Title = "Tast Item 2", Description = "This is the second thing on my todo list" });
            AddItem(new TodoItemData { Title = "Tast Item 3", Description = "This is the third thing on my todo list" });
            AddItem(new TodoItemData { Title = "Tast Item 4", Description = "This is the fourth thing on my todo list" });
            SaveAsync();
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
                    ItemsDatabase = db;
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
            catch (Exception ex)
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

        public async Task<Guid> AddItem(TodoItemData item)
        {
            ItemsDatabase.Add(item);
            await SaveAsync();
            return item.ID;

        }

        // Removes old item by ID and replace with new item
        public async Task<bool> UpdateItem(Guid oldID, TodoItemData item)
        {
            // find old item
            var oldItem = ItemsDatabase.Find(x => x.ID == oldID);

            if (oldItem != null)
            {
                // only if we found item
                ItemsDatabase.Remove(oldItem);
                item.Messages = oldItem.Messages;
                ItemsDatabase.Add(item);
                await SaveAsync();
                return true;
            }
            // didn't find item, cant update
            return false;
        }

        public async Task DeleteItem(TodoItemData item)
        {
            // find old item
            var oldItem = ItemsDatabase.Find(x => x.ID == item.ID);
            // this will lose future any Mesages or other properties added in the future
            // var newItem = new TodoItemData { Archived = true, ID = item.ID, Title = item.Title, Description = item.Description};
            // instead use  with {} 
            var newItem = oldItem with { Archived = true };
            var updateItem =  UpdateItem(oldItem.ID, newItem);
            await SaveAsync();
            //return updateItem;
            //return ItemsDatabase.Remove(item);

        }
        public async Task<bool> AddItemMessage(Guid itemID, MessageData message)
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
      
        public async Task<bool> SeItemMessageRead(Guid itemID, Guid MessageID)
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
    }

}
