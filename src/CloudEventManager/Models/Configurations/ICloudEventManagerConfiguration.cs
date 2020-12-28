using System;
using System.Collections.Generic;

namespace CloudEventManager.Models.Configurations
{
	public interface ICloudEventManagerConfiguration
	{
		string ConnectionStringHostName { get; set; }
		string TopicName { get; set; }
		string ExchangeName { get; set; }
		RetrySetting RetryConfigurationSetting { get; set; }
		IEnumerable<Type> RetryExceptionTypes { get; set; }
	}
}
