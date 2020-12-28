using CloudEventManager.Common;
using CloudEventManager.Logging;
using CloudEventManager.Manager.Implementation.Messaging.Factories;
using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Manager.Logging;
using CloudEventManager.Models;
using CloudEventManager.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation
{
	public class CloudEventManager : ICloudEventManager
	{
		private readonly ICloudEventManagerConfiguration _cloudEventManagerConfiguration;
		private readonly IContractResolver _contractResolver;
		private readonly ITelemetryClient _telemetryClient;
		private readonly IApplicationContext _applicationContext;
		private readonly IConfiguration _configuration;
		private readonly RabbitMQFactory _rabbitMQFactory;

		public CloudEventManager(ICloudEventManagerConfiguration cloudEventManagerConfiguration,
			IConfiguration configuration,
			ITelemetryClient telemetryClient,
			IApplicationContext applicationContext,
			IContractResolver contractResolver = null)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_contractResolver = contractResolver ?? throw new ArgumentNullException(nameof(contractResolver));
			_telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
			_applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
			_contractResolver = contractResolver ?? throw new ArgumentNullException(nameof(contractResolver));
			_cloudEventManagerConfiguration = cloudEventManagerConfiguration ?? throw new ArgumentNullException(nameof(cloudEventManagerConfiguration));

			_rabbitMQFactory = new RabbitMQFactory(_configuration,
													_telemetryClient,
													_applicationContext,
													_cloudEventManagerConfiguration,
													_contractResolver);
		}
		private int _currentSendAttempts = 1;
		public async Task ExecuteAsync<T>(T data, string routingKey, string correlationId = null)
		{
			// 1. Create Cloud Event (_cloudEventNotificationService)
			var cloudEvent = new CloudEvent
			{
				Id = correlationId,
				EventType = routingKey,
				Subject = routingKey,
				Data = JsonConvert.SerializeObject(data),
				DataContentType = ServiceGlobals.Logging.MediaTypes.Json,
				Source = routingKey,
				Time = _applicationContext.TransactionDateTimeUtc
			};

			var message = new Message
			{
				Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cloudEvent)),
				CorrelationId = correlationId
			};

			try
			{
				await _rabbitMQFactory.SendAsync(message, routingKey).ConfigureAwait(false);
			}
			catch(BrokerUnreachableException brokerUnreachableException)
			{
				if(_currentSendAttempts < _cloudEventManagerConfiguration.RetryConfigurationSetting.MaxAttempts)
				{
					_currentSendAttempts += 1;
					//await ExecuteAsync<T>(data, routingKey, correlationId).ConfigureAwait(false);
					await _rabbitMQFactory.ResendMessageAsync(message, routingKey).ConfigureAwait(false);
				}
			}
			catch (Exception ex)
			{

				throw;
			}
			
		}
	}
}
