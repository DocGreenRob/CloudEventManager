using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace CloudEventManager.Common
{
	public interface IApplicationContext
	{
		bool IsComponentTestRequest { get; set; }
		Dictionary<string, List<string>> TestScenarios { get; }
		DateTime TransactionDateTimeUtc { get; }
		void Add<T>(T value);
		void AddCustomHeader(string name, StringValues value);
		T Get<T>();
		Dictionary<string, StringValues> GetCustomHeaders();
	}
}
