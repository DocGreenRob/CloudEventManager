using CloudEventManager.Common;
using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Manager.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Manager.Implementation.Messaging
{
	public class MessagePublisherFactory : IMessagePublisherFactory
	{
		internal IConfiguration Configuration { get; }
		internal ITelemetryClient TelemetryClient { get; }
		internal IApplicationContext ApplicationContext { get; set; }
		internal IMessageSenderFactory MessageSenderFactory { get; }
		internal IContractResolver ContractResolver { get; }
		internal IQueueClientFactory QueueClientFactory { get; }

		public MessagePublisherFactory(IConfiguration configuration,
			ITelemetryClient telemetryClient,
			IApplicationContext applicationContext,
			IMessageSenderFactory messageSenderFactory,
			IQueueClientFactory queueClientFactory,
			IContractResolver contractResolver = null)
		{
			Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			TelemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
			ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
			MessageSenderFactory = messageSenderFactory ?? throw new ArgumentNullException(nameof(messageSenderFactory));
			QueueClientFactory = queueClientFactory ?? throw new ArgumentNullException(nameof(queueClientFactory));
			ContractResolver = contractResolver;
		}

		public IMessagePublisher GetMessagePublisher(string serviceBusConnectionStringName, string entityName)
		{
			return GetMessagePublisher(serviceBusConnectionStringName, entityName, false);
		}

		internal IMessagePublisher GetMessagePublisher(string serviceBusConnectionStringName, string entityName, bool isTesting)
		{
			var key = $"{serviceBusConnectionStringName}-{entityName}";
			if (_messagePublishers.ContainsKey(key))
			{
				var publisher = _messagePublishers[key];

				if (publisher == null || publisher.IsClosedOrClosing)
				{
					_messagePublishers.Remove(key);
					RegisterNewMessagePublisher(serviceBusConnectionStringName, entityName, isTesting, key);
				}
			}
			else
			{
				RegisterNewMessagePublisher(serviceBusConnectionStringName, entityName, isTesting, key);
			}
			return _messagePublishers[key];
		}

		public IQueueClient GetQueueClient(string serviceBusConnectionStringName, string entityPath, bool createNewQueueClient = false)
		{
			return QueueClientFactory.GetQueueClient(serviceBusConnectionStringName, entityPath, createNewQueueClient);
		}

		private void RegisterNewMessagePublisher(string serviceBusConnectionStringName, string entityName, bool isTesting, string key)
		{
			IMessagePublisher messagePublisher = new MessagePublisher(Configuration, TelemetryClient, serviceBusConnectionStringName, ApplicationContext, MessageSenderFactory, ContractResolver);
			if (!isTesting)
			{
				messagePublisher = ((MessagePublisher)messagePublisher).Initialize(entityName);
			}
			_messagePublishers[key] = messagePublisher;
		}

		public IQueueClient GetQueueClient(string serviceBusConnectionStringName, string entityPath)
		{
			throw new NotImplementedException();
		}

		private readonly IDictionary<string, IMessagePublisher> _messagePublishers = new Dictionary<string, IMessagePublisher>();
	}
}
