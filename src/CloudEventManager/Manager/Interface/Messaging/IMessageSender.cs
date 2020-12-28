using CloudEventManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Interface.Messaging
{
	public interface IMessageSender
	{
		Task CancelScheduledMessageAsync(long sequenceNumber, string routingKey);
		Task CloseAsync();
		string Path { get; }
		bool IsClosedOrClosing { get;}
		Task<long> ScheduleMessageAsync(Message message, DateTimeOffset scheduleEnqueueTimeUtc, string routingKey);
		Task SendAsync(Message message, string routingKey);
		Task SendAsync(IList<Message> messageList, string routingKey);
	}
}
