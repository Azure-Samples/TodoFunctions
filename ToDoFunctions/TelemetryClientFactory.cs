using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace ToDoFunctions
{
    public class TelemetryClientFactory : ITelemetryClientFactory
    {
        public virtual TelemetryClient GetClient()
        {
            string key = TelemetryConfiguration.Active.InstrumentationKey = System.Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
            TelemetryClient client = new TelemetryClient()
            {
                InstrumentationKey = key
            };

            return client;
        }
    }

    public interface ITelemetryClientFactory
    {
        TelemetryClient GetClient();
    }
}
