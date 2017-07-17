using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Reflection;

namespace ToDoFunctions
{
    public static class ReturnIndexPage
    {
        [FunctionName("HomePage")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")]HttpRequestMessage req, TraceWriter log, ExecutionContext context)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var path = Path.GetFullPath(Path.Combine(context.FunctionDirectory, @"..\"));
            var stream = new FileStream(path + "\\Index.html", FileMode.Open);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}