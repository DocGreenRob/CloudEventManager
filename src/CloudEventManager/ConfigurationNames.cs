using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager
{
	public class ConfigurationNames : ConfigurationNamesBase
	{
	}

	public abstract class ConfigurationNamesBase
	{
		public const string ServiceConnectionString = nameof(ServiceConnectionString);
		public static class EnvironmentConfigurations
		{
			public const string EnvironmentName = "ASPNETCORE_ENVIRONMENT";
		}
	}
}
