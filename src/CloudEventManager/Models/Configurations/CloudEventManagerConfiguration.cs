using System;
using System.Collections.Generic;

namespace CloudEventManager.Models.Configurations
{
	public class CloudEventManagerConfiguration : ICloudEventManagerConfiguration
	{
		public string ConnectionStringHostName { get; set; }
		public string TopicName { get; set; }
		public string ExchangeName { get; set; }
		public RetrySetting RetryConfigurationSetting { get; set; }
		public IEnumerable<Type> RetryExceptionTypes { get; set; }
	}
}
