using CloudEventManager.Common;
using CloudEventManager.Extensions;
using CloudEventManager.Helpers;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Manager.Logging;
using CloudEventManager.Models;
using CloudEventManager.Models.Configurations;
using CloudEventManager.Models.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation.Messaging
{
	public class MessagePublisher : IMessagePublisher, IDisposable
	{
		internal const int DefaultMaxRetryCount = 10;

		private static JsonSerializerSettings _jsonSerializerSettings;
		private readonly IClock _clock;
		private readonly IConfiguration _configuration;
		private readonly IMessageSenderFactory _messageSenderFactory;
		private readonly IRandomNumberGenerator _randomNumberGenerator;

		public MessagePublisher(IConfiguration configuration,
			 ITelemetryClient telemetryClient,
			 string serviceBusConnectionStringName,
			 IApplicationContext applicationContext,
			 IMessageSenderFactory messageSenderFactory,
			 IContractResolver contractResolver = null)
		{
			_configuration = configuration.ValidateArgNotNull(nameof(configuration));
			ServicebusConnectionString = configuration[serviceBusConnectionStringName];
			ServicebusConnectionStringName = serviceBusConnectionStringName;
			ApplicationContext = applicationContext;
			if (string.IsNullOrWhiteSpace(ServicebusConnectionString))
			{
				throw new InvalidOperationException(string.Format(ServiceGlobals.Exception.ERRMissingConfiguration, serviceBusConnectionStringName));
			}
			TelemetryClient = telemetryClient.ValidateArgNotNull(nameof(telemetryClient));
			_randomNumberGenerator = new RandomNumberGenerator();
			_clock = new Clock();
			_messageSenderFactory = messageSenderFactory;
			MaxRetryCount = GetMaxRetryCount(configuration);

			_jsonSerializerSettings = _jsonSerializerSettings ?? new JsonSerializerSettings
			{
				ContractResolver = contractResolver ?? new DefaultContractResolver()
			};
		}

		public Guid Id { get; } = Guid.NewGuid();
		public bool IsClosedOrClosing => disposedValue || InternalMessageSender.IsClosedOrClosing;
		public int MaxRetryCount { get; }

		internal IApplicationContext ApplicationContext { get; }
		internal IMessageSender InternalMessageSender { get; private set; }
		internal string ServicebusConnectionString { get; }
		internal string ServicebusConnectionStringName { get; }
		internal ITelemetryClient TelemetryClient { get; }
		public async Task CloseAsync()
		{
			if (InternalMessageSender != null && !InternalMessageSender.IsClosedOrClosing)
			{
				await InternalMessageSender.CloseAsync().ConfigureAwait(false);
			}
		}

		public Task ResendMessageAsync(Message message, TimeSpan maxWaitTimeSpan)
		{
			return InternalResendMessageAsync(message, maxWaitTimeSpan.TotalMilliseconds);
		}

		public Task ResendMessageAsync(Message message)
		{
			return InternalResendMessageAsync(message);
		}

		public async Task SendAsync<TEntity>(object item, string id, string telemetryDependencyName, params KeyValuePair<string, string>[] userProperties)
			where TEntity : class, new()
		{
			item.ValidateArgNotNull(nameof(item));

			if (!(item is string))
			{
				item = JsonConvert.SerializeObject(item, _jsonSerializerSettings);
			}

			if (InternalMessageSender.IsClosedOrClosing)
			{
				throw new InvalidOperationException(ServiceGlobals.Exception.MessagePublisherConnectionError);
			}

			var entityName = typeof(TEntity).Name;
			var startTime = DateTime.UtcNow;
			var timer = System.Diagnostics.Stopwatch.StartNew();

			try
			{
				await InternalMessageSender.SendAsync(CreateServiceBusMessage(item, userProperties))
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				TelemetryClient.TrackException(ex);
				TelemetryClient.TrackEvent($"Service-{entityName}QueuePublishFailed", TelemetryProperties(id, entityName, ex));
				throw;
			}

			timer.Stop();
			TelemetryClient.TrackDependency($"{entityName}TimetoQueue", telemetryDependencyName, entityName, startTime, timer.Elapsed, true);
			TelemetryClient.TrackEvent($"Service-{entityName}Accepted", TelemetryProperties(id, entityName, null));
		}

		public void ValidateRetryAttempt(Message message)
		{
			message.ValidateArgNotNull(nameof(message));

			if (message.GetCurrentRetryCounter() > MaxRetryCount)
			{
				throw new RetryException();
			}
		}

		public void ValidateRetryAttempt(Message message, RetrySetting retrySetting)
		{
			message.ValidateArgNotNull(nameof(message));

			if (message.GetMaxElapsedRetryTimeSpan(retrySetting.MaxBackoffTimeSpan) > retrySetting.MaxElapsedRetryTimeSpan)
			{
				throw new RetryException();
			}
		}

		internal IMessagePublisher Initialize(string entityName)
		{
			InternalMessageSender = _messageSenderFactory.GetMessageSender(ServicebusConnectionStringName, entityName);
			return this;
		}

		protected virtual Message Clone(Message message)
		{
			return message.ValidateArgNotNull(nameof(message))
				.Clone();
		}

		private static int GetMaxRetryCount(IConfiguration configuration)
		{
			if (!int.TryParse(configuration[ServiceGlobals.RetryValues.MaxRetryCount], out int maxRetry))
			{
				maxRetry = DefaultMaxRetryCount;
			}
			return maxRetry;
		}

		private Message CreateServiceBusMessage(object message, params KeyValuePair<string, string>[] userProperties)
		{
			var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(message.ToString()));
			if (userProperties != null)
			{
				foreach (var prop in userProperties)
				{
					serviceBusMessage.UserProperties.Add(prop.Key, prop.Value);
				}
			}

			var traceHeader = ApplicationContext.Get<ITraceHeader>();
			if (traceHeader != null)
			{
				serviceBusMessage.UserProperties.Add(ServiceGlobals.Logging.TraceConstants.TraceParent, traceHeader.TraceParent.ToTraceString());
				serviceBusMessage.UserProperties.Add(ServiceGlobals.Logging.TraceConstants.TraceState, traceHeader.GetTraceState());
			}

			return serviceBusMessage;
		}

		private async Task InternalResendMessageAsync(Message message, double? maxWaitTimeInMilliseconds = null)
		{
			message.ValidateArgNotNull(nameof(message));
			ValidateRetryAttempt(message);

			if (!maxWaitTimeInMilliseconds.HasValue)
			{
				maxWaitTimeInMilliseconds = (_configuration[ServiceGlobals.RetryValues.MaxWaitTimeInMilliseconds] == null) ? ServiceGlobals.RetryValues.MaximumBackoffTimeInMilliseconds : Convert.ToInt32(_configuration[ServiceGlobals.RetryValues.MaxWaitTimeInMilliseconds]);
			}

			var jitterNumber = _randomNumberGenerator.GenerateRandomNumber(1000);
			var retryTimeSpan = TimeSpan.FromMilliseconds(Math.Pow(2, message.IncrementRetryCounter() - 1) * 1000 + jitterNumber);

			if (retryTimeSpan.Milliseconds > maxWaitTimeInMilliseconds)
			{
				retryTimeSpan = TimeSpan.FromMilliseconds(maxWaitTimeInMilliseconds.Value + jitterNumber);
			}

			//if seqnum > 0 then the message was put on the queue.  If not, it failed to put the message on the queue.
			var seqNum = await InternalMessageSender.ScheduleMessageAsync(Clone(message), _clock.UtcNow.Add(retryTimeSpan)).ConfigureAwait(false);

			if (seqNum <= 0)
			{
				throw new RetryException(ErrorMessages.ReQueueError);
			}

			TelemetryClient.TrackEvent($"{ServiceGlobals.Logging.Events.ResendMessage}-{InternalMessageSender.Path}",
				new Dictionary<string, string>
				{
					["CurrentRetryCounter"] = message.GetCurrentRetryCounter().ToString(),
					["RetryTimeSpanTotalMilliseconds"] = retryTimeSpan.TotalMilliseconds.ToString()
				});
		}

		private Dictionary<string, string> TelemetryProperties(string id,
			string payload,
			Exception ex)
		{
			if (ex != null)
			{
				var x = new Dictionary<string, string>
				{
					{ ServiceGlobals.Logging.Keys.Id, id },
					{ ServiceGlobals.Logging.Keys.Exception, ex.Message },
					{ ServiceGlobals.Logging.Keys.Stacktrace, ex.StackTrace },
					{ ServiceGlobals.Logging.Keys.Payload, payload}
				};

				return x;
			}
			else
			{
				return new Dictionary<string, string>
			   {
				   { ServiceGlobals.Logging.Keys.Id, id }
			   };
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					CloseAsync().Wait();
				}

				disposedValue = true;
			}
		}
		#endregion
	}
}
