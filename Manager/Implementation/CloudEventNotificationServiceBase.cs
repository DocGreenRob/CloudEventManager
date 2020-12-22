using CloudEventManager.Extensions;
using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Logging;
using CloudEventManager.Models;
using CloudEventManager.Models.Configurations;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation
{
	public abstract class CloudEventNotificationServiceBase<T> : INotificationService<T>
		where T : class
	{
		protected CloudEventNotificationServiceBase(IMessagePublisherFactory messagePublisherFactory,
			IHttpClient httpClient,
			IContractResolver contractResolver)
		{
			Publisher = messagePublisherFactory.ValidateArgNotNull(nameof(messagePublisherFactory))
				.GetMessagePublisher(ConnectionStringConfigurationName,
						QueueName.ValidateArgNotNull(nameof(QueueName)));
			HttpClient = httpClient.ValidateArgNotNull(nameof(httpClient));
			ContractResolver = contractResolver.ValidateArgNotNull(nameof(contractResolver));
		}

		protected virtual string ClientId => "UnknownUser";
		protected abstract string ConnectionStringConfigurationName { get; }
		protected IContractResolver ContractResolver { get; }
		protected IHttpClient HttpClient { get; }
		protected IMessagePublisher Publisher { get; }
		protected abstract string QueueName { get; }
		protected abstract RetrySetting RetryConfigurationSetting { get; }
		protected abstract IEnumerable<Type> RetryExceptionTypes { get; }
		protected ITelemetryClient TelemetryClient { get; }

		public async Task SendNotificationAsync(Message message, MessageReceiver messageReceiver)
		{
			var testableMessageReceiver = GetMessageReceiver(messageReceiver);
			testableMessageReceiver.ValidateArgNotNull(nameof(messageReceiver));

			if (!message.TryParse(ContractResolver, out CloudEvent cloudEvent))
			{
				throw new ArgumentException(ServiceGlobals.Exception.ERRInvalidMessageType, nameof(message));
			}

			try
			{
				Publisher.ValidateRetryAttempt(message, RetryConfigurationSetting);
				var version = message.GetUserProperty("version");

				var callbackUri = await GetCallbackUriAsync(message).ConfigureAwait(false);

				if (callbackUri == null)
				{
					throw new InvalidOperationException(ServiceGlobals.Exception.ERRInvalidCallbackUri);
				}

				using (var request = new HttpRequestMessage(HttpMethod.Post, callbackUri))
				{
					request.Content = cloudEvent.GetBody(version);
					var response = await HttpClient.SendAsync(request)
						.ConfigureAwait(false);

					if (response.StatusCode != HttpStatusCode.OK)
					{
						throw new ServiceException(response.StatusCode, ServiceGlobals.Exception.ERRInvalidStatusCodeFromClient);
					}
				}
			}
			catch (Exception ex)
			{
				TelemetryClient.TrackMessageException<CloudEvent>(message, $"FailedCallback-{typeof(T).Name}", ex);

				if (RetryExceptionTypes.Any(x => x == ex.GetType() || ex.GetType().IsSubclassOf(x)))
				{
					await Publisher.ResendMessageAsync(message, RetryConfigurationSetting.MaxBackoffTimeSpan)
							   .ConfigureAwait(false);
				}
			}
		}

		internal virtual string GetLockToken(Message message)
		{
			return message.SystemProperties.LockToken;
		}

		internal virtual IMessageReceiver GetMessageReceiver(MessageReceiver messageReceiver)
		{
			return messageReceiver as IMessageReceiver;
		}

		protected abstract Task<Uri> GetCallbackUriAsync(Message message);

	}
}
