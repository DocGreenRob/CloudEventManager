using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Manager.Interface
{
	// Microsoft.Azure.ServiceBus.IQueueClient
	//
	// Summary:
	//     QueueClient can be used for all basic interactions with a Service Bus Queue.
	//
	// Remarks:
	//     Use Microsoft.Azure.ServiceBus.Core.IMessageSender or Microsoft.Azure.ServiceBus.Core.IMessageReceiver
	//     for advanced set of functionality.
	public interface IQueueClient
	{
		//
		// Summary:
		//     Gets the name of the queue.
		string QueueName { get; }

		//
		// Summary:
		//     Receive session messages continuously from the queue. Registers a message handler
		//     and begins a new thread to receive session-messages. This handler(System.Func`4)
		//     is awaited on every time a new message is received by the queue client.
		//
		// Parameters:
		//   handler:
		//     A System.Func`4 that processes messages. Microsoft.Azure.ServiceBus.IMessageSession
		//     contains the session information, and must be used to perform Complete/Abandon/Deadletter
		//     or other such operations on the Microsoft.Azure.ServiceBus.Message
		//
		//   sessionHandlerOptions:
		//     Options used to configure the settings of the session pump.
		//
		// Remarks:
		//     Enable prefetch to speed up the receive rate.
		void RegisterSessionHandler();
	}
}
