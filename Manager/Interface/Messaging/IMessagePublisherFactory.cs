using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Manager.Interface.Messaging
{
	public interface IMessagePublisherFactory
	{
		IMessagePublisher GetMessagePublisher(string serviceBusConnectionStringName, string entityName);

		IQueueClient GetQueueClient(string serviceBusConnectionStringName, string entityPath);
	}
}
