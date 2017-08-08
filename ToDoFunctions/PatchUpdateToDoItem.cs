using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace ToDoFunctions
{
    public static class PatchUpdateToDoItem
    {
        [FunctionName("PatchUpdateToDoItem")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "Api/ToDoItem")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, TraceWriter log)
        {
            
                var json = await req.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<ToDoItem>(json);

                var oldItem = Utility.GetToDoItemFromTable(table, item.RowKey);

                oldItem.Title = item.Title;
                oldItem.Description = item.Description;
                oldItem.Due = item.Due;

                Utility.AddOrUpdateToDoItemToTable(table, oldItem);

                return req.CreateResponse(HttpStatusCode.Created);
            
        }
    }
}