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

	public class ApplicationContext : IApplicationContext
	{
		public bool IsComponentTestRequest { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public Dictionary<string, List<string>> TestScenarios => throw new NotImplementedException();

		public DateTime TransactionDateTimeUtc => throw new NotImplementedException();

		public void Add<T>(T value)
		{
			throw new NotImplementedException();
		}

		public void AddCustomHeader(string name, StringValues value)
		{
			throw new NotImplementedException();
		}

		public T Get<T>()
		{
			throw new NotImplementedException();
		}

		public Dictionary<string, StringValues> GetCustomHeaders()
		{
			throw new NotImplementedException();
		}
	}
}
