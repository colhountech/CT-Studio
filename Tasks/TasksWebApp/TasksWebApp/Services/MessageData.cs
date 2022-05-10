namespace TasksWebApp.Services
{
    public record MessageData 
    {
        // The TodoItemData that the message is Attached
        public Guid TodoItemID { get; set; } = default!;
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;
        public string DateCreated { get; set; } = default!;
        public bool UnRead { get; set; } = default!;

    }
}
