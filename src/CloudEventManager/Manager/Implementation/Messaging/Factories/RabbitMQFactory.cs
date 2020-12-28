using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using System.Threading;
using Microsoft.Extensions.Configuration;
using CloudEventManager.Manager.Logging;
using CloudEventManager.Common;
using Newtonsoft.Json.Serialization;
using CloudEventManager.Extensions;

namespace CloudEventManager.Manager.Implementation.Messaging.Factories
{
	public class RabbitMQFactory : IMessagePublisherFactory
	{
		private readonly IConfiguration _configuration;
		private readonly ITelemetryClient _telemetryClient;
		private readonly IApplicationContext _applicationContext;
		//private readonly IMessageSenderFactory _messageSenderFactory;
		private readonly IContractResolver _contractResolver;

		public RabbitMQFactory(IConfiguration configuration,
			 ITelemetryClient telemetryClient,
			 IApplicationContext applicationContext,
			 IMessageSenderFactory messageSenderFactory,
			 IContractResolver contractResolver = null)
		{
			_configuration = configuration.ValidateArgNotNull(nameof(configuration));
			_telemetryClient = telemetryClient.ValidateArgNotNull(nameof(telemetryClient));
			_applicationContext = applicationContext.ValidateArgNotNull(nameof(applicationContext));
			//_messageSenderFactory = messageSenderFactory.ValidateArgNotNull(nameof(messageSenderFactory));
			_contractResolver = contractResolver.ValidateArgNotNull(nameof(contractResolver));
		}

		public IMessagePublisher GetMessagePublisher(string serviceBusConnectionStringName, string entityName)
		{
			// 1
			//AddMessageToQueue();
			//ReceiveMessageFromQueue();
			// 2
			//AddMessageToWorkTaskQueue("task-1");
			var messageSenderFactory = new MessageSenderFactory(_configuration);
			return new MessagePublisher(_configuration,
				_telemetryClient,
				serviceBusConnectionStringName,
				_applicationContext,
				//messageSenderFactory,
				_contractResolver);
		}

		public IQueueClient GetQueueClient(string serviceBusConnectionStringName, string entityPath)
		{
			throw new NotImplementedException();
		}
	}
}
