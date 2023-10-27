using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSeparation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> List()
        {
            return await _context.Todos.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Create(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }

        [HttpGet]
        public async Task<ActionResult<Todo>> Get([FromQuery] int id)
        {
            return await _context.Todos.FirstAsync(x => x.Id == id);
        }
    }
}
