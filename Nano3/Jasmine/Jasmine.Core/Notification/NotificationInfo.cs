using DevExpress.Data.Filtering;
using Jasmine.Core.Notification.Views;
using System;
using System.Collections.Generic;

namespace Jasmine.Core.Notification
{
    public class NotificationInfo : NotificationInfoBase
    {
        public Type NotificationView { get; set; } = typeof(NotificationView);
        public new List<NotificationParameter> Parameters { get; set; } = new List<NotificationParameter>();
        public new CriteriaOperator Filter { get; set; }
        public DateTime NextNotificationTime { get; set; } = DateTime.Now;
        public bool Approved { get; set; }
        public bool Enable { get; set; }
        public bool Subscribed { get; set; }
    }

    public class NotificationInfoBase : LookupItem
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public double Interval { get; set; }
        public string Parameters { get; set; }
        public string CollectionViewName { get; set; }
        public string Filter { get; set; }
        public bool AutoDismiss { get; set; }

    }
}