using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Models
{
	// cloned from Microsoft.Azure.ServiceBus.Message
	public class Message
	{
		//
		// Summary:
		//     User property key representing deadletter reason, when a message is received
		//     from a deadletter subqueue of an entity.
		public static string DeadLetterReasonHeader;
		//
		// Summary:
		//     User property key representing detailed error description, when a message is
		//     received from a deadletter subqueue of an entity.
		public static string DeadLetterErrorDescriptionHeader;

		//
		// Summary:
		//     Creates a new Message
		public Message() { }
		//
		// Summary:
		//     Creates a new message from the specified payload.
		//
		// Parameters:
		//   body:
		//     The payload of the message in bytes
		public Message(byte[] body) { }

		//
		// Summary:
		//     Gets the "user properties" bag, which can be used for custom message metadata.
		//
		// Remarks:
		//     Only following value types are supported: byte, sbyte, char, short, ushort, int,
		//     uint, long, ulong, float, double, decimal, bool, Guid, string, Uri, DateTime,
		//     DateTimeOffset, TimeSpan
		public IDictionary<string, object> UserProperties { get; }
		//
		// Summary:
		//     Gets the total size of the message body in bytes.
		public long Size { get; }
		//
		// Summary:
		//     Gets or sets the date and time in UTC at which the message will be enqueued.
		//     This property returns the time in UTC; when setting the property, the supplied
		//     DateTime value must also be in UTC.
		//
		// Value:
		//     The scheduled enqueue time in UTC. This value is for delayed message sending.
		//     It is utilized to delay messages sending to a specific time in the future.
		//
		// Remarks:
		//     Message enqueuing time does not mean that the message will be sent at the same
		//     time. It will get enqueued, but the actual sending time depends on the queue's
		//     workload and its state.
		public DateTime ScheduledEnqueueTimeUtc { get; set; }
		//
		// Summary:
		//     Gets or sets the address of an entity to send replies to.
		//
		// Value:
		//     The reply entity address.
		//
		// Remarks:
		//     This optional and application-defined value is a standard way to express a reply
		//     path to the receiver of the message. When a sender expects a reply, it sets the
		//     value to the absolute or relative path of the queue or topic it expects the reply
		//     to be sent to. See Message Routing and Correlation.
		public string ReplyTo { get; set; }
		//
		// Summary:
		//     Gets or sets the content type descriptor.
		//
		// Value:
		//     RFC2045 Content-Type descriptor.
		//
		// Remarks:
		//     Optionally describes the payload of the message, with a descriptor following
		//     the format of RFC2045, Section 5, for example "application/json".
		public string ContentType { get; set; }
		//
		// Summary:
		//     Gets or sets the "to" address.
		//
		// Value:
		//     The "to" address.
		//
		// Remarks:
		//     This property is reserved for future use in routing scenarios and presently ignored
		//     by the broker itself. Applications can use this value in rule-driven auto-forward
		//     chaining scenarios to indicate the intended logical destination of the message.
		public string To { get; set; }
		//
		// Summary:
		//     Gets or sets an application specific label.
		//
		// Value:
		//     The application specific label
		//
		// Remarks:
		//     This property enables the application to indicate the purpose of the message
		//     to the receiver in a standardized fashion, similar to an email subject line.
		//     The mapped AMQP property is "subject".
		public string Label { get; set; }
		//
		// Summary:
		//     Gets or sets the a correlation identifier.
		//
		// Value:
		//     Correlation identifier.
		//
		// Remarks:
		//     Allows an application to specify a context for the message for the purposes of
		//     correlation, for example reflecting the MessageId of a message that is being
		//     replied to. See Message Routing and Correlation.
		public string CorrelationId { get; set; }
		//
		// Summary:
		//     Gets or sets the message’s "time to live" value.
		//
		// Value:
		//     The message’s time to live value.
		//
		// Remarks:
		//     This value is the relative duration after which the message expires, starting
		//     from the instant the message has been accepted and stored by the broker, as captured
		//     in Microsoft.Azure.ServiceBus.Message.SystemPropertiesCollection.EnqueuedTimeUtc.
		//     When not set explicitly, the assumed value is the DefaultTimeToLive for the respective
		//     queue or topic. A message-level Microsoft.Azure.ServiceBus.Message.TimeToLive
		//     value cannot be longer than the entity's DefaultTimeToLive setting and it is
		//     silently adjusted if it does. See Expiration
		public TimeSpan TimeToLive { get; set; }
		//
		// Summary:
		//     Gets or sets a session identifier augmenting the Microsoft.Azure.ServiceBus.Message.ReplyTo
		//     address.
		//
		// Value:
		//     Session identifier. Maximum length is 128 characters.
		//
		// Remarks:
		//     This value augments the ReplyTo information and specifies which SessionId should
		//     be set for the reply when sent to the reply entity. See Message Routing and Correlation
		public string ReplyToSessionId { get; set; }
		//
		// Summary:
		//     Gets the Microsoft.Azure.ServiceBus.Message.SystemPropertiesCollection, which
		//     is used to store properties that are set by the system.
		public SystemPropertiesCollection SystemProperties { get; }
		//
		// Summary:
		//     Gets or sets the session identifier for a session-aware entity.
		//
		// Value:
		//     The session identifier. Maximum length is 128 characters.
		//
		// Remarks:
		//     For session-aware entities, this application-defined value specifies the session
		//     affiliation of the message. Messages with the same session identifier are subject
		//     to summary locking and enable exact in-order processing and demultiplexing. For
		//     session-unaware entities, this value is ignored. See Message Sessions.
		public string SessionId { get; set; }
		//
		// Summary:
		//     Gets or sets a partition key for sending a message into an entity via a partitioned
		//     transfer queue.
		//
		// Value:
		//     The partition key. Maximum length is 128 characters.
		//
		// Remarks:
		//     If a message is sent via a transfer queue in the scope of a transaction, this
		//     value selects the transfer queue partition: This is functionally equivalent to
		//     Microsoft.Azure.ServiceBus.Message.PartitionKey and ensures that messages are
		//     kept together and in order as they are transferred. See Transfers and Send Via.
		public string ViaPartitionKey { get; set; }
		//
		// Summary:
		//     Gets or sets a partition key for sending a message to a partitioned entity.
		//
		// Value:
		//     The partition key. Maximum length is 128 characters.
		//
		// Remarks:
		//     For partitioned entities, setting this value enables assigning related messages
		//     to the same internal partition, so that submission sequence order is correctly
		//     recorded. The partition is chosen by a hash function over this value and cannot
		//     be chosen directly. For session-aware entities, the Microsoft.Azure.ServiceBus.Message.SessionId
		//     property overrides this value.
		public string PartitionKey { get; set; }
		//
		// Summary:
		//     Gets or sets the MessageId to identify the message.
		//
		// Remarks:
		//     The message identifier is an application-defined value that uniquely identifies
		//     the message and its payload. The identifier is a free-form string and can reflect
		//     a GUID or an identifier derived from the application context. If enabled, the
		//     duplicate detection feature identifies and removes second and further submissions
		//     of messages with the same MessageId.
		public string MessageId { get; set; }
		//
		// Summary:
		//     Gets or sets the body of the message.
		//
		// Remarks:
		//     The easiest way to create a new message from a string is the following:
		//     message.Body = System.Text.Encoding.UTF8.GetBytes("Message1");
		public byte[] Body { get; set; }
		//
		// Summary:
		//     Gets the date and time in UTC at which the message is set to expire.
		//
		// Value:
		//     The message expiration time in UTC. This property is read-only.
		//
		// Exceptions:
		//   T:System.InvalidOperationException:
		//     If the message has not been received. For example if a new message was created
		//     but not yet sent and received.
		//
		// Remarks:
		//     The UTC instant at which the message is marked for removal and no longer available
		//     for retrieval from the entity due to expiration. Expiry is controlled by the
		//     Microsoft.Azure.ServiceBus.Message.TimeToLive property and this property is computed
		//     from Microsoft.Azure.ServiceBus.Message.SystemPropertiesCollection.EnqueuedTimeUtc+Microsoft.Azure.ServiceBus.Message.TimeToLive
		public DateTime ExpiresAtUtc { get; }

		//
		// Summary:
		//     Clones a message, so that it is possible to send a clone of an already received
		//     message as a new message. The system properties of original message are not copied.
		//
		// Returns:
		//     A cloned Microsoft.Azure.ServiceBus.Message.
		public Message Clone()
		{
			return (Message)this.MemberwiseClone();
		}
		//
		// Summary:
		//     Returns a string that represents the current message.
		//
		// Returns:
		//     The string representation of the current message.
		public override string ToString() { return JsonConvert.SerializeObject(this); }

		//
		// Summary:
		//     A collection used to store properties which are set by the Service Bus service.
		public sealed class SystemPropertiesCollection
		{
			public SystemPropertiesCollection() { }

			//
			// Summary:
			//     Specifies whether or not there is a lock token set on the current message.
			//
			// Remarks:
			//     A lock token will only be specified if the message was received using Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock
			public bool IsLockTokenSet { get; }
			//
			// Summary:
			//     Gets the lock token for the current message.
			//
			// Remarks:
			//     The lock token is a reference to the lock that is being held by the broker in
			//     Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock mode. Locks are used to explicitly
			//     settle messages as explained in the product documentation in more detail. The
			//     token can also be used to pin the lock permanently through the Deferral API and,
			//     with that, take the message out of the regular delivery state flow. This property
			//     is read-only.
			public string LockToken { get; }
			//
			// Summary:
			//     Specifies if the message has been obtained from the broker.
			public bool IsReceived { get; }
			//
			// Summary:
			//     Get the current delivery count.
			//
			// Value:
			//     This value starts at 1.
			//
			// Remarks:
			//     Number of deliveries that have been attempted for this message. The count is
			//     incremented when a message lock expires, or the message is explicitly abandoned
			//     by the receiver. This property is read-only.
			public int DeliveryCount { get; }
			//
			// Summary:
			//     Gets the date and time in UTC until which the message will be locked in the queue/subscription.
			//
			// Value:
			//     The date and time until which the message will be locked in the queue/subscription.
			//
			// Remarks:
			//     For messages retrieved under a lock (peek-lock receive mode, not pre-settled)
			//     this property reflects the UTC instant until which the message is held locked
			//     in the queue/subscription. When the lock expires, the Microsoft.Azure.ServiceBus.Message.SystemPropertiesCollection.DeliveryCount
			//     is incremented and the message is again available for retrieval. This property
			//     is read-only.
			public DateTime LockedUntilUtc { get; }
			//
			// Summary:
			//     Gets the unique number assigned to a message by Service Bus.
			//
			// Remarks:
			//     The sequence number is a unique 64-bit integer assigned to a message as it is
			//     accepted and stored by the broker and functions as its true identifier. For partitioned
			//     entities, the topmost 16 bits reflect the partition identifier. Sequence numbers
			//     monotonically increase. They roll over to 0 when the 48-64 bit range is exhausted.
			//     This property is read-only.
			public long SequenceNumber { get; }
			//
			// Summary:
			//     Gets the name of the queue or subscription that this message was enqueued on,
			//     before it was deadlettered.
			//
			// Remarks:
			//     Only set in messages that have been dead-lettered and subsequently auto-forwarded
			//     from the dead-letter queue to another entity. Indicates the entity in which the
			//     message was dead-lettered. This property is read-only.
			public string DeadLetterSource { get; }
			//
			// Summary:
			//     Gets or sets the original sequence number of the message.
			//
			// Value:
			//     The enqueued sequence number of the message.
			//
			// Remarks:
			//     For messages that have been auto-forwarded, this property reflects the sequence
			//     number that had first been assigned to the message at its original point of submission.
			//     This property is read-only.
			public long EnqueuedSequenceNumber { get; }
			//
			// Summary:
			//     Gets or sets the date and time of the sent time in UTC.
			//
			// Value:
			//     The enqueue time in UTC.
			//
			// Remarks:
			//     The UTC instant at which the message has been accepted and stored in the entity.
			//     This value can be used as an authoritative and neutral arrival time indicator
			//     when the receiver does not want to trust the sender's clock. This property is
			//     read-only.
			public DateTime EnqueuedTimeUtc { get; }
		}
	}
}
