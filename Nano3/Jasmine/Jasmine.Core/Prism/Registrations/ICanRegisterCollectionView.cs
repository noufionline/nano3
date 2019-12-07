namespace Jasmine.Core.Prism.Registrations
{
    public interface ICanRegisterCollectionView
    {
        ICanRegisterCollectionViewAndDialogAndService CollectionView<T>() where T : class;
        ICanRegisterCollectionViewAndDialogAndService CollectionView<TView,TViewModel>()   where TView : class
            where TViewModel:class;
    }
}