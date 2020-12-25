using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Models.Configurations
{
	public interface ITest { void DoWork(); }
	public class Test : ITest
	{
		public void DoWork()
		{
			throw new NotImplementedException();
		}
	}
	public interface ICloudEventManagerConfiguration2
	{
		string ConnectionStringConfigurationName { get; set; }
		string QueueName { get; set; }
		RetrySetting RetryConfigurationSetting { get; set; }
		IEnumerable<Type> RetryExceptionTypes { get; set; }
	}
	public class CloudEventManagerConfiguration2 : ICloudEventManagerConfiguration2
	{
		public string ConnectionStringConfigurationName { get; set; }
		public string QueueName { get; set; }
		public RetrySetting RetryConfigurationSetting { get; set; }
		public IEnumerable<Type> RetryExceptionTypes { get; set; }
	}
	public interface ICloudEventManagerConfiguration
	{
		string ConnectionStringConfigurationName { get; set; }
		string QueueName { get; set; }
		RetrySetting RetryConfigurationSetting { get; set; }
		IEnumerable<Type> RetryExceptionTypes { get; set; }
	}

	public class CloudEventManagerConfiguration : ICloudEventManagerConfiguration
	{
		public CloudEventManagerConfiguration()
		{

		}
		public string ConnectionStringConfigurationName { get; set; }
		public string QueueName { get; set; }
		public RetrySetting RetryConfigurationSetting { get; set; }
		public IEnumerable<Type> RetryExceptionTypes { get; set; }
	}
}
