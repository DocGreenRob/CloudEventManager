using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Manager.Logging
{
	public interface ITelemetryClient
	{
		string ContextOperationId { get; set; }
		void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success);
		void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
		void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
	}
}
