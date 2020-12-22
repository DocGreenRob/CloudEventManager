using CloudEventManager.Extensions;
using CloudEventManager.Manager.Interface;
using CloudEventManager.Models.Configurations;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace CloudEventManager.Manager.Implementation
{
	public abstract class NotificationServiceBase<T> : CloudEventNotificationServiceBase<T>
		where T : class
	{
		internal static IEnumerable<Type> DefaultRetryExceptionTypes = new Type[]
		{
			typeof(Exception),
			//typeof(DocumentClientException)
		};

		protected NotificationServiceBase(IMessagePublisherFactory messagePublisherFactory,
			IHttpClient httpClient,
			IContractResolver contractResolver,
			RetryConfiguration retryConfiguration)
			: base(messagePublisherFactory, httpClient, contractResolver)
		{
			RetryConfiguration = retryConfiguration.ValidateArgNotNull(nameof(retryConfiguration));
		}

		protected override string ClientId => "Unknown";
		protected override string ConnectionStringConfigurationName => ConfigurationNames.ServiceConnectionString;
		protected RetryConfiguration RetryConfiguration { get; }
		protected override IEnumerable<Type> RetryExceptionTypes => DefaultRetryExceptionTypes;
	}
}
