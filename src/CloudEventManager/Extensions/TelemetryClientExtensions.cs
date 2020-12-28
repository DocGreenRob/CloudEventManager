using CloudEventManager.Manager.Logging;
using CloudEventManager.Models;
using System;
using System.Collections.Generic;
using static CloudEventManager.ServiceGlobals.Logging;

namespace CloudEventManager.Extensions
{
	public static class TelemetryClientExtensions
	{
		public static void TrackIncomingMessage<TMessage>(this ITelemetryClient instance, Message message)
			where TMessage : class
		{
			ValidateArgs(instance, message);

			instance.TrackEvent($"{Events.IncomingMessage}-{typeof(TMessage).Name}", message.GetEventProperties());
		}
		public static void TrackMessageException<TMessage>(this ITelemetryClient instance,
			Message message,
			string eventName,
			Exception exception)
			where TMessage : class
		{
			ValidateArgs(instance, message);
			eventName.ValidateArgNotNull(nameof(eventName));
			exception.ValidateArgNotNull(nameof(exception));

			instance.TrackException(exception);
			instance.TrackEvent($"{eventName}-{typeof(TMessage).Name}", message.GetEventProperties());
		}

		private static Dictionary<string, string> GetEventProperties(this Message instance)
		{
			return new Dictionary<string, string>
			{
				[Keys.QueueMessage] = instance.GetBody(),
				[ServiceGlobals.RetryValues.RetryCounterKeyName] = instance.GetCurrentRetryCounter().ToString()
			};
		}

		private static void ValidateArgs(ITelemetryClient instance, Message message)
		{
			instance.ValidateArgNotNull(nameof(instance));
			instance.ValidateArgNotNull(nameof(message));
		}
	}
}
