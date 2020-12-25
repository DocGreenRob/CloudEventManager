using CloudEventManager.Common;
using CloudEventManager.Manager.Implementation.Messaging.Factories;
using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Manager.Logging;
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
		private readonly ICloudEventManagerConfiguration2 _cloudEventManagerConfiguration;
		private readonly CloudEventNotificationService _cloudEventNotificationService;
		private readonly IContractResolver _contractResolver;
		private readonly IHttpClient _httpClient;
		private readonly ITelemetryClient _telemetryClient;
		private readonly IApplicationContext _applicationContext;
		private readonly IMessagePublisherFactory _messagePublisherFactory;
		private readonly IMessageSenderFactory _messageSenderFactory;

		public CloudEventManager(ICloudEventManagerConfiguration2 cloudEventManagerConfiguration,
			IConfiguration configuration,
			ITelemetryClient telemetryClient,
			IApplicationContext applicationContext,
			IMessageSenderFactory messageSenderFactory,
			IContractResolver contractResolver = null)
		{
			_contractResolver = contractResolver ?? throw new ArgumentNullException(nameof(contractResolver));
			_messageSenderFactory = messageSenderFactory ?? throw new ArgumentNullException(nameof(messageSenderFactory));
			_telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
			_applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
			_contractResolver = contractResolver ?? throw new ArgumentNullException(nameof(contractResolver));
			_cloudEventManagerConfiguration = cloudEventManagerConfiguration ?? throw new ArgumentNullException(nameof(cloudEventManagerConfiguration));

			_messagePublisherFactory = new RabbitMQFactory(configuration ?? throw new ArgumentNullException(nameof(configuration)),
															_telemetryClient,
															_applicationContext,
															_messageSenderFactory,
															_contractResolver);
			_httpClient = new HttpClient();
			_contractResolver = new ContractResolver();
			Func<string> func = () => { return "hello world"; };
			_cloudEventNotificationService = new CloudEventNotificationService(_messagePublisherFactory, _httpClient, _contractResolver, _cloudEventManagerConfiguration);
		}

		public Task ExecuteAsync<T1>(T1 obj)
		{
			var eventType = JsonConvert.SerializeObject(obj);
			throw new NotImplementedException();
		}
	}
}
