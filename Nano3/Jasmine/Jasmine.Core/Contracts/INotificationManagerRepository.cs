using Jasmine.Core.Notification;
using Jasmine.Core.Notification.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmine.Core.Contracts
{
    public interface INotificationManagerRepository
    {        
        Task<NotificationModel> GetNotification(int id, List<NotificationParameter> parameters);
        Task<List<NotificationInfoBase>> GetNotificationLookups();
    }
}
