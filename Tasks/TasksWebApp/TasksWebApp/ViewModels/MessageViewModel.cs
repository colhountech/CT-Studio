namespace TasksWebApp.ViewModels
{
   public record MessageViewModel
    {
        // The parent TodoItem that the message is attached to
        public Guid TodoItemID { get; set; } = default!;
        public Guid ID { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;
        public string DateCreated { get; set; } = default!;
        public bool UnRead { get; set; } = default!;

    }
}
