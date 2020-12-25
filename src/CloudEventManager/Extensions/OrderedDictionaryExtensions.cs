using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace CloudEventManager.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="OrderedDictionary"/>
	/// </summary>
	public static class OrderedDictionaryExtensions
	{
		public static string AsHeaderString(this OrderedDictionary dictionary)
		{
			var allValues = new StringBuilder();
			int count = 0;
			int total = dictionary.Count;

			foreach (DictionaryEntry kvp in dictionary)
			{
				allValues.Append($"{kvp.Key}={kvp.Value}");

				if (count++ < total - 1)
				{
					allValues.Append(",");
				}
			}

			return allValues.ToString();
		}

		/// <summary>
		/// Populates <see cref="OrderedDictionary"/> with the key value pairs extracted from the <paramref name="initializer"/>
		/// </summary>
		/// <param name="dictionary">The <see cref="OrderedDictionary"/> to populate with the KeyValuePairs extracted from the <paramref name="initializer"/></param>
		/// <param name="initializer">The KeyValuePair string (i.e., key1=val1,key2=val2...)</param>
		public static OrderedDictionary InitializeFromString(this OrderedDictionary dictionary, string initializer, char pairDelimiter = ',', char keyValueDelimiter = '=')
		{
			dictionary = new OrderedDictionary();

			if (initializer == null)
			{
				return dictionary;
			}

			var kvps = initializer.Split(new char[] { pairDelimiter }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string kvp in kvps)
			{
				var pair = kvp.Split(new char[] { keyValueDelimiter });
				if (pair.Length != 0)
				{
					dictionary[pair[0]] = string.Join(keyValueDelimiter.ToString(), pair.Skip(1));
				}
			}

			return dictionary;
		}
	}
}
