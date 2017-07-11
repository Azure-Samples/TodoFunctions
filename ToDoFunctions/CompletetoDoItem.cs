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

namespace ToDoFunctions
{
    public static class CompleteToDoItem
    {
        [FunctionName("CompleteToDoItem")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CompleteToDoItem/{id}")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]IQueryable<ToDoItem> inTable, [Table("todotable", Connection = "MyTable")]CloudTable outTable, string id, TraceWriter log)
        {
            var item = inTable.Where(p => p.PartitionKey == id).FirstOrDefault();
            item.IsComplete = true;

            var operation = TableOperation.Replace(item);
            outTable.ExecuteAsync(operation);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(@"Index.html", FileMode.Open);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}