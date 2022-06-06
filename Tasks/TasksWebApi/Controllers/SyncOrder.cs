using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TasksServices.Services;
using TasksAppData;
using TasksWebApi.ViewModels;

namespace TasksWebApp
{
    [Route("[controller]")]
    [ApiController]
    public class SyncOrder : ControllerBase
    {
        private readonly ILogger<SyncOrder> _logger;
        private readonly IToDoItemService _service;
        private readonly IMapper _mapper;

        public SyncOrder(
            ILogger<SyncOrder> logger,
            IToDoItemService service,
            IMapper mapper
            )
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;

        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutTodoItem(IList<SyncOrderViewModel> items)
        {

            if (items == null) return BadRequest("list is empty");

            try
            {
                // do something
            }
            catch (Exception ex)
            {
                //if (!TodoItemExists(id))
                //{
                //    return NotFound();
                //}
                //else
                {
                    throw;
                }
            }

            return NoContent();



        }
    }
}
