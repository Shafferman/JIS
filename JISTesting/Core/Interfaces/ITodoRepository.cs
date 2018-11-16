using JISTesting.Models;
using System.Collections.Generic;

namespace JISTesting.Core.Interfaces
{
    public interface ITodoRepository
    {
        void Add(TodoItem item);
        List<TodoItem> GetAll();
        TodoItem GetById(long id);
        void Update(TodoItem item);
        void Remove(TodoItem item);
    }
}
