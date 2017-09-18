using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace ToDoFunctions.UnitTests.Stubs
{
    public class TestTelemetryClientFactory : ITelemetryClientFactory
    {
        public TelemetryClient GetClient()
        {
            var configuration = new TelemetryConfiguration();

            configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => { } };
            configuration.InstrumentationKey = Guid.NewGuid().ToString();
            configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            var telemetryClient = new TelemetryClient(configuration);

            return telemetryClient;
        }
    }
}
