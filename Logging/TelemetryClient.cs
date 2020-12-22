using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Logging
{
	public class TelemetryClient
	{
		public string InstrumentationKey { get; set; }
		public Context Context { get; set; }

		public void Flush() { }
		public void TrackDependency(string dependencyTypeName,
			string dependencyName,
			string data,
			DateTimeOffset startTime,
			TimeSpan duration,
			bool success)
		{ }

		public void TrackEvent(string eventName, 
			IDictionary<string, string> properties = null, 
			IDictionary<string, double> metrics = null)
		{ }

		public void TrackException(Exception exception, 
			IDictionary<string, string> properties = null, 
			IDictionary<string, double> metrics = null)
		{ }
	}

	public class Context
	{
		public Operation Operation { get; set; }
	}

	public class Operation
	{
		public string Id { get; set; }
	}
}
