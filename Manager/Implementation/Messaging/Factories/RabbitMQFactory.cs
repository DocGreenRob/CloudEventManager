using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using System;
using RabbitMQ.Client;
using System.Text;

namespace CloudEventManager.Manager.Implementation.Messaging.Factories
{
	public class RabbitMQFactory : IMessagePublisherFactory
	{
		public IMessagePublisher GetMessagePublisher(string serviceBusConnectionStringName, string entityName)
		{

			throw new NotImplementedException();
		}

		public IQueueClient GetQueueClient(string serviceBusConnectionStringName, string entityPath)
		{
			throw new NotImplementedException();
		}
	}
}
