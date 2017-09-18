using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoFunctions.Entities
{
    public static class ToDoExtensions
    {
        public static ToDoItem MapToTableEntity(this ToDo todo)
        {
            return new ToDoItem
            {
                RowKey = todo.id,
                Title = todo.title,
                Description = todo.description,
                Due = todo.due,
                IsComplete = todo.isComplete
            };
        }

        public static ToDo MapFromTableEntity(this ToDoItem item)
        {
            return new ToDo
            {
                id = item.RowKey,
                title = item.Title,
                description = item.Description,
                due = item.Due,
                isComplete = item.IsComplete
            };
        }
    }
}
