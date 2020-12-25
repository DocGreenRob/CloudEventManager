using CloudEventManager.Extensions;
using CloudEventManager.Manager.Interface.Messaging;
using System;
using System.Linq;
using System.Text;

namespace CloudEventManager.Manager.Implementation.Messaging
{
	public class TraceParent : ITraceParent
	{
		public static Random rnd = new Random();
		public byte TraceFlags = 0x00;
		public byte Version = 0x00;
		public bool IsSpanIdValid
		{
			get
			{
				return (!SpanId.Any(b => b == 0));
			}
		}

		public bool IsTraceIdValid
		{
			get
			{
				return (!TraceId.Any(b => b == 0));
			}
		}

		public byte[] SpanId { get; set; } = Enumerable.Repeat((byte)0x00, 16).ToArray();
		public byte[] TraceId { get; set; } = Enumerable.Repeat((byte)0x00, 32).ToArray();
		public void InitializeParent(string traceParent)
		{
			if (string.IsNullOrWhiteSpace(traceParent))
			{
				VerifyValidTrace(); // this will initialize
				return;
			}
			else
			{
				string[] parts = traceParent.Split('-');
				if (parts.Count() == 4)
				{
					bool VersionLengthCheck = parts[0].Length > 2;
					bool TraceIdLengthCheck = parts[1].Length > 32;
					bool SpanIdLengthCheck = parts[2].Length > 16;
					bool TraceFlagsCheck = parts[3].Length > 2;
					if (VersionLengthCheck || TraceIdLengthCheck || SpanIdLengthCheck || TraceFlagsCheck)
					{
						throw new ArgumentException($"Length check too long Version-{VersionLengthCheck} TraceId-{TraceIdLengthCheck} SpanId-{SpanIdLengthCheck} TraceFlags-{TraceIdLengthCheck}");
					}
					else
					{
						Version = Convert.ToByte(parts[0]);
						TraceId.SetBytesFromValue(parts[1]);
						SpanId.SetBytesFromValue(parts[2]);
						TraceFlags = Convert.ToByte(parts[3]);
					}
				}
				else if (parts.Count() == 1)
				{
					TraceId.SetBytesFromValue(parts[0]);
					SetNewSpan();
				}
				else
				{
					throw new ArgumentException("Invalid Token Count");
				}
			}
			VerifyValidTrace();
		}

		public void SetNewSpan()
		{
			SpanId.GetRandomBytes();
		}

		public void SetTraceFlagRecorded()
		{
			TraceFlags = 0x01;
		}

		public string ToTraceString()
		{
			var separator = "-";
			if (!TraceId.Any(b => b == 0))
			{
				StringBuilder s = new StringBuilder();
				s.Append(Version.ToString("X2"));
				s.Append(separator);
				s.Append(Encoding.ASCII.GetString(TraceId));
				s.Append(separator);
				s.Append(Encoding.ASCII.GetString(SpanId));
				s.Append(separator);
				s.Append(TraceFlags.ToString("X2"));
				return s.ToString();
			}
			else
			{
				return null;
			}
		}
		public void VerifyValidTrace()
		{
			if (!IsTraceIdValid)
			{
				TraceId.GetRandomBytes();
			}
			if (!IsSpanIdValid)
			{
				SetNewSpan();
			}
		}

	}
}
