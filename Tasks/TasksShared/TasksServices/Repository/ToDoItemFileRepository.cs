using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CloudStorage;
using Microsoft.Extensions.Logging;
using TasksAppData;

namespace TasksServices.Repository
{
	public class ToDoItemFileRepository : IToDoItemRepository
	{
        // on mac os x, Alt-Enter is Quick Fix
        //
        private readonly ILogger<ToDoItemFileRepository> _logger;

        private static readonly string _path = "Database.json";


        public ToDoItemFileRepository(
            ILogger<ToDoItemFileRepository> logger
            )
        {
            _logger = logger;
        }

        public async Task<List<TodoItemData>> RestoreData()
        {
            using (var fs = File.OpenRead(_path))
            {
                var options = new JsonSerializerOptions();
                var db = await JsonSerializer.DeserializeAsync<List<TodoItemData>>(fs, options);
                if (db is null) throw (new ApplicationException("No Database: Check Database.json exists"));
                return db;
            }
            
        }

        public async Task StoreData(List<TodoItemData>? db)
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

