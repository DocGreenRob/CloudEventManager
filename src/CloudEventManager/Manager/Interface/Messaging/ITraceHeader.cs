using CloudEventManager.Manager.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using System.Net.Http.Headers;

namespace CloudEventManager.Manager.Interface.Messaging
{
	public interface ITraceHeader
	{
		ITraceParent TraceParent { get; }
		OrderedDictionary TraceState { get; set; }
		void ContinueTrace(string traceStateName, string optionalTraceValue = null, ITelemetryClient telemetry = null);
		ITraceHeader InitializeTraceParent(string traceParent);
		ITraceHeader InitializeTraceState(string currentTraceState, string newTraceStateName);
		void AddNewTraceState(string newTraceStateName, string traceValue);
		string GetTraceId();
		string GetTraceState();
		void SetTraceHeaders(HttpHeaders headers);
		void SetTraceHeaders(IHeaderDictionary headers);
	}
}
