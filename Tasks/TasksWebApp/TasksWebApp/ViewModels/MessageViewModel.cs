namespace TasksWebApp.ViewModels
{
   public record MessageViewModel
    {
        // The parent TodoItem that the message is attached to
        public Guid TodoItemID { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;

    }
}
