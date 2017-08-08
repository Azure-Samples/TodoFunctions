using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Text;

namespace ToDoFunctions
{
    public static class GetUpdateToDoItem
    {
        [FunctionName("GetUpdateToDoItem")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="Api/ToDoItem/{id}")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, string id, TraceWriter log)
        {
            
            //var val = req.RequestUri.ToString();
            //string[] parts = val.Split(new char[] { '=' });
            //var idd = parts[1];
            //idd = idd.Replace("\"", "");

            var item = Utility.GetToDoItemFromTable(table, id);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
            };

            return response;


        }


    }
}