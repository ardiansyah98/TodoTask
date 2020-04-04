using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoTask.Contants;
using TodoTask.Data;
using TodoTask.Models;

namespace TodoTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoTaskContext _context;

        public TodoController(TodoTaskContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        // Get all todo in db
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodo()
        {
            return await _context.Todo.ToListAsync();
        }

        // GET: api/Todo/{expiredAt}
        // Get todos with specific day (date)
        [HttpGet("{expiredAt:DateTime}")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodo(DateTime expiredAt)
        {
            // only get date, if request have time it will be ignore to db request 
            DateTime expiredFrom = expiredAt.Date; 

            // get date+1 to limit of our request range for today using '<' condition of expiredTo 
            DateTime expiredTo = expiredAt.Date.AddDays(1); 

            return await _context.Todo.Where(x => x.ExpiredAt >= expiredFrom && x.ExpiredAt < expiredTo).ToListAsync();
        }

        // GET: api/Todo/{expiredFrom}/{expiredTo}
        // Get todos with range datetime
        [HttpGet("{expiredFrom:DateTime}/{expiredTo:DateTime}")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodo(DateTime expiredFrom, DateTime expiredTo)
        {
            return await _context.Todo.Where(x => x.ExpiredAt >= expiredFrom && x.ExpiredAt <= expiredTo).ToListAsync();
        }

        // GET: api/Todo/{id}
        // Get todo with id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _context.Todo.FindAsync(id);

            if (todo == null)
            {
                return new Todo();
            }

            return todo;
        }


        // PUT: api/Todo/{id}
        // Update todo
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Todo>> PutTodo(int id, Todo todo)
        {
            if (!TodoExists(id))
                return NotFound();

            var old_todo = GetTodo(id).Result.Value;

            //mapping old todo to new todo
            todo.Id = todo.Id == 0 ? old_todo.Id : todo.Id;
            todo.Title = todo.Title == null ? old_todo.Title : todo.Title;
            todo.Description = todo.Description == null ? old_todo.Description : todo.Description;
            todo.CompletePercentage = todo.CompletePercentage == 0 ? old_todo.CompletePercentage : todo.CompletePercentage;
            todo.Status = todo.Status == null ? old_todo.Status : todo.Status.ToUpper();
            todo.ExpiredAt = todo.ExpiredAt == DateTime.MinValue ? old_todo.ExpiredAt : todo.ExpiredAt;
            todo.CreatedAt = old_todo.CreatedAt;
            todo.UpdatedAt = DateTime.Now;


            // set status and complete percentage on complete todo
            if(todo.CompletePercentage == 100)
            {
                todo.Status = TodoConstants.STATUS_COMPLETE;
            } 
            else if(todo.Status.ToUpper() == TodoConstants.STATUS_COMPLETE)
            {
                todo.CompletePercentage = 100;
            }
                       

            // remove and add method to update value
            // old todo already mapped to new todo
            _context.Todo.Remove(old_todo);
            _context.Todo.Add(todo);
            await _context.SaveChangesAsync();

            return todo;
        }


        // POST: api/Todo
        // Create Todo
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            DateTime now = DateTime.Now;

            // Set created and updated datetime for the first time
            todo.CreatedAt = now;
            todo.UpdatedAt = now;

            _context.Todo.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
        }

        // DELETE: api/Todo/{id}
        // Delete todo with id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Todo>> DeleteTodo(int id)
        {
            var todo = await _context.Todo.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }
            else
            {
                _context.Todo.Remove(todo);
                await _context.SaveChangesAsync();
                return todo;
            }
            
        }

        // Check todo exist in db
        private bool TodoExists(int id)
        {
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}
