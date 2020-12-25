using CloudEventManager.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Interface
{
	public interface IHttpClient
	{
		string ProviderName { get; }
		IHttpClient Init(Uri baseAddress, string providerName, bool enableMtls);
		IHttpClient Init(Uri baseAddress, string providerName, HttpMessageHandler handler = null);
		Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
		void SetContext(IApplicationContext applicationContext);
	}
}
