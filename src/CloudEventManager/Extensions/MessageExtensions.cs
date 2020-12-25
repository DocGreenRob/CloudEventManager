using CloudEventManager.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace CloudEventManager.Extensions
{
	public static class MessageExtensions
	{
		public static string GetBody(this Message instance)
		{
			return instance?.Body?.ToUtf8();
		}

		public static int GetCurrentRetryCounter(this Message instance)
		{
			instance.ValidateArgNotNull(nameof(instance));

			return (int)(instance.UserProperties.ContainsKey(ServiceGlobals.RetryValues.RetryCounterKeyName) ?
									instance.UserProperties[ServiceGlobals.RetryValues.RetryCounterKeyName] : 1);
		}

		public static TimeSpan GetMaxElapsedRetryTimeSpan(this Message instance, TimeSpan maxBackoffTimeSpan)
		{
			instance.ValidateArgNotNull(nameof(instance));

			var currentRetryCounter = instance.GetCurrentRetryCounter();
			var result = new TimeSpan();

			for (int i = 1; i < currentRetryCounter; i++)
			{
				var backoff = TimeSpan.FromMilliseconds(Math.Pow(2, i) * 1000 + 1000);

				if (backoff > maxBackoffTimeSpan)
				{
					backoff = maxBackoffTimeSpan;
				}
				result.Add(backoff);
			}

			return result;
		}

		public static string GetUserProperty(this Message instance, string key)
		{
			if (instance?.UserProperties == null
				|| !instance.UserProperties.TryGetValue(key, out object value))
			{
				return null;
			}

			return value?.ToString()?.Trim();
		}

		public static int IncrementRetryCounter(this Message instance)
		{
			instance.ValidateArgNotNull(nameof(instance));

			var newCounter = instance.GetCurrentRetryCounter() + 1;
			instance.UserProperties[ServiceGlobals.RetryValues.RetryCounterKeyName] = newCounter;

			return newCounter;
		}
		public static Message ResetRetryCounter(this Message instance)
		{
			instance.ValidateArgNotNull(nameof(instance));

			if (instance.UserProperties.ContainsKey(ServiceGlobals.RetryValues.RetryCounterKeyName))
			{
				instance.UserProperties.Remove(ServiceGlobals.RetryValues.RetryCounterKeyName);
			}
			instance.UserProperties[ServiceGlobals.RetryValues.RetryCounterKeyName] = 0;

			return instance;
		}
		public static bool TryParse<TMessage>(this Message instance, out TMessage result)
					where TMessage : class
		{
			result = null;

			if (instance != null)
			{
				try
				{
					result = string.IsNullOrEmpty(instance.GetBody()) ?
										null : JsonConvert.DeserializeObject<TMessage>(instance.GetBody());
				}
				catch (InvalidCastException)
				{ }
			}

			return result != null;
		}

		public static bool TryParse<TMessage>(this Message instance, IContractResolver contractResolver, out TMessage result)
					where TMessage : class
		{
			result = null;

			if (instance != null)
			{
				try
				{
					result = string.IsNullOrEmpty(instance.GetBody()) ?
										null : JsonConvert.DeserializeObject<TMessage>(instance.GetBody(), new JsonSerializerSettings { ContractResolver = contractResolver });
				}
				catch (InvalidCastException)
				{ }
			}

			return result != null;
		}
	}
}
