namespace CloudEventManager.Manager.Interface.Messaging
{
	public interface IMessageSenderFactory
	{
		IMessageSender GetMessageSender(string serviceBusConnectionString, string entityName);
	}
}
