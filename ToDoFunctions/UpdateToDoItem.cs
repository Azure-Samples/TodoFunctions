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
    public static class UpdateToDoItem
    {
        [FunctionName("UpdateToDoItem")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", Route ="Api/UpdateToDoItem")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, TraceWriter log)
        {
            if (req.Method == HttpMethod.Get)
            {
                var val = req.RequestUri.ToString();
                string[] parts = val.Split(new char[] { '=' });
                var id = parts[1];
                id = id.Replace("\"", "");

                var item = Utility.GetToDoItemFromTable(table, id);

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
                };

                return response;
            }

            if (req.Method == HttpMethod.Post)
            {
                var json = req.Content.ReadAsStringAsync().Result;
                var item = JsonConvert.DeserializeObject<ToDoItem>(json);

                var oldItem = Utility.GetToDoItemFromTable(table, item.RowKey);

                oldItem.Title = item.Title;
                oldItem.Description = item.Description;
                oldItem.Due = item.Due;

                Utility.AddOrUpdateToDoItemToTable(table, oldItem);

                return req.CreateResponse(HttpStatusCode.Created);
            }

            else
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }


    }
}