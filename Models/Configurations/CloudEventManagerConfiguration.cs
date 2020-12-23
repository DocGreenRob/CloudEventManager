using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Models.Configurations
{
	public class CloudEventManagerConfiguration
	{
		public string ConnectionStringConfigurationName { get; set; }
		public string QueueName { get; set; }
		public RetrySetting RetryConfigurationSetting { get; set; }
		public IEnumerable<Type> RetryExceptionTypes { get; set; }
	}
}
