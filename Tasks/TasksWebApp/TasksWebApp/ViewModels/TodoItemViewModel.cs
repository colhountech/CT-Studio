namespace TasksWebApp.ViewModels
{
    public record TodoItemViewModel(Guid ID, string Title, string Description, bool Archived );

}