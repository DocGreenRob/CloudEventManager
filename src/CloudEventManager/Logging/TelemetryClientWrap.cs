using CloudEventManager.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CloudEventManager.Manager.Logging
{
	public class TelemetryClientWrap : ITelemetryClient, IDisposable
	{
		internal const string InstrumentationKeyConfigurationName = "APPINSIGHTS_INSTRUMENTATIONKEY";
		internal readonly TelemetryClient Client;

		public TelemetryClientWrap(IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}
			var instrumentationKey = configuration[InstrumentationKeyConfigurationName];

			if (string.IsNullOrWhiteSpace(instrumentationKey))
			{
				throw new InvalidOperationException($"The configuration '{InstrumentationKeyConfigurationName}' was not found and it is required to log events in Application Insights.");
			}
			Client = new TelemetryClient() { InstrumentationKey = instrumentationKey };

			//Needed for the telemetry
			//Client.Context.Operation.Id = Client.Context.Operation.Id ?? Guid.NewGuid().ToString();
		}

		public string ContextOperationId { get { return Client.Context.Operation.Id; } set => throw new NotImplementedException(); }

		public void Dispose()
		{
			Client.Flush();
		}

		public void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
		{
			Client.TrackDependency(dependencyTypeName, dependencyName, data, startTime, duration, success);
		}

		public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
		{
			Client.TrackEvent(eventName, properties, metrics);
		}

		public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
		{
			Client.TrackException(exception, properties, metrics);
		}
	}
}
