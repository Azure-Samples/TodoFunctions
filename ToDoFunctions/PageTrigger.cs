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
    public static class Page
    {
        [FunctionName("Page")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Page/{pageName}/{id?}")]HttpRequestMessage req, string pageName, TraceWriter log, ExecutionContext context)
        {
            if (pageName.Contains(".html"))
            {
                var path = Path.GetFullPath(Path.Combine(context.FunctionDirectory, @"..\")) + $"Pages\\{pageName}";
                if (File.Exists(path))
                {
                    return Utility.ReturnRequestedHttpResponsePage(path);
                }
                else
                {
                    path = Path.GetFullPath(Path.Combine(context.FunctionDirectory, @"..\")) + $"Pages\\404.html";
                    return Utility.Return404HttpResponsePage(path);
                }
                
            }
            else if (pageName.Contains("favicon.ico"))
            {
                log.Warning($"Request: {pageName}");
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
            }
            else
            {
                var path = Path.GetFullPath(Path.Combine(context.FunctionDirectory, @"..\")) + $"Pages\\{pageName}.html";
                if (File.Exists(path))
                {                
                    return Utility.ReturnRequestedHttpResponsePage(path);
                }
                else
                {
                    path = Path.GetFullPath(Path.Combine(context.FunctionDirectory, @"..\")) + $"Pages\\404.html";
                    return Utility.Return404HttpResponsePage(path);
                }
            }
        }
    }
}