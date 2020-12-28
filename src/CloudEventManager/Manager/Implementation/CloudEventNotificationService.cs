using CloudEventManager.Extensions;
using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Models;
using CloudEventManager.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Implementation
{
	public class CloudEventNotificationService : CloudEventNotificationServiceBase
	{
		public CloudEventNotificationService(IMessagePublisherFactory messagePublisherFactory,
			IHttpClient httpClient,
			IContractResolver contractResolver,
			ICloudEventManagerConfiguration cloudEventManagerConfiguration) : base(messagePublisherFactory, httpClient, contractResolver, cloudEventManagerConfiguration)
		{
		}

		protected override Task<Uri> GetCallbackUriAsync(Message message)
		{
			throw new NotImplementedException();
		}

		public async Task SendNotificationAsync(CloudEvent messageModel, string telemetryDependencyName, params KeyValuePair<string, string>[] userProperties)
		{
			await Publisher.SendAsync<CloudEvent>(messageModel, messageModel.Id, telemetryDependencyName, userProperties).ConfigureAwait(false);
		}
	}
}
