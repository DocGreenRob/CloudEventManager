using System;

namespace CloudEventManager.Models.Exceptions
{
	public class RetryException : Exception
	{
		public RetryException() : base(ErrorMessages.RetryExceeded)
		{ }

		public RetryException(Exception innerException) : base(ErrorMessages.RetryExceeded, innerException)
		{ }

		public RetryException(string message) : base(message)
		{
		}

		public RetryException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
