using System.Threading.Tasks;

namespace CloudEventManager.Manager.Interface
{
	public interface ICloudEventManager
	{
		Task DoWork();
	}
}
