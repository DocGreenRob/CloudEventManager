using CloudEventManager.Manager.Implementation.Messaging.Factories;
using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation
{
	public class CloudEventManager : ICloudEventManager
	{
		private readonly CloudEventNotificationService _cloudEventNotificationService;
		private readonly IContractResolver _contractResolver;
		private readonly IHttpClient _httpClient;
		private readonly IMessagePublisherFactory _messagePublisherFactory;

		public CloudEventManager(CloudEventManagerConfiguration cloudEventManagerConfiguration)
		{
			_messagePublisherFactory = new RabbitMQFactory();
			_httpClient = new HttpClient();
			_contractResolver = new ContractResolver();
			Func<string> func = () => { return "hello world"; };
			_cloudEventNotificationService = new CloudEventNotificationService(_messagePublisherFactory, _httpClient, _contractResolver, cloudEventManagerConfiguration);
		}

		public Task DoWork()
		{
			throw new System.NotImplementedException();
		}
	}
}
