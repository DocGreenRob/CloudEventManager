using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudEventManager.Extensions
{
	public static class GenericExtensions
	{
		private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
		{
			ContractResolver = new OrderedContractResolver()
		};

		private static Dictionary<Type, Func<object, bool>> _validationActions = new Dictionary<Type, Func<object, bool>>
		{
			[typeof(string)] = (x) => string.IsNullOrWhiteSpace(x?.ToString())
		};

		public static IEnumerable<T> Extract<T>(this T[] instance, int size)
			where T : class, new()
		{
			if (instance == null)
			{
				throw new ArgumentNullException(nameof(instance));
			}

			if (size <= 0)
			{
				throw new InvalidOperationException("Size must be greater than 0");
			}

			var count = instance.Length;

			count = count > size ? size : count;

			for (int i = 0; i < size; i++)
			{
				yield return i < count ? instance[i] : new T();
			}
		}

		public static bool IsJsonEqual<T>(this T instance, T compareObject)
		{
			return JsonConvert.SerializeObject(instance) == JsonConvert.SerializeObject(compareObject);
		}

		public static bool IsJsonEqual<T>(this T instance, dynamic compareObject)
		{
			return JsonConvert.SerializeObject(instance, _jsonSerializerSettings) == JsonConvert.SerializeObject(compareObject, _jsonSerializerSettings);
		}

		public static T ValidateArgNotNull<T>(this T instance, string paramName)
		{
			if (string.IsNullOrWhiteSpace(paramName))
			{
				throw new ArgumentNullException(nameof(paramName));
			}

			Func<object, bool> validationFunction = null;

			if (!_validationActions.TryGetValue(typeof(T), out validationFunction))
			{
				validationFunction = (x) => x == null;
			}

			if (validationFunction(instance))
			{
				throw new ArgumentNullException(paramName);
			}

			return instance;
		}

		private class OrderedContractResolver : DefaultContractResolver
		{
			protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
			{
				return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
			}
		}
	}
}
