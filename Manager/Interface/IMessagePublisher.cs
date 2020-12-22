using CloudEventManager.Models;
using CloudEventManager.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Interface
{
	public interface IMessagePublisher : IDisposable
	{
		Guid Id { get; }
		bool IsClosedOrClosing { get; }
		int MaxRetryCount { get; }
		Task CloseAsync();
		Task ResendMessageAsync(Message message);
		Task ResendMessageAsync(Message message,
			TimeSpan maxWaitTimeSpan);
		Task SendAsync<TEntity>(object item,
			string id,
			string telemetryDependencyName,
			params KeyValuePair<string, string>[] userProperties)
			where TEntity : class, new();
		void ValidateRetryAttempt(Message message);
		void ValidateRetryAttempt(Message message, RetrySetting retrySetting);
	}
}
