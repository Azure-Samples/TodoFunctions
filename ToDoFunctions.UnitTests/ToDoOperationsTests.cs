using Microsoft.Azure.WebJobs.Host;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using ToDoFunctions.Entities;
using ToDoFunctions.UnitTests.Stubs;
using Xunit;

namespace ToDoFunctions.UnitTests
{
    public class ToDoOperationsTests : IClassFixture<TestTelemetryClientFactory>
    {
      

        public ToDoOperationsTests(TestTelemetryClientFactory telemetryClientFactory)
        {

            ToDoOperations.telemetryFactory = telemetryClientFactory;
        }

        [Fact]
        public async Task GetAllToDos_ShouldReturnAllToDos()
        {
            var tw = new Mock<TraceWriter>(MockBehavior.Strict, new object[] { TraceLevel.Info });
            var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/todos");
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var todos = GetToDos();

            var resp = ToDoOperations.GetAllToDos(req, todos, tw.Object);
            var results = await resp.Content.ReadAsAsync<IEnumerable<ToDoItem>>();

            Assert.Equal(todos.Count(), results.Count());
        }

        [Fact]
        public async Task GetAllToDos_IncludeAll_ShouldReturnAllToDos()
        {
            var tw = new Mock<TraceWriter>(MockBehavior.Strict, new object[] { TraceLevel.Info });
            var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/todos?includeactive=true&includecompleted=true");
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var todos = GetToDos();

            var resp = ToDoOperations.GetAllToDos(req, todos, tw.Object);
            var results = await resp.Content.ReadAsAsync<IEnumerable<ToDoItem>>();

            Assert.Equal(todos.Count(), results.Count());
        }

        [Fact]
        public async Task GetAllToDos_IncludeActiveFalse_ShouldReturnOnlyCompleted()
        {
            var tw = new Mock<TraceWriter>(MockBehavior.Strict, new object[] { TraceLevel.Info });
            var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/todos?includeactive=false");
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var todos = GetToDos();

            var resp = ToDoOperations.GetAllToDos(req, todos, tw.Object);
            var results = await resp.Content.ReadAsAsync<IEnumerable<ToDoItem>>();

            Assert.Equal(1, results.Count());
            Assert.Equal(0, results.Count(r => !r.IsComplete));
        }

        [Fact]
        public async Task GetAllToDos_IncludeCompletedFalse_ShouldReturnOnlyActive()
        {
            var tw = new Mock<TraceWriter>(MockBehavior.Strict, new object[] { TraceLevel.Info });
            var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/todos?includecompleted=false");
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var todos = GetToDos();

            var resp = ToDoOperations.GetAllToDos(req, todos, tw.Object);
            var results = await resp.Content.ReadAsAsync<IEnumerable<ToDoItem>>();

            Assert.Equal(1, results.Count());
            Assert.Equal(0, results.Count(r => r.IsComplete));
        }

        private static IQueryable<ToDoItem> GetToDos()
        {
            return new List<ToDoItem>
            {
                new ToDoItem
                {
                    Title = "Foo",
                    IsComplete = false
                },
                new ToDoItem
                {
                    Title = "Bar",
                    IsComplete = true
                }
            }.AsQueryable();
        }

    }
}
