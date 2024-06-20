using Microsoft.AspNetCore.Mvc;
using Tasks.AppData;
using Tasks.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp
{
    [Route("[controller]")]
    [ApiController]
    public class SyncOrder : ControllerBase
    {
        private readonly ILogger<SyncOrder> _logger;
        private readonly ITodoItemService _service;

        public SyncOrder(
            ILogger<SyncOrder> logger,
            ITodoItemService service
            )
        {
            _logger = logger;
            _service = service;

        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]   // happy path
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTodoItems(IList<SyncOrderViewModel> items)
        {

            if (items == null) return BadRequest("list is empty");

            try
            {
                await _service.LoadAsync();
                var storedItems = _service.GetItems(archived: false);
                var updateItems = new List<TodoItemData>();

                foreach ( var updateItem in items)
                {
                    // find match in db
                    var match = storedItems.Where(x => x.ID == updateItem.ID).FirstOrDefault();

                    // update Order to new item.Order
                    if (match is not null && match.Order != updateItem.Order)
                    {                        
                        match.Order = updateItem.Order;
                        updateItems.Add(match);
                    }
                }
                _logger.LogTrace($"matched {updateItems.Count}");

                if (updateItems.Count == 0)
                {
                    // nothing to update;
                    return NotFound();
                }

                // update db
                _service.UpdateItems(updateItems);
                await _service.SaveAsync();
                // this may throw an OptimisticConcurrencyControl exception
                // if so, just reload page


            }
            catch (Exception ex)
            {                
                {
                    _logger.LogWarning("Couldn't update Order:" + ex.Message);
                    throw;
                }
            }

            return NoContent();



        }


    }
}
