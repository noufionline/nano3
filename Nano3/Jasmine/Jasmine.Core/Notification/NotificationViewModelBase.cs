using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Linq;
using System.Windows;
using PostSharp.Patterns.Model;
using Prism.Commands;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using Jasmine.Core.Mvvm;
using Jasmine.Core.Notification.Views;
using Commands = Prism.Commands;
using System.Threading.Tasks;
using Jasmine.Core.Contracts;
using Jasmine.Core.Aspects;
using Prism.Events;

namespace Jasmine.Core.Notification
{

    [NotifyPropertyChanged]
    public abstract class NotificationViewModelBase<TEntity> : DxMvvmServicesBase, INotificationAware, IRegionMemberLifetime, INavigationAware where TEntity : NotificationModel
    {
        readonly IRegionManager _regionManager;
        public NotificationInfo NotificationInfo { get; set; }
        readonly INotificationManagerService _service;
        readonly IEventAggregator _eventAggregator;
        public INotificationService DefaultNotificationService => GetService<INotificationService>("DefaultNotificationService");
        public IDispatcherService DispatcherService => GetService<IDispatcherService>();

        public NotificationViewModelBase(IRegionManager regionManager, INotificationManagerService service, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _service = service;
            _regionManager = regionManager;
            ShowCommand = new Commands.DelegateCommand(ExecuteShow);
            DismissCommand = new Commands.DelegateCommand(ExecuteDismiss);
        }

        public bool CanNotify => Entity != null && Entity.Message != null && Entity.Message != "NA";

        public string Name { get; set; }
        public TEntity Entity { get; set; }
        protected abstract Task<TEntity> GetEntityAsync();
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue("notificationInfo", out NotificationInfo notificationInfo))
            {
                NotificationInfo = notificationInfo;
                Name = notificationInfo.Name;
                await AddOrRemoveNoticationAsync();
            }
        }
        async Task AddOrRemoveNoticationAsync()
        {
            var entity = await GetEntityAsync();

            if (entity == null || entity.Message == "NA")
            {
                RemoveNotification();
                return;
            }

            if (Entity?.Message != entity.Message)
            {
                Entity = entity;
                if (DefaultNotificationService != null)
                {
                    
                    INotification notificationService = DefaultNotificationService.CreateCustomNotification(
                        new { Type = entity.Type, Message = entity.Message });
                    await notificationService.ShowAsync();
                }                
            }
            _eventAggregator.GetEvent<NotificationUpdatedEvent>().Publish();

        }

        public void RemoveNotification()
        {
            var notifications = _regionManager.Regions[KnownRegions.NotificationRegion].Views.OfType<FrameworkElement>().ToList();
            if (notifications.Count > 0)
            {
                var unNotify = notifications
                    .SingleOrDefault(x => x.DataContext is INotificationAware i && string.Compare(i.Name, Name, StringComparison.Ordinal) == 0);
                if (unNotify != null)
                {
                    _regionManager.Regions[KnownRegions.NotificationRegion].Remove(unNotify);
                    _eventAggregator.GetEvent<NotificationUpdatedEvent>().Publish();
                }
            }

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue("notificationInfo", out NotificationInfo notificationInfo))
            {
                if (Name != notificationInfo.Name)
                    return false;
            }
            return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #region ShowCommand

        public Commands.DelegateCommand ShowCommand { get; set; }

        [ShowWaitIndicator()]
        private void ExecuteShow()
        {
            ShowNotification();
        }

        public void ShowNotification()
        {
            if (NotificationInfo.AutoDismiss)
                RemoveNotification();

            CriteriaOperator filter = NotificationInfo.Filter;
            _regionManager.RequestNavigate("DocumentRegion", NotificationInfo.CollectionViewName, new NavigationParameters { { "filter", filter } });
        }

        #endregion

        #region DismissCommand

        public Commands.DelegateCommand DismissCommand { get; set; }

        public bool KeepAlive => true;

        private void ExecuteDismiss()
        {
            RemoveNotification();
        }
        #endregion
    }
}
