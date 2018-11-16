using JISTesting.Core.Interfaces;
using JISTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JISTesting.Infrastructure
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public void Add(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();
        }

        public List<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        public TodoItem GetById(long id)
        {
            return _context.TodoItems.Find(id);
        }

        public void Remove(TodoItem item)
        {
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
        }

        public void Update(TodoItem item)
        {
            _context.TodoItems.Update(item);
        }
    }
}
