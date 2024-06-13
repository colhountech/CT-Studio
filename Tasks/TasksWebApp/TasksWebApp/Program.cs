using CloudStorage;
using Microsoft.AspNetCore.Cors.Infrastructure;
using TasksServices.Repository;
using TasksServices.Services;
using Microsoft.Extensions.Azure;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IToDoItemService, ToDoItemService>();
builder.Services.AddTransient<IToDoItemRepository, ToDoItemAzureBlobRepository>();
builder.Services.AddTransient<ICloudStorageRepository, AzureStorageRepository>();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["AzuriteConnectionString:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzuriteConnectionString:queue"]!, preferMsi: true);
});
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