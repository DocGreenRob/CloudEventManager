using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation.Messaging
{
	public class MessageSender : IMessageSender
	{
		public MessageSender(string connectionString, string targetName)
		{

		}

		bool IMessageSender.IsClosedOrClosing => throw new NotImplementedException();
		public string Path => throw new NotImplementedException();
		public Task CancelScheduledMessageAsync(long sequenceNumber)
		{
			throw new NotImplementedException();
		}

		public Task CloseAsync()
		{
			throw new NotImplementedException();
		}

		public bool IsClosedOrClosing()
		{
			throw new NotImplementedException();
		}

		public Task<long> ScheduleMessageAsync(Message message, DateTimeOffset scheduleEnqueueTimeUtc)
		{
			throw new NotImplementedException();
		}

		public Task SendAsync(Message message)
		{
			throw new NotImplementedException();
		}

		public Task SendAsync(IList<Message> messageList)
		{
			throw new NotImplementedException();
		}
	}
}
