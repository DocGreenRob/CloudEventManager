namespace CloudEventManager.Helpers
{
	public interface IRandomNumberGenerator
	{
		int GenerateRandomNumber();
		int GenerateRandomNumber(int maxValue);
	}
}
