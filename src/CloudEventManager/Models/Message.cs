using CloudEventManager.Extensions;
using System.Collections.Generic;

namespace CloudEventManager.Models
{
	public class Message
	{
		public Message() { }
		public Message(byte[] body)
		{
			Body = body.ValidateArgNotNull(nameof(body));
		}

		/// <summary>
		/// Gets the "user properties" bag, which can be used for custom message metadata.
		/// </summary>
		public IDictionary<string, object> UserProperties { get; }

		/// <remarks>
		/// Allows an application to specify a context for the message for the purposes of
		/// correlation, for example reflecting the MessageId of a message that is being
		/// replied to. See Message Routing and Correlation.
		/// </remarks>
		public string CorrelationId { get; set; }

		/// <summary>
		/// Gets or sets the body of the message.
		/// </summary>
		/// <remarks>
		/// The easiest way to create a new message from a string is the following:
		/// message.Body = System.Text.Encoding.UTF8.GetBytes("Message1");
		/// </remarks>
		public byte[] Body { get; set; }
	}
}
