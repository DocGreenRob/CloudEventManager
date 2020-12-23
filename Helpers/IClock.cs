using System;

namespace CloudEventManager.Helpers
{
	public interface IClock
	{
		DateTimeOffset UtcNow { get; }
	}
}
