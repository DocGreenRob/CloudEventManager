using System;
using System.Collections.Generic;
using System.Text;

namespace CloudEventManager.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="Byte"/> arrays
	/// </summary>
	public static class ByteCharExtensions
	{
		public static Random rnd = new Random();
		private static readonly Random _rnd = new Random();

		/// <summary>
		/// Determines if character is a valid HEX
		/// </summary>
		/// <param name="instance">The character</param>
		/// <returns>The boolean result</returns>
		public static bool IsValidHexChar(this char instance)
		{
			if (instance is default(char) || instance == ' ' || string.IsNullOrWhiteSpace(instance.ToString()))
			{
				return false;
			}

			return (instance >= '0' && instance <= '9') ||
				  (instance >= 'a' && instance <= 'f') ||
				  (instance >= 'A' && instance <= 'F');
		}

		/// <summary>
		/// Converts a byte array to ASCII
		/// </summary>
		/// <param name="instance">The byte[]</param>
		/// <returns>The ASCII encoded string</returns>
		/// <remarks>Returns null if <paramref name="instance"/> is null or the default for <see cref="byte[]"/></remarks>
		public static string ToAsciiString(this byte[] instance)
		{
			if (instance == null || instance == default(byte[]))
			{
				return null;
			}

			return System.Text.Encoding.ASCII.GetString(instance);
		}

		/// <summary>
		/// Converts the byte array to UTF8
		/// </summary>
		/// <param name="bytes">The bytes to convert</param>
		/// <returns>The encoded UTF8 byte array</returns>
		public static string ToUtf8(this byte[] bytes)
		{
			return System.Text.Encoding.UTF8.GetString(bytes);
		}

		/// <summary>
		/// Populates the byte array from the value
		/// </summary>
		/// <param name="bytes">The byte array to populate</param>
		/// <param name="value">The string of values to use to populate the byte array</param>
		/// <returns>The boolean result</returns>
		public static bool TrySetBytesFromValue(this byte[] bytes, string value)
		{
			var isValid = true;
			int byteIndex = bytes.Length - 1;
			int partLength = value.Length - 1;

			for (int i = partLength; i >= 0; i--)
			{
				if (IsValidHexChar(value[i]))
				{
					bytes[byteIndex] = Convert.ToByte(value[i]);
					byteIndex--;
					if (byteIndex < 0)
						break;
				}
				else
				{
					isValid = false;
					partLength--;
				}
			}

			return isValid;
		}

		public static void FillWithEmptyRandom(this byte[] bytes, int partLength)
		{
			// make sure its filled could be optimized to get all bytes at once
			if (partLength + 1 < bytes.Length)
			{
				byte[] randomByte = new byte[1];
				for (int j = bytes.Length - partLength; j >= 0; j--)
				{
					rnd.NextBytes(randomByte);
					bytes[j] = randomByte[0];
				}
			}
		}

		public static string BytesToAsciiString(this byte[] bytes)
		{
			return System.Text.Encoding.ASCII.GetString(bytes);
		}

		public static void GetRandomBytes(this byte[] bytes)
		{
			string validChars = "123456789abcdef";
			int validCharsLength = validChars.Length - 1;

			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = Convert.ToByte(validChars[rnd.Next(validCharsLength)]);
			}
			return;
		}

		public static bool SetBytesFromValue(this byte[] bytes, string value)
		{
			var isValid = true;
			int byteIndex = bytes.Length - 1;
			int partLength = value.Length - 1;

			for (int i = partLength; i >= 0; i--)
			{
				if (IsValidHexChar(value[i]))
				{
					bytes[byteIndex] = Convert.ToByte(value[i]);
					byteIndex--;
					if (byteIndex < 0)
						break;
				}
				else
				{
					isValid = false;
					partLength--;
				}
			}
			// make sure its filled could be optimized to get all bytes at once
			if (partLength + 1 < bytes.Length)
			{
				byte[] randomByte = new byte[1];
				for (int j = bytes.Length - partLength; j >= 0; j--)
				{
					rnd.NextBytes(randomByte);
					bytes[j] = randomByte[0];
				}
			}
			return isValid;
		}
	}
}
