using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoFunctions
{
    public static class Utility
    {
        public static ToDoItem DeserializeJson(string json)
        {
            var toDoItem = JsonConvert.DeserializeObject<ToDoItem>(json);

            return toDoItem;
        }

        public static string SerializeToDoItemToJson(ToDoItem toDoItem)
        {
            var json = JsonConvert.SerializeObject(toDoItem);

            return json;
        }

        public static string SerializeToDoItemToJson(List<ToDoItem> toDoItems)
        {
            var json = JsonConvert.SerializeObject(toDoItems);

            return json;
        }

        public static void AddOrUpdateToDoItemToTable(CloudTable table, ToDoItem item)
        {
            var saveOperation = TableOperation.InsertOrReplace(item);
            table.Execute(saveOperation);
        }

        public static ToDoItem GetToDoItemFromTable(CloudTable table, string id)
        {
            var retrieveOperation = TableOperation.Retrieve("ToDoItem", id);
            var item = (ToDoItem)table.Execute(retrieveOperation).Result;
            return item;
        }

        public static void DeleteToDoItemFromTable(CloudTable table, ToDoItem item)
        {
            var deleteOperation = TableOperation.Delete(item);
            table.Execute(deleteOperation);

        }
    }
}
