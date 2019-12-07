using Prism.Ioc;
using Prism.Services.Dialogs;

namespace Jasmine.Core.Prism.Registrations
{
    public class FluentContainerRegistry : ICanRegisterCollectionViewAndDialogAndService, ICanRegisterDialogAndService, ICanRegisterServiceAndRepository
    {
        private readonly IContainerRegistry _registry;

        public FluentContainerRegistry(IContainerRegistry registry)
        {
            _registry = registry;
        }



        public ICanRegisterCollectionViewAndDialogAndService CollectionView<T>() where T : class
        {
            _registry.RegisterForNavigation<T>();
            return this;
        }

        public ICanRegisterCollectionViewAndDialogAndService CollectionView<TView, TViewModel>() where TView : class where TViewModel : class
        {
            _registry.RegisterForNavigation<TView,TViewModel>();
            return this;
        }

        public ICanRegisterDialogAndService WithDialog<T>() where T : class
        {
            _registry.RegisterDialog<T>();            
            return this;
        }

        public ICanRegisterDialogAndService WithDialog<TView, TViewModel>() where TView : class where TViewModel : class, IDialogAware
        {
            _registry.RegisterDialog<TView,TViewModel>();
            return this;
        }

        public ICanRegisterServiceAndRepository WithService<TFrom, TTo>() where TTo : TFrom
        {
            _registry.RegisterSingleton<TFrom, TTo>();
            return this;
        }

        public ICanRegisterRepository WithRepository<TFrom, TTo>() where TTo : TFrom
        {
            _registry.RegisterSingleton<TFrom, TTo>();
            return this;
        }
        
    }
}