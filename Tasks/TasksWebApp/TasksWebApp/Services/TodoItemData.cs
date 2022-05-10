namespace TasksWebApp.Services
{
    public record TodoItemData
    {
       
        public Guid ID { get; init; } = Guid.NewGuid();
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;
        public bool Archived { get; set; } = default!;
        public int Order { get; set; } = default!;

        public virtual ICollection<MessageData> Messages { get; set; }

        public TodoItemData()
        {
            Messages = new List<MessageData>();
        }
    }
}
