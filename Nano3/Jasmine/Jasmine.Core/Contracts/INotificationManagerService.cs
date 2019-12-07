using Jasmine.Core.Notification;
using Jasmine.Core.Notification.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmine.Core.Contracts
{
    public interface INotificationManagerService
    {
        Task<List<NotificationInfoBase>> GetNotificationLookups();        
        Task<NotificationModel> GetNotification(int id, List<NotificationParameter> parameters);
    }
}
