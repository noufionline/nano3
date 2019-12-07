using Prism.Modularity;
using System;
using System.Collections.Generic;

namespace Jasmine.Core.Notification
{
    public interface INotificationManager : ISupportRegisterAssembly, ISupportRegisterNotifications, ISupportStart
    {
        List<NotificationInfo> GetNotificationInfos();
    }

}
