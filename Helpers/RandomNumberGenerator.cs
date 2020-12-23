using System;

namespace CloudEventManager.Helpers
{
	public class RandomNumberGenerator : IRandomNumberGenerator
	{
		private static readonly Random _random = new Random();
		public int GenerateRandomNumber()
		{
			return _random.Next();
		}

		public int GenerateRandomNumber(int maxValue)
		{
			return _random.Next(maxValue);
		}
	}
}
