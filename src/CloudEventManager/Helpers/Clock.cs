using System;

namespace CloudEventManager.Helpers
{
	public class Clock : IClock
	{
		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
	}
}
