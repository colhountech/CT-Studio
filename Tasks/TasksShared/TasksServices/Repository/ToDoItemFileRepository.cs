using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CloudStorage;
using Microsoft.Extensions.Logging;
using TasksAppData;

namespace TasksServices.Repository
{
	public class TodoItemFileRepository : ITodoItemRepository
	{
        // on mac os x, Alt-Enter is Quick Fix
        //
        private readonly ILogger<TodoItemFileRepository> _logger;

        private static readonly string _path = "Database.json";
        private static bool validated = false;



        public TodoItemFileRepository(
            ILogger<TodoItemFileRepository> logger
            )
        {
            _logger = logger;
        }


        public async Task ValidateSourceAsync()
        {
            if (!validated)
            {
                var exists = File.Exists(_path);

                if (!exists)
                {
                    throw new ApplicationException("Can't find Your Database. Have you changed something");
                }
                validated = true;
            }
            await Task.CompletedTask;
        }

        public async Task<List<TodoItemData>?> RestoreDataAsync()
        {
            using (var fs = File.OpenRead(_path))
            {
                var options = new JsonSerializerOptions();
                return await JsonSerializer.DeserializeAsync<List<TodoItemData>>(fs, options);
            }
            
        }

        public async Task StoreDataAsync(List<TodoItemData>? db)
        {
            try
            {
                var backup = $"{_path}~";
                File.Move(_path, backup, true);
            }
            catch (Exception ex)
            {
                   _logger.LogError($"StoreData Backup FAILED: {ex.Message}");
            }

            using (var fs = File.Open(_path, FileMode.Create))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                };
                await JsonSerializer.SerializeAsync(fs, db, options);
            }
        }
    }
}

