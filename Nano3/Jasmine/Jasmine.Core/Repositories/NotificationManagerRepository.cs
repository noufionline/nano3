using Jasmine.Core.Contracts;
using Jasmine.Core.Notification;
using Jasmine.Core.Notification.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Jasmine.Core.Repositories
{
    public class NotificationManagerRepository :RestApiRepositoryBase, INotificationManagerRepository
    {
        
        public NotificationManagerRepository(IHttpClientFactory factory):base(factory)
        {
            
        }


        public Task<List<NotificationInfoBase>> GetNotificationLookups()
        {
            string request = $"notifications/lookup";
            return ReadAllAsStreamAsync<NotificationInfoBase>(request);
        }

        public Task<NotificationModel> GetNotification(int id, List<NotificationParameter> parameters)
        {
            string request;
            if (parameters != null && parameters.Count > 0)
            {
                request = $"notifications/{id}?parameter={JsonConvert.SerializeObject(parameters)}";
            }
            else
            {
                request = $"notifications/{id}";
            }
            return ReadAsStreamAsync<NotificationModel>(request);
        }


    }
}
