using TasksWebApp.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IToDoItemService, ToDoItemService>();
var app = builder.Build();
app.UseStaticFiles().UseRouting().UseAuthorization();
app.MapRazorPages();
app.Run();