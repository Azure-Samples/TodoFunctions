using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Net.Http.Headers;

namespace ToDoFunctions
{
    public static class ReturnEditToDoPage
    {
        [FunctionName("EditToDo")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "EditToDo/{id}")]HttpRequestMessage req, string id, ExecutionContext context, TraceWriter log)
        {
            // TODO: add logic to check if item is in Table


            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var path = Path.GetFullPath(Path.Combine(context.FunctionDirectory, @"..\"));
            var stream = new FileStream(path + "\\UpdateToDo.html", FileMode.Open);
            response.Content = new StreamContent(stream);             
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            if (id == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name in the request body");
            }

            return response;
        }
    }
}