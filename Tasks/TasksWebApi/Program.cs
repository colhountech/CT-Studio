using CloudStorage;
using TasksServices.Services;

namespace TasksWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var corsPolicy = "_";
            var origins = new string[] { "https://localhost:61549", "http://localhost:61550" };


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: corsPolicy, policy =>{ policy
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    ; });
            });
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
            app.UseCors(corsPolicy);
            app.MapControllers();
            app.Run();
        }
    }
}