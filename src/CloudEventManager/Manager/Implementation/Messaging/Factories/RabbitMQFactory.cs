using CloudEventManager.Common;
using CloudEventManager.Extensions;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Manager.Logging;
using CloudEventManager.Models;
using CloudEventManager.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation.Messaging.Factories
{
	public class RabbitMQFactory : IMessageSender, IMessagePublisher
	{
		private readonly IApplicationContext _applicationContext;
		private readonly ICloudEventManagerConfiguration _cloudEventManagerConfiguration;
		private readonly IConfiguration _configuration;
		private readonly ConnectionFactory _connectionFactory;
		private readonly IContractResolver _contractResolver;
		private readonly ITelemetryClient _telemetryClient;

		public RabbitMQFactory(IConfiguration configuration,
			 ITelemetryClient telemetryClient,
			 IApplicationContext applicationContext,
			 ICloudEventManagerConfiguration cloudEventManagerConfiguration,
			 IContractResolver contractResolver = null)
		{
			_configuration = configuration.ValidateArgNotNull(nameof(configuration));
			_telemetryClient = telemetryClient.ValidateArgNotNull(nameof(telemetryClient));
			_applicationContext = applicationContext.ValidateArgNotNull(nameof(applicationContext));
			_contractResolver = contractResolver.ValidateArgNotNull(nameof(contractResolver));
			_cloudEventManagerConfiguration = cloudEventManagerConfiguration.ValidateArgNotNull(nameof(cloudEventManagerConfiguration));
			_connectionFactory = new ConnectionFactory() { HostName = _cloudEventManagerConfiguration.ConnectionStringHostName };
		}

		public Guid Id => throw new NotImplementedException();
		public bool IsClosedOrClosing => throw new NotImplementedException();
		public int MaxRetryCount => throw new NotImplementedException();
		public string Path => throw new NotImplementedException();
		public Task CancelScheduledMessageAsync(long sequenceNumber, string routingKey)
		{
			throw new NotImplementedException();
		}

		public Task CloseAsync()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public Task ResendMessageAsync(Message message)
		{
			throw new NotImplementedException();
		}

		public Task ResendMessageAsync(Message message, TimeSpan maxWaitTimeSpan)
		{
			throw new NotImplementedException();
		}

		public Task<long> ScheduleMessageAsync(Message message, DateTimeOffset scheduleEnqueueTimeUtc, string routingKey)
		{
			throw new NotImplementedException();
		}

		public Task SendAsync(Message message, string routingKey)
		{
			using(var connection = _connectionFactory.CreateConnection())
			using(var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(exchange: _cloudEventManagerConfiguration.ExchangeName, type: ExchangeType.Topic);

				var properties = channel.CreateBasicProperties();
				properties.Persistent = true;

				if (!string.IsNullOrEmpty(message.CorrelationId))
				{
					properties.CorrelationId = message.CorrelationId;
				}

				if(message.UserProperties != null && message.UserProperties.Count > 0)
				{
					properties.Headers = message.UserProperties;
				}

				channel.BasicPublish(exchange: _cloudEventManagerConfiguration.ExchangeName,
									 routingKey: routingKey,
									 basicProperties: properties,
									 body: message.Body);
			}

			return Task.FromResult("");
		}

		public async Task SendAsync(IList<Message> messageList, string routingKey)
		{
			foreach(var message in messageList)
			{
				await SendAsync(message, routingKey).ConfigureAwait(false);
			}
		}

		public Task SendAsync<TEntity>(object item, string id, string telemetryDependencyName, params KeyValuePair<string, string>[] userProperties) where TEntity : class, new()
		{
			throw new NotImplementedException();
		}

		public void ValidateRetryAttempt(Message message)
		{
			throw new NotImplementedException();
		}

		public void ValidateRetryAttempt(Message message, RetrySetting retrySetting)
		{
			throw new NotImplementedException();
		}
	}
}
