using System.ComponentModel;
using DevExpress.Mvvm;

namespace Jasmine.Core.Mvvm
{
    public abstract class DxMvvmServicesBase : ISupportServices
    {
        private IServiceContainer _serviceContainer;
        IServiceContainer ISupportServices.ServiceContainer => ServiceContainer;

        protected IServiceContainer ServiceContainer =>
            _serviceContainer ?? (_serviceContainer = CreateServiceContainer());

        protected IServiceContainer CreateServiceContainer() => new ServiceContainer(this);

        [PostSharp.Patterns.Model.Pure]
        protected T GetService<T>() where T : class => GetService<T>(ServiceSearchMode.PreferLocal);

        [PostSharp.Patterns.Model.Pure]
        protected T GetService<T>(string key) where T : class => GetService<T>(key, ServiceSearchMode.PreferLocal);


        [EditorBrowsable(EditorBrowsableState.Never)]
        protected T GetService<T>(ServiceSearchMode searchMode) where T : class =>
            ServiceContainer.GetService<T>(searchMode);

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected T GetService<T>(string key, ServiceSearchMode searchMode) where T : class =>
           ServiceContainer.GetService<T>(key, searchMode);
    }
}