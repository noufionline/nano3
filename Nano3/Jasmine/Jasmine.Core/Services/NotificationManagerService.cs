using Jasmine.Core.Contracts;
using Jasmine.Core.Notification;
using Jasmine.Core.Notification.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmine.Core.Services
{
    public class NotificationManagerService : INotificationManagerService
    {
        private readonly INotificationManagerRepository _repository;

        public NotificationManagerService(INotificationManagerRepository repository)
        {
            _repository = repository;
        }

        public Task<List<NotificationInfoBase>> GetNotificationLookups()
        {
            return _repository.GetNotificationLookups();
        }

        public Task<NotificationModel> GetNotification(int id, List<NotificationParameter> parameters)
        {
            return _repository.GetNotification(id, parameters);
        }
    }
}
