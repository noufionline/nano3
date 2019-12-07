using Jasmine.Core.Tracking;
using System;
using System.Linq;

namespace Jasmine.Core.Notification.Views
{
    public class NotificationModel : EntityBase
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
