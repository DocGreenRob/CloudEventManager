using CloudEventManager.Common;
using CloudEventManager.Manager.Interface;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation
{
	public class HttpClient : IHttpClient
	{
		public string ProviderName => throw new NotImplementedException();

		public IHttpClient Init(Uri baseAddress, string providerName, bool enableMtls)
		{
			throw new NotImplementedException();
		}

		public IHttpClient Init(Uri baseAddress, string providerName, HttpMessageHandler handler = null)
		{
			throw new NotImplementedException();
		}

		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
		{
			throw new NotImplementedException();
		}

		public void SetContext(IApplicationContext applicationContext)
		{
			throw new NotImplementedException();
		}
	}
}
