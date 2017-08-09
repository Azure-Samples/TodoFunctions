using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ToDoFunctions
{
    public static class Utility
    {
        public static void AddOrUpdateToDoToTable(CloudTable table, ToDo todo)
        {
            if (string.IsNullOrEmpty(todo.id))
            {
                todo.id = Guid.NewGuid().ToString();
            }

            var saveOperation = TableOperation.InsertOrReplace(todo.MapToTableEntity());
            table.Execute(saveOperation);
        }

        public static ToDo GetToDoFromTable(CloudTable table, string id)
        {
            var retrieveOperation = TableOperation.Retrieve<ToDoItem>("ToDoItem", id);
            var item = (ToDoItem)table.Execute(retrieveOperation).Result;
            return item.MapFromTableEntity();
        }

        public static void DeleteToDoFromTable(CloudTable table, string id)
        {
            var item = new ToDoItem { RowKey = id, ETag = "*" };
            var deleteOperation = TableOperation.Delete(item);
            table.Execute(deleteOperation);
        }
    }
}
