using CloudEventManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Interface.Messaging
{
	public interface IMessageSender
	{
		Task CancelScheduledMessageAsync(long sequenceNumber);
		Task CloseAsync();
		string Path { get; }
		bool IsClosedOrClosing { get;}
		Task<long> ScheduleMessageAsync(Message message, DateTimeOffset scheduleEnqueueTimeUtc);
		Task SendAsync(Message message);
		Task SendAsync(IList<Message> messageList);
	}
}
