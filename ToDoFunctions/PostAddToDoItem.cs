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
    public static class PostAddToDoItem
    {
        [FunctionName("PostAddToDoItem")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route ="Api/ToDoItem")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, TraceWriter log, ExecutionContext context)
        {
            try
            {
                var json = await req.Content.ReadAsStringAsync();
                var toDoItem = JsonConvert.DeserializeObject<ToDoItem>(json);
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