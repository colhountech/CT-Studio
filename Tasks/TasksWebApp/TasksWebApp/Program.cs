using CloudStorage;
using Microsoft.AspNetCore.Cors.Infrastructure;
using TasksServices.Repository;
using TasksServices.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITodoItemService, TodoItemService>();
builder.Services.AddTransient<ITodoItemRepository, TodoItemAzureBlobRepository>();
builder.Services.AddTransient<ICloudStorageRepository, AzureStorageRepository>();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapRazorPages();
app.Run();