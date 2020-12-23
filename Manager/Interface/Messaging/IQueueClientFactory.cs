namespace CloudEventManager.Manager.Interface.Messaging
{
	public interface IQueueClientFactory
	{
		IQueueClient GetQueueClient(string serviceBusConnectionStringName, string entityPath, bool createNewQueueClient = false);
	}
}
