using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _dbContext;

        public TodoController(TodoContext dbContext)
        {
            _dbContext = dbContext;

        }


        [HttpGet("/getTodos")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            if (_dbContext.Todos == null)
            {
                return NotFound();
            }
            return await _dbContext.Todos.ToListAsync();
        }

        [HttpGet("/getTodo/{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            if (_dbContext.Todos == null)
            {
                return NotFound();
            }
            var todo = await _dbContext.Todos.FindAsync(id);

            if (todo == null)
            { return NotFound(); }

            return todo;
        }

        [HttpPost("/createTodo")]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            return Ok(todo);

        }

        [HttpPut("/editTodo/{id}")]
        public async Task<ActionResult> EditTodo(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(todo).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();

        }
        [HttpPut("/completeTodo/{id}")]

        public async Task<ActionResult> CompleteTodo(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }
            var todo = await _dbContext.Todos.FindAsync(id);
            if (todo == null) { return NotFound(); }

            todo.IsCompleted = !todo.IsCompleted;
            _dbContext.Entry(todo).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();


            return NoContent();

        }

        [HttpDelete("/deleteTodo/{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var todo = await _dbContext.Todos.FindAsync(id);
            if (todo == null) { return NotFound(); }



            _dbContext.Todos.Remove(todo);
            await _dbContext.SaveChangesAsync();
            return Ok();

        }
    }
}
