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
using System;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation
{
	public class CloudEventManager : ICloudEventManager
	{
		private readonly ICloudEventManagerConfiguration _cloudEventManagerConfiguration;
		private readonly CloudEventNotificationService _cloudEventNotificationService;
		private readonly IContractResolver _contractResolver;
		private readonly IHttpClient _httpClient;
		private readonly ITelemetryClient _telemetryClient;
		private readonly IApplicationContext _applicationContext;
		private readonly IMessagePublisherFactory _messagePublisherFactory;
		private readonly IMessageSenderFactory _messageSenderFactory;
		private readonly IConfiguration _configuration;

		public CloudEventManager(ICloudEventManagerConfiguration cloudEventManagerConfiguration,
			IConfiguration configuration,
			ITelemetryClient telemetryClient,
			IApplicationContext applicationContext,
			IMessageSenderFactory messageSenderFactory,
			IHttpClient httpClient,
			IContractResolver contractResolver = null)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_contractResolver = contractResolver ?? throw new ArgumentNullException(nameof(contractResolver));
			_messageSenderFactory = messageSenderFactory ?? throw new ArgumentNullException(nameof(messageSenderFactory));
			_telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
			_applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
			_contractResolver = contractResolver ?? throw new ArgumentNullException(nameof(contractResolver));
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_cloudEventManagerConfiguration = cloudEventManagerConfiguration ?? throw new ArgumentNullException(nameof(cloudEventManagerConfiguration));

			_messagePublisherFactory = new RabbitMQFactory(_configuration,
															_telemetryClient,
															_applicationContext,
															_messageSenderFactory,
															_contractResolver);

			_cloudEventNotificationService = new CloudEventNotificationService(_messagePublisherFactory,
															_httpClient,
															_contractResolver,
															_cloudEventManagerConfiguration);
		}

		public Task ExecuteAsync<T1>(T1 obj)
		{
			// 1. Create Cloud Event (_cloudEventNotificationService)
			var messageModel = new CloudEvent
			{
				Id = TelemetryClient.ContextOperationId,
				EventType = eventType,
				Subject = id,
				Data = data,
				DataContentType = Common.Globals.LoggingGlobals.MediaTypes.Json,
				Source = cloudEventSource,
				Time = ApplicationContext.TransactionDateTimeUtc
			};

			await _cloudEventNotificationService.SendNotificationAsync(messageModel).ConfigureAwait(false);

			// 2. Send to Exchange (_messagePublisherFactory)
			var eventType = JsonConvert.SerializeObject(obj);
			throw new NotImplementedException();
		}
	}
}
