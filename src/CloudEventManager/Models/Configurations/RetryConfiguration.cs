using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Models.Configurations
{
	public class RetryConfiguration
	{
		public RetrySetting CallbackNotificationSetting { get; set; }
		public RetrySetting SubmissionSetting { get; set; }
		public RetrySetting UpdateDecisionsSetting { get; set; }
	}
}
