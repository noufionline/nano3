using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using Jasmine.Core.Contracts;
using Jasmine.Core.Tracking;
using PostSharp.Patterns.Model;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jasmine.Core.Notification.Views
{
    public class NotificationViewModel : NotificationViewModelBase<NotificationModel>
    {        
        readonly IRegionManager _regionManager;
        readonly INotificationManagerService _service;

        public NotificationViewModel(IRegionManager regionManager, INotificationManagerService service, IEventAggregator eventAggregator) : base(regionManager, service, eventAggregator)
        {
            _service = service;
            _regionManager = regionManager;
        }

        protected override async Task<NotificationModel> GetEntityAsync()
        {
            return await _service.GetNotification(NotificationInfo.Id, NotificationInfo.Parameters); 
        }
    }
}
