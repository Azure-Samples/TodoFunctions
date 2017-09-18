using Microsoft.ApplicationInsights.Channel;
using System;

namespace ToDoFunctions.UnitTests.Stubs
{
    public sealed class StubTelemetryChannel : ITelemetryChannel
    {
        public StubTelemetryChannel()
        {
            this.OnSend = telemetry => { };
        }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress { get; set; }

        public bool ThrowError { get; set; }

        public TelemetryAction OnSend { get; set; }

        public void Send(ITelemetry item)
        {
            if (this.ThrowError)
            {
                throw new Exception("test error");
            }

            this.OnSend(item);
        }

        public void Dispose()
        {
        }
        public void Flush()
        {
        }
    }

    public delegate void TelemetryAction(ITelemetry telemetry);
}
