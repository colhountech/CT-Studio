namespace TasksWebApp.Services
{


    public class ToDoItemService : IToDoItemService
    {
        protected static readonly List<TodoItemData> ItemsDatabase = new List<TodoItemData>();

        public ToDoItemService()
        {
            AddItem(new TodoItemData { Title = "Tast Item 1", Description = "This is the first thing on my todo list" });
            AddItem(new TodoItemData { Title = "Tast Item 2", Description = "This is the second thing on my todo list" });
            AddItem(new TodoItemData { Title = "Tast Item 3", Description = "This is the third thing on my todo list" });
            AddItem(new TodoItemData { Title = "Tast Item 4", Description = "This is the fourth thing on my todo list" });

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

        public Guid AddItem(TodoItemData item)
        {
            ItemsDatabase.Add(item);
            return item.ID;

        }

        // Removes old item by ID and replace with new item
        public bool UpdateItem(Guid oldID, TodoItemData item)
        {
            // find old item
            var oldItem = ItemsDatabase.Find(x => x.ID == oldID);

            if (oldItem != null)
            {
                // only if we found item
                ItemsDatabase.Remove(oldItem);
                ItemsDatabase.Add(item);
                return true;
            }
            // didn't find item, cant update
            return false;
        }

        public bool DeleteItem(TodoItemData item)
        {
            // find old item
            var oldItem = ItemsDatabase.Find(x => x.ID == item.ID);
            var newItem = new TodoItemData { Archived = true, ID = item.ID, Title = item.Title, Description = item.Description};
            return UpdateItem(oldItem.ID, newItem);
            //return ItemsDatabase.Remove(item);

        }
    }

}
