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
        public static void AddOrUpdateToDoItemToTable(CloudTable table, ToDoItem item)
        {
            if (item.RowKey == null || item.RowKey == "")
            {
                item.RowKey = Guid.NewGuid().ToString();
                item.PartitionKey = "ToDoItem";
            }
                

            var saveOperation = TableOperation.InsertOrReplace(item);
            table.Execute(saveOperation);
        }

        // TODO: 
        public static void AddOrUpdateToDoItemToTable(CloudTable table, ToDo todo)
        {
            if (string.IsNullOrEmpty(todo.id))
            {
                todo.id = Guid.NewGuid().ToString();
            }

            var saveOperation = TableOperation.InsertOrReplace(todo.MapToTableEntity());
            table.Execute(saveOperation);
        }

        public static ToDoItem GetToDoItemFromTable(CloudTable table, string id)
        {
            var retrieveOperation = TableOperation.Retrieve<ToDoItem>("ToDoItem", id);
            var item = (ToDoItem)table.Execute(retrieveOperation).Result;
            return item;
        }

        public static ToDo GetToDoFromTable(CloudTable table, string id)
        {
            var retrieveOperation = TableOperation.Retrieve<ToDoItem>("ToDoItem", id);
            var item = (ToDoItem)table.Execute(retrieveOperation).Result;
            return item.MapFromTableEntity();
        }

        public static void DeleteToDoItemFromTable(CloudTable table, ToDoItem item)
        {
            var deleteOperation = TableOperation.Delete(item);
            table.Execute(deleteOperation);

        }

        public static void DeleteToDoFromTable(CloudTable table, string id)
        {
            var item = new ToDoItem { RowKey = id, ETag = "*" };
            var deleteOperation = TableOperation.Delete(item);
            table.Execute(deleteOperation);
        }

        public static HttpResponseMessage ReturnRequestedHttpResponsePage(string path)
        {
            
            var stream = new FileStream(path, FileMode.Open);
            var response = new HttpResponseMessage()
            {
                Content = new StreamContent(stream),
                StatusCode = HttpStatusCode.OK
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return response;
        }

        public static HttpResponseMessage Return404HttpResponsePage(string path)
        {

            var stream = new FileStream(path, FileMode.Open);
            var response = new HttpResponseMessage
            {
                Content = new StreamContent(stream),
                StatusCode = HttpStatusCode.NotFound
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return response;
        }
    }
}
