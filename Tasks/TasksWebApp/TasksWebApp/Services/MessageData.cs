namespace TasksWebApp.Services
{
    public record MessageData 
    {
        // The TodoItemData that the message is Attached
        public Guid TodoItemID { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;

    }
}
