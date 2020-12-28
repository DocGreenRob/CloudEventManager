using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace CloudEventManager.Common
{
	public class ApplicationContext : IApplicationContext
	{
		private readonly Dictionary<Type, object> _contextObjects;
		private readonly Dictionary<string, StringValues> _customHeaders;

		public bool IsComponentTestRequest { get; set; }
		public Dictionary<string, List<string>> TestScenarios { get; } = new Dictionary<string, List<string>>();
		public DateTime TransactionDateTimeUtc { get; } = DateTime.UtcNow;

		public virtual void Add<T>(T value)
		{
			if (_contextObjects.ContainsKey(typeof(T)))
			{
				_contextObjects[typeof(T)] = value;
			}
			else
			{
				_contextObjects.Add(typeof(T), value);
			}
		}

		public virtual void AddCustomHeader(string name, StringValues value)
		{
			if (_customHeaders.ContainsKey(name))
			{
				_customHeaders[name] = value;
			}
			else
			{
				_customHeaders.Add(name, value);
			}
		}

		public virtual T Get<T>()
		{
			object result = null;

			if (_contextObjects.TryGetValue(typeof(T), out result))
			{
				return (T)result;
			}

			return default(T);
		}

		public virtual Dictionary<string, StringValues> GetCustomHeaders()
		{
			return _customHeaders;
		}
	}
}
