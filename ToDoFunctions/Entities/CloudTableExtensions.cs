using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ToDoFunctions.Entities
{
    public static class CloudTableExtensions
    {
        public static void AddOrUpdateToDoToTable(this CloudTable table, ToDo todo)
        {
            if (string.IsNullOrEmpty(todo.id))
            {
                todo.id = Guid.NewGuid().ToString();
            }

            var saveOperation = TableOperation.InsertOrReplace(todo.MapToTableEntity());
            table.Execute(saveOperation);
        }

        public static ToDo GetToDoFromTable(this CloudTable table, string id)
        {
            var retrieveOperation = TableOperation.Retrieve<ToDoItem>("ToDoItem", id);
            var item = (ToDoItem)table.Execute(retrieveOperation).Result;
            return item.MapFromTableEntity();
        }

        public static void DeleteToDoFromTable(this CloudTable table, string id)
        {
            var item = new ToDoItem { RowKey = id, ETag = "*" };
            var deleteOperation = TableOperation.Delete(item);
            table.Execute(deleteOperation);
        }
    }
}
