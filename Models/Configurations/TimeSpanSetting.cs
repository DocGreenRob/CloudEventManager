using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Models.Configurations
{
	public class TimeSpanSetting
	{
		public int Hours { get; set; }
		public int Minutes { get; set; }
		public int Seconds { get; set; }
		public double? TotalMilliseconds { get; set; }

		public TimeSpan ToTimeSpan()
		{
			if (TotalMilliseconds.HasValue && TotalMilliseconds.Value > 0)
			{
				return TimeSpan.FromMilliseconds(TotalMilliseconds.Value);
			}

			return new TimeSpan(Hours, Minutes, Seconds);
		}
	}
}
