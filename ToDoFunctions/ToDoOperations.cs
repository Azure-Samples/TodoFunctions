using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using ToDoFunctions.Entities;

namespace ToDoFunctions
{
    public static class ToDoOperations
    {
        public static ITelemetryClientFactory telemetryFactory = new TelemetryClientFactory();

        [FunctionName("GetAllToDos")]
        public static HttpResponseMessage GetAllToDos([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/todos")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]IQueryable<ToDoItem> inTable, TraceWriter log)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var includeCompleted = !req.GetQueryNameValuePairs().Any(i => i.Key == "includecompleted" && i.Value == "false");
            var includeActive = !req.GetQueryNameValuePairs().Any(i => i.Key == "includeactive" && i.Value == "false");

            var items = inTable
                .Where(p => (p.IsComplete == false || includeCompleted) && (p.IsComplete == true || includeActive)).ToList()
                .Select(i => i.MapFromTableEntity()).ToList();

            stopWatch.Stop();

            var metrics = new Dictionary<string, double> {
                { "processingTime", stopWatch.Elapsed.TotalMilliseconds}
            };

            TelemetryClient telemetryClient = telemetryFactory.GetClient();
            telemetryClient.TrackEvent("get-all-todos", metrics: metrics);

            return req.CreateResponse(HttpStatusCode.OK, items);
        }

        [FunctionName("GetToDo")]
        public static HttpResponseMessage GetToDo([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/todos/{id}")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, string id, TraceWriter log)
        {
            Stopwatch stopWatch = new Stopwatch();
            var item = table.GetToDoFromTable( id);

            stopWatch.Stop();

            var metrics = new Dictionary<string, double> {
                { "processingTime", stopWatch.Elapsed.TotalMilliseconds}
            };

            var props = new Dictionary<string, string> {
                { "todo-id", id}
            };

            TelemetryClient telemetryClient = telemetryFactory.GetClient();
            telemetryClient.TrackEvent("get-todo", metrics: metrics);

            return req.CreateResponse(HttpStatusCode.OK, item);
        }

        [FunctionName("CreateToDo")]
        public static async Task<HttpResponseMessage> CreateToDo([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/todos")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, TraceWriter log, ExecutionContext context)
        { TelemetryClient telemetryClient = telemetryFactory.GetClient();
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                var json = await req.Content.ReadAsStringAsync();
                var todo = JsonConvert.DeserializeObject<ToDo>(json);
                table.AddOrUpdateToDoToTable( todo);
                stopWatch.Stop();

                var metrics = new Dictionary<string, double> {
                    { "processingTime", stopWatch.Elapsed.TotalMilliseconds}
                };

                var props = new Dictionary<string, string> {
                     { "todo-id", todo.id}
                };

               
                telemetryClient.TrackEvent("create-todo", metrics: metrics);
                return req.CreateResponse(HttpStatusCode.Created, todo);
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
                return req.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [FunctionName("UpdateToDo")]
        public static async Task<HttpResponseMessage> UpdateToDo([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "api/todos/{id}")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, string id, TraceWriter log)
        {
            Stopwatch stopWatch = new Stopwatch();

            var json = await req.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<ToDo>(json);

            var oldItem = table.GetToDoFromTable( id);
            item.id = id; // ensure item id matches id passed in
            item.isComplete = oldItem.isComplete; // ensure we don't change isComplete

            table.AddOrUpdateToDoToTable( item);

            stopWatch.Stop();

            var metrics = new Dictionary<string, double> {
                { "processingTime", stopWatch.Elapsed.TotalMilliseconds}
            };

            var props = new Dictionary<string, string> {
                { "todo-id", id}
            };

            TelemetryClient telemetryClient = telemetryFactory.GetClient();
            telemetryClient.TrackEvent("update-todo", properties: props, metrics: metrics);
            return req.CreateResponse(HttpStatusCode.OK, item);
        }

        [FunctionName("SetCompleteToDo")]
        public static async Task<HttpResponseMessage> SetCompleteToDo([HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "api/todos/{id}")]HttpRequestMessage req, [Table("todotable", Connection = "MyTable")]CloudTable table, string id, TraceWriter log)
        {
            Stopwatch stopWatch = new Stopwatch();
            var json = await req.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<ToDo>(json);

            var oldItem = table.GetToDoFromTable( id);
            oldItem.isComplete = item.isComplete;

            table.AddOrUpdateToDoToTable( oldItem);
            stopWatch.Stop();

            var metrics = new Dictionary<string, double> {
                { "processingTime", stopWatch.Elapsed.TotalMilliseconds}
            };

            var props = new Dictionary<string, string> {
                { "todo-id", id}
            };

            TelemetryClient telemetryClient = telemetryFactory.GetClient();
            telemetryClient.TrackEvent("complete-todo", properties: props, metrics: metrics);

            return req.CreateResponse(HttpStatusCode.OK, oldItem);
        }

        [FunctionName("DeleteToDo")]
        public static HttpResponseMessage DeleteToDo([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "api/todos/{id}")]HttpRequestMessage req, string id, [Table("todotable", Connection = "MyTable")]CloudTable table, TraceWriter log)
        {
            Stopwatch stopWatch = new Stopwatch();
            table.DeleteToDoFromTable( id);

            stopWatch.Stop();

            var metrics = new Dictionary<string, double> {
                { "processingTime", stopWatch.Elapsed.TotalMilliseconds}
            };

            var props = new Dictionary<string, string> {
                { "todo-id", id}
            };

            TelemetryClient telemetryClient = telemetryFactory.GetClient();
            telemetryClient.TrackEvent("delete-todo", properties: props, metrics: metrics);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}