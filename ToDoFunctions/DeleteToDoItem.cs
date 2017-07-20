using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;

namespace ToDoFunctions
{
    public static class DeleteToDoItem
    {
        [FunctionName("DeleteToDoItem")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "DeleteToDoItem")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, TraceWriter log)
        {
            var val = req.Content;
            var id = val.ReadAsStringAsync().Result;
            id = id.Replace("\"", "");

            var item = Utility.GetToDoItemFromTable(table, id);

            Utility.DeleteToDoItemFromTable(table, item);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}