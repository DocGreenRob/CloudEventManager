using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CloudEventManager.Models
{
	public class ServiceException : Exception
	{
		public List<string> Messages = new List<string>();
		public ServiceException(HttpStatusCode code, string message, Exception innerException = null, params string[] messages) : base(message, innerException)
		{
			if (messages != null)
			{
				foreach (var m in messages.Where(x => !string.IsNullOrWhiteSpace(x)))
				{
					Messages.Add(m);
				}
			}

			HttpStatusCode = code;
		}

		public HttpStatusCode HttpStatusCode { get; }
		public void AddMessage(string message)
		{
			Messages.Add(message);
		}
	}
}
