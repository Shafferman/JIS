using JISTesting.Core.Interfaces;
using JISTesting.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JISTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repo;

        public TodoController(ITodoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}", Name ="GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _repo.GetById(id);

            if(item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _repo.Add(item);

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, TodoItem item)
        {
            var todo = _repo.GetById(id);

            if(todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _repo.Update(todo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _repo.GetById(id);

            if(todo == null)
            {
                return NotFound();
            }

            _repo.Remove(todo);

            return NoContent();
        }
    }
}
