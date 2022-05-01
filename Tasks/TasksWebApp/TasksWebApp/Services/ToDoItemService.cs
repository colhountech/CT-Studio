namespace TasksWebApp.Services
{
    public record TodoItemData
    {
       
        public Guid ID { get; init; } = Guid.NewGuid();
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;
    }


    public class ToDoItemService
    {
    protected static readonly List<TodoItemData> ItemsDatabase = new List<TodoItemData>();


        public IEnumerable<TodoItemData> GetItems()
        {
            return ItemsDatabase.AsEnumerable();

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
            return ItemsDatabase.Remove(item);
        }
    }

}
