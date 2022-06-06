using CloudStorage;
using TasksServices.Services;

namespace TasksWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSingleton<IToDoItemService, ToDoItemAzureBlobService>();
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
            app.MapControllers();
            app.Run();
        }
    }
}