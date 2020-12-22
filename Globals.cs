using System;

namespace CloudEventManager
{
	public static class ServiceGlobals
	{
		public static class RetryValues
		{
			public static TimeSpan DefaultMaxElapsedRetryTimeSpan = new TimeSpan(4, 0, 0);
			public static TimeSpan DefaultMaxBackoffTimeSpan = new TimeSpan(0, 2, 0);
			public const double MaximumBackoffTimeInMilliseconds = 14400000;
			public const int MaximumRetryAmount = 10;
			public const string MaxRetryCount = "maxRetryCount";
			public const string MaxWaitTimeInMilliseconds = "maxWaitTimeInMilliseconds";
			public const string RetryCounterKeyName = "RetryCounter";
		}

		public static class Exception
		{
			public const string ERRInvalidMessageType = nameof(ERRInvalidMessageType);
			public const string ERRInvalidCallbackUri = nameof(ERRInvalidCallbackUri);
			public const string ERRInvalidStatusCodeFromClient = nameof(ERRInvalidStatusCodeFromClient);
		}

		public static class Logging
		{
			public static class Events
			{
				public const string ResendMessage = nameof(ResendMessage);
				public const string IncomingMessage = nameof(IncomingMessage);
				public const string ReQueueDLMessage = nameof(ReQueueDLMessage);
			}

			public static class Keys
			{
				public const string QueueMessage = "queueMessage";
			}
		}
	}
}
