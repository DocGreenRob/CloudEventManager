using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CloudEventManager.Models.Configurations
{
	public class RetrySetting
	{
		public int? MaxAttempts { get; set; }
		public TimeSpanSetting MaxBackoffSetting { get; set; }
		public TimeSpanSetting MaxElapsedRetrySetting { get; set; }

		#region JsonIgnore Properties
		[JsonIgnore]
		public TimeSpan MaxBackoffTimeSpan => MaxBackoffSetting == null ? ServiceGlobals.RetryValues.DefaultMaxBackoffTimeSpan
			: MaxBackoffSetting.ToTimeSpan();

		[JsonIgnore]
		public TimeSpan MaxElapsedRetryTimeSpan => MaxElapsedRetrySetting == null ? ServiceGlobals.RetryValues.DefaultMaxElapsedRetryTimeSpan
			: MaxElapsedRetrySetting.ToTimeSpan();
		#endregion JsonIgnore Properties
	}
}
