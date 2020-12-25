using CloudEventManager.Manager.Interface.Messaging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CloudEventManager.Manager.Implementation.Messaging
{
	public class MessageSenderFactory : IMessageSenderFactory
	{
		private readonly IConfiguration _configuration;
		private readonly IDictionary<string, IMessageSender> _messageSenders = new Dictionary<string, IMessageSender>();

		public MessageSenderFactory(
			IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IMessageSender GetMessageSender(string serviceBusConnectionStringName, string entityName)
		{
			var key = $"{serviceBusConnectionStringName}-{entityName}";

			if (_messageSenders.ContainsKey(key))
			{
				var sender = _messageSenders[key];

				if (sender == null || sender.IsClosedOrClosing)
				{
					_messageSenders.Remove(key);
					_messageSenders[key] = InitializeNewMessageSender(_configuration[serviceBusConnectionStringName], entityName);
				}
			}
			else
			{
				_messageSenders[key] = InitializeNewMessageSender(_configuration[serviceBusConnectionStringName], entityName);
			}

			return _messageSenders[key];
		}

		internal IMessageSender InitializeNewMessageSender(string serviceBusConnectionString, string entityName)
		{
			return new MessageSender(serviceBusConnectionString, entityName);
		}
	}
}
