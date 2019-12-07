using System;
using Jasmine.Core.Contracts;
using Jasmine.Core.Notification;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Jasmine.Core
{
    public abstract class ModuleBase : IModule
    {
        protected IRegionManager RegionManager { get; }
        private readonly ILookupItemAuthorizationProvider _provider;
        readonly INotificationManager _notificationManager;

        protected ModuleBase(IRegionManager regionManager, ILookupItemAuthorizationProvider provider, INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
            RegionManager = regionManager;
            _provider = provider;
        }

        public abstract Type GetNavigationView();

      
   
        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            RegisterLookupItemAuthorization(_provider);
            var navBar = GetNavigationView();
            if (navBar != null) RegionManager.RegisterViewWithRegion(KnownRegions.NavbarControlRegion, GetNavigationView());
            RegisterAssembly(_notificationManager);
            RegisterNotifications(_notificationManager);
        }

        public virtual void RegisterAssembly(INotificationManager notificationManager)
        {

        }

        public virtual void RegisterNotifications(INotificationManager notificationManager)
        {

        }

        public abstract void RegisterLookupItemAuthorization(ILookupItemAuthorizationProvider provider);
    }
}