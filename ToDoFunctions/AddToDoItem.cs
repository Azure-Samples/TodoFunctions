using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http.Headers;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using System.Diagnostics;
using System.Reflection;

namespace ToDoFunctions
{
    public static class AddToDoItem
    {
        [FunctionName("AddToDoItem")]
        [StorageAccount("MyTable")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, TraceWriter log, ExecutionContext context)
        {
            try
            {
                var val = req.Content;
                var json = val.ReadAsStringAsync().Result;
                var toDoItem = Utility.DeserializeJson(json);
                Utility.AddOrUpdateToDoItemToTable(table, toDoItem);

                return req.CreateResponse(HttpStatusCode.Created);                                              
            }
            catch (Exception e)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }            
        }
    }
}