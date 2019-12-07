using DevExpress.Xpf.Core.Native;
using Jasmine.Core.Contracts;
using Jasmine.Core.Dialogs;
using PostSharp.Patterns.Model;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using IDialogService=Prism.Services.Dialogs.IDialogService;
namespace Jasmine.Core.Notification.Views
{
    public class ManageNotificationViewModel : Mvvm.ViewModelBase
    {


        readonly INotificationManager _notificationManager;
        public ManageNotificationViewModel(IEventAggregator eventAggregator, IDialogService dialogService, 
            INotificationManager notificationManager,
            IAuthorizationCache authorizationCache) : base(eventAggregator, dialogService, authorizationCache)
        {
            _notificationManager = notificationManager;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            ApprovedNotifications = _notificationManager.GetNotificationInfos().OrderByDescending(i => i.Enable).ToList();
        }


        public override bool ShowValidationSummary => false;

        public override string Caption => "Notification Settings";       

        public List<NotificationInfo> ApprovedNotifications { get; set; }
    }
}
