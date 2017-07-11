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


namespace ToDoFunctions
{
    public static class AddToDoItem
    {
        [FunctionName("AddToDoItem")]
        [StorageAccount("MyTable")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "get")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]ICollector<ToDoItem> outTable, TraceWriter log)
        {
            try
            {                
                if (req.Method == HttpMethod.Post)
                {
                    var val = req.Content;
                    var json = val.ReadAsStringAsync().Result;
                    var toDoItem = JsonConvert.DeserializeObject<ToDoItem>(json);
                    //outTable.Add(toDoItem);
                    AddEntityToTable(toDoItem, outTable);
                    return req.CreateResponse(HttpStatusCode.Created);
                }

                else if(req.Method == HttpMethod.Get)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(@"c:\users\streamer\Source\Repos\ToDoFunctions\ToDoFunctions\AddToDo.html", FileMode.Open);
                    response.Content = new StreamContent(stream);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                    return response;
                }
                else
                {
                    return req.CreateResponse(HttpStatusCode.InternalServerError);
                }                            
            }
            catch (Exception)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }            
        }

        private static void AddEntityToTable(ToDoItem item, ICollector<ToDoItem> outTable)
        {          
            item.PartitionKey = Guid.NewGuid().ToString();
            item.RowKey = item.Title;
            outTable.Add(item);
        }
    }
}