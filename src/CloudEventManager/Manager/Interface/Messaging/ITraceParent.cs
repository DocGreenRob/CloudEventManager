namespace CloudEventManager.Manager.Interface.Messaging
{
	public interface ITraceParent
	{
		bool IsSpanIdValid { get; }
		bool IsTraceIdValid { get; }
		void InitializeParent(string traceParent);
		void SetNewSpan();
		void SetTraceFlagRecorded();
		string ToTraceString();
		void VerifyValidTrace();
		byte[] TraceId { get; }
		byte[] SpanId { get; }
	}
}
