using CloudEventManager.Manager.Implementation;
using CloudEventManager.Models;
using System.Threading.Tasks;

namespace CloudEventManager.Manager.Interface
{
	public interface INotificationService
	{
		Task SendNotificationAsync(Message message, MessageReceiver messageReceiver);
	}
}
