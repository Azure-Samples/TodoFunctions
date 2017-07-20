using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Net.Http.Headers;

namespace ToDoFunctions
{
    public static class ReturnAddToDoPage
    {
        [FunctionName("CreateToDo")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req, ExecutionContext context, TraceWriter log)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var path = Path.GetFullPath(Path.Combine(context.FunctionDirectory, @"..\"));
            var stream = new FileStream(path + "\\AddToDo.html", FileMode.Open);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}