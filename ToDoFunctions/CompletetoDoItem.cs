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
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route ="Api/CompleteToDoItem")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, TraceWriter log, ExecutionContext context)
        {
            var val = req.Content;
            var id = val.ReadAsStringAsync().Result;
            id = id.Replace("\"", "");


            var item = Utility.GetToDoItemFromTable(table, id);
            item.IsComplete = true;

            Utility.AddOrUpdateToDoItemToTable(table, item);

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            return response;
        }
    }
}