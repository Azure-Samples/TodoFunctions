using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;

namespace ToDoFunctions
{
    public static class CompleteToDoItem
    {
        [FunctionName("CompleteToDoItem")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]IQueryable<ToDoItem> inTable, [Table("todotable", Connection = "MyTable")]CloudTable outTable, TraceWriter log, ExecutionContext context)
        {
            var val = req.Content;
            var id = val.ReadAsStringAsync().Result;
            id = id.Replace("\"", "");

            var retrieveOperation = TableOperation.Retrieve<ToDoItem>("ToDoItem", id);

            var item = (ToDoItem)outTable.Execute(retrieveOperation).Result;
            item.IsComplete = true;

            var updateOperation = TableOperation.Replace(item);
            outTable.ExecuteAsync(updateOperation);

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            return response;
        }
    }
}