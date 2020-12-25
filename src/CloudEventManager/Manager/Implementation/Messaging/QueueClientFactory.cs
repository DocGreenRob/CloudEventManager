using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using System;

namespace CloudEventManager.Manager.Implementation.Messaging
{
	public class QueueClientFactory : IQueueClientFactory
	{
		public IQueueClient GetQueueClient(string serviceBusConnectionStringName, string entityPath, bool createNewQueueClient = false)
		{
			throw new NotImplementedException();
		}
	}
}
