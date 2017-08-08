using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Text;

namespace ToDoFunctions
{
    public static class GetToDoItems
    {
        [FunctionName("GetToDoItems")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Api/ToDoItem/Current")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]IQueryable<ToDoItem> inTable, TraceWriter log)
        {
            var items = inTable.Where(p => p.IsComplete == false).ToList();

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json")
            };

            return response;
        }
    }
}