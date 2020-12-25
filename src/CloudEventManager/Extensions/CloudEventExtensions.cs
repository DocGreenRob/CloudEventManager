using CloudEventManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CloudEventManager.Extensions
{
	public static class CloudEventExtensions
	{
		public static StringContent GetBody(this CloudEvent cloudEvent, string version)
		{
			var content = (string.IsNullOrWhiteSpace(version) || version.Equals("1")) ? cloudEvent.Data : JsonConvert.SerializeObject(cloudEvent);

			return new StringContent(content, Encoding.UTF8, cloudEvent.DataContentType);
		}
	}
}
