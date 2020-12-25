using Newtonsoft.Json.Serialization;
using System;

namespace CloudEventManager.Manager.Implementation
{
	public class ContractResolver : IContractResolver
	{
		public JsonContract ResolveContract(Type type)
		{
			throw new NotImplementedException();
		}
	}
}
