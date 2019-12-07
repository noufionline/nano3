using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Jasmine.Core.Common
{
    public class NavigationParameterParser
    {
        private readonly IDialogParameters _parameters;


        public NavigationParameterParser(IDialogParameters parameters)
        {
            _parameters = parameters;
        }

        public ObservableCollection<T> GetAll<T>(string key)
        {
            return _parameters.GetValue<List<T>>(key).ToObservableList();
        }

        public List<T> GetList<T>(string key) => _parameters.GetValue<List<T>>(key);

        public T GetValue<T>(string key) => _parameters.GetValue<T>(key);
        public ObservableCollection<LookupItem> GetLookupItems(string key) => _parameters.GetValue<List<LookupItem>>(key).ToObservableList();
    }

    public abstract class NavigationParameterUpdaterBase
    {
        protected abstract Task UpdateParameters(NavigationParameters parameters);
    }

    public abstract class ViewModelLookupItemProviderServiceBase<TViewModel,TModel> where TViewModel: class where TModel: class
    {
        public abstract Task FillLookupItemsAsync(TViewModel viewModel, TModel model);
    }

}