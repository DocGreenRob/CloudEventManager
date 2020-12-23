using CloudEventManager.Extensions;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Manager.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using System.Net.Http.Headers;

namespace CloudEventManager.Manager.Implementation.Messaging
{
	public class TraceHeader : ITraceHeader
	{
		public TraceHeader()
		{
			TraceState = new OrderedDictionary();
		}

		public ITraceParent TraceParent { get; } = new TraceParent();
		public OrderedDictionary TraceState { get; set; }

		public void AddNewTraceState(string newTraceStateName, string traceValue)
		{
			if (TraceState.Contains(newTraceStateName))
			{
				TraceState.Remove(newTraceStateName);
			}
			TraceState.Insert(0, newTraceStateName, traceValue);
		}

		public void ContinueTrace(string traceStateName, string traceValue = null, ITelemetryClient telemetry = null)
		{
			TraceParent.SetNewSpan();
			traceValue = traceValue ?? TraceParent.SpanId.BytesToAsciiString();
			AddNewTraceState(traceStateName, traceValue);

			if (telemetry != null)
			{
				telemetry.ContextOperationId = GetTraceId();
			}
		}

		public string GetTraceId()
		{
			return TraceParent.TraceId.BytesToAsciiString();
		}

		public string GetTraceState()
		{
			TraceParent.VerifyValidTrace();
			return TraceState.AsHeaderString();
		}

		public ITraceHeader InitializeTraceParent(string traceParent)
		{
			TraceParent.InitializeParent(traceParent);
			return this;
		}

		public ITraceHeader InitializeTraceState(string currentTraceState, string newTraceStateName)
		{
			if (currentTraceState != null)
			{
				TraceState = new OrderedDictionary();
				TraceState.InitializeFromString(currentTraceState);
			}
			if (newTraceStateName != null)
			{
				AddNewTraceState(newTraceStateName, TraceParent.SpanId.BytesToAsciiString());
			}
			return this;
		}
		public void SetTraceHeaders(IHeaderDictionary headers)
		{
			if (headers.ContainsKey(ServiceGlobals.Logging.TraceConstants.TraceParent))
			{
				headers[ServiceGlobals.Logging.TraceConstants.TraceParent] = TraceParent.ToTraceString();
			}
			else
			{
				headers.Add(ServiceGlobals.Logging.TraceConstants.TraceParent, TraceParent.ToTraceString());
			}

			if (headers.ContainsKey(ServiceGlobals.Logging.TraceConstants.TraceState))
			{
				headers[ServiceGlobals.Logging.TraceConstants.TraceState] = TraceState.AsHeaderString();
			}
			else
			{
				headers.Add(ServiceGlobals.Logging.TraceConstants.TraceState, TraceState.AsHeaderString());
			}
		}

		public void SetTraceHeaders(HttpHeaders headers)
		{
			if (headers.Contains(ServiceGlobals.Logging.TraceConstants.TraceParent))
			{
				headers.Remove(ServiceGlobals.Logging.TraceConstants.TraceParent);
			}
			headers.Add(ServiceGlobals.Logging.TraceConstants.TraceParent, TraceParent.ToString());
			if (headers.Contains(ServiceGlobals.Logging.TraceConstants.TraceState))
			{
				headers.Remove(ServiceGlobals.Logging.TraceConstants.TraceState);
			}
			headers.Add(ServiceGlobals.Logging.TraceConstants.TraceState, TraceState.AsHeaderString());
		}

	}
}
