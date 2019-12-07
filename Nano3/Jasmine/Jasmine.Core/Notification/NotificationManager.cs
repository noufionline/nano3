using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using Jasmine.Core.Contracts;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Timers;
using Z.Expressions;

namespace Jasmine.Core.Notification
{
    public class NotificationManager : INotificationManager
    {
        readonly IRegionManager _regionManager;
        List<Func<Task<List<NotificationInfoBase>>>> _getNotificationInfos = new List<Func<Task<List<NotificationInfoBase>>>>();
        List<NotificationInfo> _approvedNotificationInfos = new List<NotificationInfo>();
        List<NotificationInfo> _registeredNotificationInfos = new List<NotificationInfo>();
        readonly IContainerExtension _container;
        EvalContext _context = new EvalContext();
        //static List<Timer> _timers = new List<Timer>();
        readonly IAuthorizationCache _authorizationCache;
        Timer _timer;

        public NotificationManager(IRegionManager regionManager, IContainerExtension container, IAuthorizationCache authorizationCache)
        {
            Task = Task.FromResult<int>(0);
            _authorizationCache = authorizationCache;
            _container = container;
            _regionManager = regionManager;
            _context.RegisterAssembly(this.GetType().Assembly);
            _context.RegisterType(typeof(ClaimsPrincipal));
            _context.RegisterAssembly(typeof(CriteriaOperator).Assembly);
        }

        public void RegisterAssembly(Assembly assembly)
        {
            _context.RegisterAssembly(assembly);
        }

        public void RegisterNotification(NotificationInfo notificationInfo)
        {
            _registeredNotificationInfos.Add(notificationInfo);
        }

        public ISupportStart RegisterNotifications(Func<Task<List<NotificationInfoBase>>> getNotificationInfos)
        {
            _getNotificationInfos.Add(getNotificationInfos);
            return this;
        }


        public async Task<NotificationManager> StartAsync(IDispatcherService DispatcherService, int dueTime = 10000, int period = 10000)
        {
            foreach (var getNotificationInfo in _getNotificationInfos)
            {
                var notificationInfos = await getNotificationInfo();
                if (notificationInfos != null && notificationInfos.Count > 0)
                {
                    foreach (var notificationInfo in notificationInfos)
                    {
                        if (notificationInfo is NotificationInfoBase)
                        {
                            var info = new NotificationInfo
                            {
                                Id = notificationInfo.Id,
                                Name = notificationInfo.Name,
                                Type = notificationInfo.Type,
                                Description = notificationInfo.Description,
                                Interval = notificationInfo.Interval,
                                Parameters = notificationInfo.Parameters != null ? _context.Execute<List<NotificationParameter>>(notificationInfo.Parameters) : null,
                                Filter = notificationInfo.Filter != null ? _context.Execute<CriteriaOperator>(notificationInfo.Filter) : null,
                                CollectionViewName = notificationInfo.CollectionViewName,
                            };

                            _registeredNotificationInfos.Add(info);
                        }
                    }
                }
            }

            foreach (var registeredNotificationInfo in _registeredNotificationInfos)
            {
                if (_authorizationCache.CheckAccess($"Notify{registeredNotificationInfo.Name}Alert"))
                {
                    _approvedNotificationInfos.Add(registeredNotificationInfo);
                    registeredNotificationInfo.Approved = true;
                    registeredNotificationInfo.Enable = true;
                }
            }

            //foreach (var _approvedNotificationInfo in _approvedNotificationInfos)
            //{
            //    _container.RegisterForNavigation(_approvedNotificationInfo.NotificationView, _approvedNotificationInfo.Name);

            //    var timer = new Timer();
            //    timer.Interval = dueTime;
            //    timer.Start();
            //    timer.Elapsed += (s, e) =>
            //    {
            //        timer.Interval = _approvedNotificationInfo.Interval;
            //        DispatcherService.Invoke(() =>
            //        _regionManager.RequestNavigate(KnownRegions.NotificationRegion, _approvedNotificationInfo.Name,
            //        new NavigationParameters { { "notificationInfo", _approvedNotificationInfo } })
            //        );
            //    };
            //    _timers.Add(timer);
            //    dueTime += period;
            //}

            foreach (var approvedNotificationInfo in _approvedNotificationInfos)
            {
                _container.RegisterForNavigation(approvedNotificationInfo.NotificationView, approvedNotificationInfo.Name);
            }

            _timer = new Timer();
            _timer.Interval = dueTime;
            _timer.Start();
            _timer.Elapsed += (s, e) =>
            {
                var notifications = _approvedNotificationInfos.Where(i => i.NextNotificationTime < DateTime.Now && i.Enable).ToList();
                foreach (var notification in notifications)
                {
                    notification.NextNotificationTime = DateTime.Now.AddMilliseconds(notification.Interval);
                    Debug.WriteLine($"Notifying {notification.Name}, Next Notification at {notification.NextNotificationTime}");
                    QueueWork(() =>
                    {
                        DispatcherService.Invoke(() =>
                        _regionManager.RequestNavigate(KnownRegions.NotificationRegion, notification.Name,
                            new NavigationParameters { { "notificationInfo", notification } })
                        );
                    }, period);

                }
            };

            return this;
        }

        public List<NotificationInfo> GetNotificationInfos()
        {
            return _registeredNotificationInfos;
        }

        public Task Task { get; private set; }

        private void QueueWork(Action work, int period)
        {
            // queue up the work
            Task = Task.ContinueWith(task =>
            {
                work();
                Task.Wait(period);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }


        public void Stop()
        {
            //foreach (var item in _timers)
            //{
            //    item.Stop();
            //    item.Dispose();
            //}
            _timer.Stop();
            _timer.Dispose();
            _getNotificationInfos.Clear();
            _registeredNotificationInfos.Clear();
            _approvedNotificationInfos.Clear();
        }

    }


    #region Interfaces
    public interface ISupportRegisterAssembly
    {
        void RegisterAssembly(Assembly assembly);
    }

    public interface ISupportRegisterNotifications
    {
        void RegisterNotification(NotificationInfo getLookups);
        ISupportStart RegisterNotifications(Func<Task<List<NotificationInfoBase>>> getNotificationInfos);
    }

    public interface ISupportStart
    {
        Task<NotificationManager> StartAsync(IDispatcherService DispatcherService, int dueTime = 10000, int period = 10000);
    }


    #endregion
}
