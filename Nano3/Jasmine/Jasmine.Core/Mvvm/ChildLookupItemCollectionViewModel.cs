using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using Jasmine.Core.Contracts;
using Prism.Events;
using IDialogService = Prism.Services.Dialogs.IDialogService;

namespace Jasmine.Core.Mvvm
{
    public class ChildLookupItemCollectionViewModel<T>:CollectionViewModelBase where T:class ,IEntity
    {

        private ObservableCollection<T> _entities = new ObservableCollection<T>();

        private CancellationTokenSource _cancellationTokenSource;

        protected ChildLookupItemCollectionViewModel(IDialogService dialogService,
            IEventAggregator eventAggregator)
        {

            NewCommand = new DelegateCommand<object>(ExecuteNew, CanExecuteNew);
            EditCommand = new DelegateCommand<object>(ExecuteEdit, CanExecuteEdit);
            ViewCommand = new DelegateCommand<object>(ExecuteView, CanExecuteView);
            DeleteCommand = new DelegateCommand<object>(ExecuteDelete, CanExecuteDelete);
            RefreshCommand = new DelegateCommand(ExecuteRefresh, CanExecuteRefresh);

        }



        private bool CanExecuteDelete(object arg) => SelectedEntity != null && AuthorizedToDelete();

        protected virtual bool AuthorizedToDelete() =>
            //var typeName = typeof(TEntity).Name;
            //var entityName= typeName.Substring(0, typeName.IndexOf("Model", StringComparison.Ordinal)).Humanize();
            false;

        private void ExecuteDelete(object obj)
        {
            //var result = MessageBoxService.Show(
            //    string.Format(CommonResources.Confirmation_Delete, .Name),
            //    CommonResources.Confirmation_Caption,
            //    MessageButton.YesNo, MessageIcon.Warning, MessageResult.No);

            //if (result == MessageResult.Yes)
            //{
            //    Delete(SelectedEntity);
            //    Refresh();
            //}
        }

        public virtual bool Delete(T entity) => false;

        public ICommand DeleteCommand { get; set; }

        private bool CanExecuteRefresh() => !IsLoading;

        private void ExecuteRefresh() => Refresh();

        public DelegateCommand RefreshCommand { get; set; }

        protected virtual void Refresh() => LoadEntities(false);

        public virtual bool IsLoading { get; protected set; }
        protected bool IsLoaded { get; private set; }
        public ObservableCollection<T> Entities
        {
            get
            {
                if (!IsLoaded)
                    LoadEntities(false);
                return _entities;
            }
        }

        protected async void LoadEntities(bool forceLoad)
        {
            if (forceLoad)
            {
                _cancellationTokenSource?.Cancel();
            }
            else if (IsLoading)
            {
                return;
            }
            _cancellationTokenSource = await LoadCoreAsync();
        }



        private async Task<CancellationTokenSource> LoadCoreAsync()
        {
            IsLoading = true;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            try
            {
                List<T> items;

                    items = await FillEntities(cancellationTokenSource);

                _entities = new ObservableCollection<T>(items);
            }
            finally
            {
                IsLoading = false;
                IsLoaded = true;
            }
            return cancellationTokenSource;
        }

        /// <summary>
        /// Fills the entities.
        /// </summary>
        /// <param name="tokenSource">The token source.</param>
        /// <returns>Task&lt;List&lt;TEntityList&gt;&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Task<List<T>> FillEntities(CancellationTokenSource tokenSource) => Task.FromResult(new List<T>());



        public T SelectedEntity { get; set; }

        private bool CanExecuteEdit(object view) => SelectedEntity?.Id > 0 && AuthorizedToEdit();

        protected virtual bool AuthorizedToEdit() => false;

        private void ExecuteEdit(object view)
        {

        }

        private bool CanExecuteView(object view) => view != null && SelectedEntity?.Id > 0 && AuthorizedToView();

        protected virtual bool AuthorizedToView() => false;
        private void ExecuteView(object view)
        {

        }

        private bool CanExecuteNew(object view) => view != null && AuthorizedToAdd();

        protected virtual bool AuthorizedToAdd() => false;

        private void ExecuteNew(object view)
        {

        }

        #region Commands

        public ICommand NewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ViewCommand { get; }

        #endregion




    }

}