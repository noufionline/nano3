using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using Jasmine.Core.Aspects;
using Jasmine.Core.Contracts;
using Jasmine.Core.Events;
using Jasmine.Core.Properties;
using PostSharp.Patterns.Diagnostics;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Jasmine.Core.Common;
using Prism.Services.Dialogs;
using IDialogService = Prism.Services.Dialogs.IDialogService;
using Jasmine.Core.Helpers;
using DevExpress.Diagram.Core;

namespace Jasmine.Core.Mvvm
{

    //public abstract class CollectionListViewModel<TEntity> : CollectionListViewModel<TEntity, TEntity>
    //    where TEntity : class, IEntity
    //{
    //    private readonly IDialogService _dialogService;
    //    private readonly IEventAggregator _eventAggregator;

    //    protected CollectionListViewModel(IDialogService dialogService, IEventAggregator eventAggregator,
    //        IAuthorizationCache authorizationCache) : base(dialogService, eventAggregator, authorizationCache)
    //    {
    //        _dialogService = dialogService;
    //        _eventAggregator = eventAggregator;
    //    }
    //}

    //public abstract class NewNavigationAwareCollectionListViewModel<TEntityList> : AsyncViewModelBase
    //    where TEntityList : class, IEntity
    //{
    //    private readonly IAuthorizationCache _authorizationCache;

    //    private CancellationTokenSource _cancellationTokenSource;
    //    private readonly IDialogService _dialogService;


    //    private ObservableCollection<TEntityList> _entities = new ObservableCollection<TEntityList>();
    //    private readonly LogSource _logger;
    //    readonly IRegionManager _regionManager;

    //    public NewNavigationAwareCollectionListViewModel(IDialogService dialogService,
    //        IEventAggregator eventAggregator, IAuthorizationCache authorizationCache, IRegionManager regionManager) : base(eventAggregator, dialogService, authorizationCache)
    //    {
    //        _regionManager = regionManager;
    //        _dialogService = dialogService;

    //        _authorizationCache = authorizationCache;
    //        _logger = LogSource.Get();



    //        eventAggregator.GetEvent<NewEntitySavedEvent>().Subscribe(RefreshList);
    //        eventAggregator.GetEvent<NewEntityDeletedEvent>().Subscribe(RefreshList);

    //        NewCommand = new DelegateCommand<object>(ExecuteNew, CanExecuteNew);
    //        EditCommand = new DelegateCommand<object>(ExecuteEdit, CanExecuteEdit);
    //        ViewCommand = new DelegateCommand<object>(ExecuteView, CanExecuteView);
    //        DeleteCommand = new AsyncCommand<object>(ExecuteDelete, CanExecuteDelete);
    //        SearchCommand = new DelegateCommand(ExecuteSearch, CanExecuteSearch);
    //        RefreshCommand = new DelegateCommand(ExecuteRefresh, CanExecuteRefresh);

    //        ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);
    //        ExportToPdfCommand = new DelegateCommand<object>(ExportToPdf);
    //    }

    //    private bool CanExecuteDelete(object arg) => this is ISupportDeletion && SelectedEntity != null && AuthorizedToDelete();

    //    private bool CanExecuteEdit(object view) => SelectedEntity?.Id > 0 && AuthorizedToEdit();

    //    private bool CanExecuteNew(object view) => view != null && AuthorizedToAdd();

    //    private bool CanExecuteRefresh() => !IsLoading;
    //    private bool CanExecuteSearch() => true;

    //    private bool CanExecuteView(object view) => view != null && SelectedEntity?.Id > 0 && AuthorizedToView();

    //    [ShowException(AspectPriority = 2)]
    //    private async Task ExecuteDelete(object obj)
    //    {
    //        MessageResult result = MessageBoxService.Show(
    //            string.Format(CommonResources.Confirmation_Delete, typeof(TEntityList).Name),
    //            CommonResources.Confirmation_Caption,
    //            MessageButton.YesNo, MessageIcon.Warning, MessageResult.No);

    //        if (result == MessageResult.Yes)
    //        {
    //            await DeleteAsync(SelectedEntity);
    //            Refresh();
    //        }
    //    }

    //    private void ExecuteEdit(object view)
    //    {
    //        try
    //        {
    //            var parameters = new DialogParameters
    //            {
    //                {"entity", (SelectedEntity.Id, false)}, {"caption", Caption}, {"captionimage", CaptionImage}, {"collectionViewType", this.GetType().FullName}
    //            };

    //            _regionManager.RequestNavigate(KnownRegions.DocumentRegion, view.ToString(), parameters);
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBoxService.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    //        }

    //    }


    //    private void ExecuteNew(object view)
    //    {
    //        var parameters = new DialogParameters { { "caption", Caption }, { "captionimage", CaptionImage }, { "collectionViewType", this.GetType().FullName } };

    //        _regionManager.RequestNavigate(KnownRegions.DocumentRegion, view.ToString(), parameters);
    //    }
    //    [BackgroundTask(AspectPriority = 2)]
    //    private void ExecuteRefresh() => Refresh();

    //    [ShowException(AspectPriority = 2)]
    //    private void ExecuteSearch()
    //    {
    //        if (OnSearch())
    //        {
    //            IsLoaded = false;
    //            LoadEntities(false);
    //        }
    //    }
    //    //private void ExecuteView(object view) => _dialogService.ShowDialog(view.ToString(), new NavigationParameters { { "entity", (SelectedEntity.Id, true) } });

    //    private void ExecuteView(object view)
    //    {
    //        try
    //        {
    //            var parameters = new DialogParameters
    //                {{"entity", (SelectedEntity.Id, true)}, {"caption", Caption}, {"captionimage", CaptionImage}};

    //            _regionManager.RequestNavigate(KnownRegions.DocumentRegion, view.ToString(), parameters);
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBoxService.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    //        }
    //    }
    //    private void ExportDocument(ExportDocumentType documentType, object tableView)
    //    {

    //        SaveFileDialogService.Filter = documentType == ExportDocumentType.Pdf ? "PDF Files (*.pdf)|*.pdf" : "Excel Files (*.xlsx)|*.xlsx";
    //        if (SaveFileDialogService.ShowDialog())
    //        {
    //            string path = $"{SaveFileDialogService.File.DirectoryName}\\{SaveFileDialogService.File.Name}";
    //            if (documentType == ExportDocumentType.Pdf) ((TableView)tableView).ExportToPdf(path);
    //            else ((TableView)tableView).ExportToXlsx(path, new XlsxExportOptions(TextExportMode.Value));
    //            MessageResult warningResult = MessageBoxService.ShowMessage(CommonResources.File_Open_Message, CommonResources.File_Open_Caption, MessageButton.OKCancel);
    //            if (warningResult == MessageResult.OK)
    //                Process.Start(path);
    //        }
    //    }

    //    private void ExportToPdf(object tableView) => ExportDocument(ExportDocumentType.Pdf, tableView);

    //    private async Task<CancellationTokenSource> LoadCoreAsync()
    //    {
    //        IsLoading = true;
    //        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    //        try
    //        {
    //            await FillLookupItemsAsync();
    //            List<TEntityList> items;
    //            if (IsSearchable)
    //            {
    //                items = await ApplyFilterAsync(cancellationTokenSource);
    //            }
    //            else
    //            {
    //                items = await FillEntitiesAsync(cancellationTokenSource);
    //            }

    //            _entities = new ObservableCollection<TEntityList>(items);
    //        }
    //        finally
    //        {
    //            IsLoading = false;
    //            IsLoaded = true;
    //        }
    //        return cancellationTokenSource;
    //    }

    //    protected virtual bool AuthorizedToAdd() => _authorizationCache.CheckAccess($"Create{GetEntityName()}");



    //    protected virtual bool AuthorizedToDelete() => _authorizationCache.CheckAccess($"Delete{GetEntityName()}");

    //    protected virtual bool AuthorizedToEdit() => _authorizationCache.CheckAccess($"Create{GetEntityName()}");

    //    protected virtual bool AuthorizedToView() => _authorizationCache.CheckAccess($"View{GetEntityName()}");

    //    protected virtual Task FillLookupItemsAsync()
    //    {
    //        return Task.CompletedTask;
    //    }

    //    protected async void LoadEntities(bool forceLoad)
    //    {
    //        if (forceLoad)
    //        {
    //            _cancellationTokenSource?.Cancel();
    //        }
    //        else if (IsLoading)
    //        {
    //            return;
    //        }
    //        _cancellationTokenSource = await LoadCoreAsync();
    //    }

    //    protected virtual void Refresh()
    //    {
    //        if (IsActive)
    //        {
    //            LoadEntities(false);
    //        }
    //    }
    //    protected void RefreshList(string viewType)
    //    {
    //        if (viewType == this.GetType().FullName)
    //        {
    //            LoadEntities(false);
    //        }
    //    }

    //    protected virtual void SetEntities(ObservableCollection<TEntityList> items)
    //    {
    //        _entities = items;
    //    }

    //    protected bool IsLoaded { get; private set; }

    //    public virtual Task<List<TEntityList>> ApplyFilterAsync(CancellationTokenSource cancellationTokenSource) => Task.FromResult(new List<TEntityList>());

    //    public virtual Task<bool> DeleteAsync(TEntityList entity) => Task.FromResult(false);

    //    public void ExportToExcel(object tableView) => ExportDocument(ExportDocumentType.Excel, tableView);

    //    /// <summary>
    //    /// Fills the entities.
    //    /// </summary>
    //    /// <param name="tokenSource">The token source.</param>
    //    /// <returns>Task&lt;List&lt;TEntityList&gt;&gt;.</returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public virtual Task<List<TEntityList>> FillEntitiesAsync(CancellationTokenSource tokenSource) => Task.FromResult(new List<TEntityList>());

    //    public virtual string GetEntityName()
    //    {
    //        string typeName = typeof(TEntityList).Name;
    //        var value = typeName.EndsWith("List") ? typeName.IndexOf("List", StringComparison.Ordinal) : typeName.IndexOf("Model", StringComparison.Ordinal);
    //        var length = value >= 0 ? value : typeName.Length;
    //        string entityName = typeName.Substring(0, length);//.Humanize();
    //        return entityName;
    //    }

    //    public virtual bool OnSearch()
    //    {
    //        bool? result = _dialogService.ShowDialog("FilterView");
    //        return result != null && result.Value;
    //    }

    //    public ICommand DeleteCommand { get; set; }
    //    public ObservableCollection<TEntityList> Entities
    //    {
    //        get
    //        {
    //            if (!IsLoaded)
    //                LoadEntities(false);
    //            return _entities;
    //        }
    //    }

    //    public DelegateCommand<object> ExportToExcelCommand { get; }

    //    public DelegateCommand<object> ExportToPdfCommand { get; set; }

    //    public virtual bool IsLoading { get; protected set; }

    //    public bool IsSearchable => this is ISearchable;


    //    public override bool KeepAlive => true;

    //    public DelegateCommand RefreshCommand { get; set; }

    //    public ICommand SearchCommand { get; }

    //    public virtual TEntityList SelectedEntity { get; set; }


    //    //protected virtual IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
    //    #region Commands

    //    public DelegateCommand<object> NewCommand { get; }
    //    public DelegateCommand<object> EditCommand { get; }
    //    public DelegateCommand<object> ViewCommand { get; }

    //    #endregion

    //}

    public abstract class CollectionListViewModel<TEntityList> : AsyncViewModelBase
        where TEntityList : class, IEntity
    {
        private readonly IAuthorizationCache _authorizationCache;

        private CancellationTokenSource _cancellationTokenSource;
        private readonly IDialogService _dialogService;


        private ObservableCollection<TEntityList> _entities = new ObservableCollection<TEntityList>();
        private readonly LogSource _logger;
        private readonly IRegionManager _regionManager;

        protected CollectionListViewModel(IDialogService dialogService,
            IRegionManager regionManager,
            IEventAggregator eventAggregator, IAuthorizationCache authorizationCache) : base(eventAggregator, dialogService, authorizationCache)
        {
            _dialogService = dialogService;
            _regionManager = regionManager;

            _authorizationCache = authorizationCache;
            _logger = LogSource.Get();



            eventAggregator.GetEvent<NewEntitySavedEvent>().Subscribe(RefreshList);
            eventAggregator.GetEvent<NewEntityDeletedEvent>().Subscribe(RefreshList);

            NewCommand = new DelegateCommand<object>(ExecuteNew, CanExecuteNew);
            EditCommand = new DelegateCommand<object>(ExecuteEdit, CanExecuteEdit);
            ViewCommand = new DelegateCommand<object>(ExecuteView, CanExecuteView);
            DeleteCommand = new AsyncCommand<object>(ExecuteDelete, CanExecuteDelete);
            SearchCommand = new DelegateCommand(ExecuteSearch, CanExecuteSearch);
            RefreshCommand = new DelegateCommand(ExecuteRefresh, CanExecuteRefresh);

            ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);
            ExportToPdfCommand = new DelegateCommand<object>(ExportToPdf);
        }

        private bool CanExecuteDelete(object arg) => this is ISupportDeletion && SelectedEntity != null && AuthorizedToDelete();

        private bool CanExecuteEdit(object view) => SelectedEntity?.Id > 0 && AuthorizedToEdit();



        private bool CanExecuteNew(object view) => view != null && AuthorizedToAdd();

        private bool CanExecuteRefresh() => !IsLoading;
        private bool CanExecuteSearch() => true;


        private bool CanExecuteView(object view) => view != null && SelectedEntity?.Id > 0 && AuthorizedToView();

        [ShowException(AspectPriority = 2)]
        private async Task ExecuteDelete(object obj)
        {
            MessageResult result = MessageBoxService.Show(
                string.Format(CommonResources.Confirmation_Delete, typeof(TEntityList).Name),
                CommonResources.Confirmation_Caption,
                MessageButton.YesNo, MessageIcon.Warning, MessageResult.No);

            if (result == MessageResult.Yes)
            {
                await DeleteAsync(SelectedEntity);
                Refresh();
            }
        }

        private void ExecuteEdit(object view)
        {
            try
            {
                var parameters = new DialogParameters
                {
                    {"entity", (SelectedEntity.Id, false)}, {"caption", Caption}, {"captionimage", CaptionImage}, { "collectionViewType", this.GetType().FullName}
                };

                OnExecuteEditCommand(view, parameters);
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnExecuteEditCommand(object view, DialogParameters parameters)
        {
            var viewOptions = ConfigureViewOptions();
            if (viewOptions == null)
            {
                throw new ArgumentNullException(nameof(viewOptions), @"View Option must be specified and cannot be null");
            }
            switch (viewOptions.EditViewType)
            {
                case ChildViewType.PopupView:
                    _dialogService.ShowDialog(view.ToString(), parameters);
                    break;
                case ChildViewType.TabView:
                    _regionManager.RequestNavigate(KnownRegions.DocumentRegion, view.ToString(), parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExecuteNew(object view)
        {
            var parameters = new DialogParameters { { "caption", Caption }, { "captionimage", CaptionImage }, { "collectionViewType", this.GetType().FullName } };
            OnExecuteNewCommand(view,parameters);
        }


        protected virtual void OnExecuteNewCommand(object view,DialogParameters parameters)
        {
            var viewOptions = ConfigureViewOptions();
            if (viewOptions == null)
            {
                throw new ArgumentNullException(nameof(viewOptions),@"View Option must be specified and cannot be null");
            }
           
            switch (viewOptions.NewViewType)
            {
                case ChildViewType.PopupView:
                    _dialogService.ShowDialog(view.ToString(), parameters);
                    break;
                case ChildViewType.TabView:
                    _regionManager.RequestNavigate(KnownRegions.DocumentRegion, view.ToString(), parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

       
        [BackgroundTask(AspectPriority = 2)]
        private void ExecuteRefresh() => Refresh();

        [ShowException(AspectPriority = 2)]
        private void ExecuteSearch()
        {
            if (OnSearch())
            {
                IsLoaded = false;
                LoadEntities(false);
            }
        }
        //private void ExecuteView(object view) => _dialogService.ShowDialog(view.ToString(), new NavigationParameters { { "entity", (SelectedEntity.Id, true) } });

        private void ExecuteView(object view)
        {
            try
            {
                var parameters = new DialogParameters
                    {{"entity", (SelectedEntity.Id, true)}, {"caption", Caption}, {"captionimage", CaptionImage}};

                OnExecuteViewCommand(view, parameters);
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnExecuteViewCommand(object view, DialogParameters parameters)
        {
            var viewOptions = ConfigureViewOptions();
            if (viewOptions == null)
            {
                throw new ArgumentNullException(nameof(viewOptions), @"View Option must be specified and cannot be null");
            }

            switch (viewOptions.ReadOnlyViewType)
            {
                case ChildViewType.PopupView:
                    _dialogService.ShowDialog(view.ToString(), parameters);
                    break;
                case ChildViewType.TabView:
                    _regionManager.RequestNavigate(KnownRegions.DocumentRegion, view.ToString(), parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExportDocument(ExportDocumentType documentType, object tableView)
        {

            SaveFileDialogService.Filter = documentType == ExportDocumentType.Pdf ? "PDF Files (*.pdf)|*.pdf" : "Excel Files (*.xlsx)|*.xlsx";
            if (SaveFileDialogService.ShowDialog())
            {
                string path = $"{SaveFileDialogService.File.DirectoryName}\\{SaveFileDialogService.File.Name}";
                if (documentType == ExportDocumentType.Pdf) ((TableView)tableView).ExportToPdf(path);
                else ((TableView)tableView).ExportToXlsx(path, new XlsxExportOptions(TextExportMode.Value));
                MessageResult warningResult = MessageBoxService.ShowMessage(CommonResources.File_Open_Message, CommonResources.File_Open_Caption, MessageButton.OKCancel);
                if (warningResult == MessageResult.OK)
                    Process.Start(path);
            }
        }

        private void ExportToPdf(object tableView) => ExportDocument(ExportDocumentType.Pdf, tableView);

        private async Task<CancellationTokenSource> LoadCoreAsync()
        {
            IsLoading = true;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            try
            {
                List<TEntityList> items;
                if (IsSearchable)
                {
                    items = await ApplyFilterAsync(cancellationTokenSource);
                }
                else
                {
                    items = await FillEntitiesAsync(cancellationTokenSource);
                }

                _entities = new ObservableCollection<TEntityList>(items);
            }
            finally
            {
                IsLoading = false;
                IsLoaded = true;
            }
            return cancellationTokenSource;
        }

        protected virtual bool AuthorizedToAdd() => _authorizationCache.CheckAccess($"Create{GetEntityName()}");



        protected virtual bool AuthorizedToDelete() => _authorizationCache.CheckAccess($"Delete{GetEntityName()}");

        protected virtual bool AuthorizedToEdit() => _authorizationCache.CheckAccess($"Create{GetEntityName()}");

        protected virtual bool AuthorizedToView() => _authorizationCache.CheckAccess($"View{GetEntityName()}");


        protected abstract ViewOptions ConfigureViewOptions();

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

        protected virtual void Refresh()
        {
            if (IsActive)
            {
                LoadEntities(false);
            }
        }

        protected void RefreshList(string viewType)
        {
            if (viewType == this.GetType().FullName)
            {
                LoadEntities(false);
            }
        }

        protected virtual void SetEntities(ObservableCollection<TEntityList> items)
        {
            _entities = items;
        }

        protected bool IsLoaded { get; private set; }

        public virtual Task<List<TEntityList>> ApplyFilterAsync(CancellationTokenSource cancellationTokenSource) => Task.FromResult(new List<TEntityList>());

        public virtual Task<bool> DeleteAsync(TEntityList entity) => Task.FromResult(false);

        public void ExportToExcel(object tableView) => ExportDocument(ExportDocumentType.Excel, tableView);

        /// <summary>
        /// Fills the entities.
        /// </summary>
        /// <param name="tokenSource">The token source.</param>
        /// <returns>Task&lt;List&lt;TEntityList&gt;&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Task<List<TEntityList>> FillEntitiesAsync(CancellationTokenSource tokenSource) => Task.FromResult(new List<TEntityList>());

        public virtual string GetEntityName()
        {
            string typeName = typeof(TEntityList).Name;
            var value = typeName.EndsWith("List") ? typeName.IndexOf("List", StringComparison.Ordinal) : typeName.IndexOf("Model", StringComparison.Ordinal);
            
            var length = value >= 0 ? value : typeName.Length;
            string entityName = typeName.Substring(0, length);//.Humanize();

            //Debug.WriteLine($"Entity Name :{entityName}");
            return entityName;
        }

        public virtual bool OnSearch()
        {
            ButtonResult result=ButtonResult.None;
            _dialogService.ShowDialog("FilterView",null,callback=> 
            {
                result=callback.Result;                
            });
            return result==ButtonResult.OK;
        }

        public ICommand DeleteCommand { get; set; }
        public ObservableCollection<TEntityList> Entities
        {
            get
            {
                if (!IsLoaded)
                    LoadEntities(false);
                return _entities;
            }
        }

        public DelegateCommand<object> ExportToExcelCommand { get; }

        public DelegateCommand<object> ExportToPdfCommand { get; set; }

        public virtual bool IsLoading { get; protected set; }

        public bool IsSearchable => this is ISearchable;


        public override bool KeepAlive => true;

        public DelegateCommand RefreshCommand { get; set; }

        public ICommand SearchCommand { get; }

        public virtual TEntityList SelectedEntity { get; set; }

        //protected virtual IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
        #region Commands

        public DelegateCommand<object> NewCommand { get; }
        public DelegateCommand<object> EditCommand { get; }
        public DelegateCommand<object> ViewCommand { get; }

        #endregion

    }

    public class ViewOptions
    {
        public ViewOptions(
            ChildViewType newViewType = ChildViewType.PopupView,
            ChildViewType editViewType = ChildViewType.PopupView,
            ChildViewType readOnlyViewType = ChildViewType.PopupView)
        {
            NewViewType = newViewType;
            EditViewType = editViewType;
            ReadOnlyViewType = readOnlyViewType;

        }

        public static ViewOptions TabView => new ViewOptions(ChildViewType.TabView, ChildViewType.TabView, ChildViewType.TabView);

        public static ViewOptions PopupView => new ViewOptions(ChildViewType.PopupView, ChildViewType.PopupView, ChildViewType.PopupView);
       
        public ChildViewType EditViewType { get; }

        public ChildViewType NewViewType { get; }
        public ChildViewType ReadOnlyViewType { get; }
        public string ViewName { get; set; }
    }

    public abstract class CollectionListViewModel<TEntity, TEntityList> : AsyncViewModelBase
        where TEntity : class, IEntity
        where TEntityList : class, IEntity
    {
        private readonly IAuthorizationCache _authorizationCache;

        private CancellationTokenSource _cancellationTokenSource;
        private readonly IDialogService _dialogService;


        private ObservableCollection<TEntityList> _entities = new ObservableCollection<TEntityList>();
        private readonly LogSource _logger;

        protected CollectionListViewModel(IDialogService dialogService,
            IEventAggregator eventAggregator, IAuthorizationCache authorizationCache) : base(eventAggregator, dialogService, authorizationCache)
        {
            _dialogService = dialogService;

            _authorizationCache = authorizationCache;
            _logger = LogSource.Get();



            eventAggregator.GetEvent<EntitySavedEvent>().Subscribe(Refresh);
            eventAggregator.GetEvent<EntityDeletedEvent>().Subscribe(Refresh);

            NewCommand = new DelegateCommand<object>(ExecuteNew, CanExecuteNew);
            EditCommand = new DelegateCommand<object>(ExecuteEdit, CanExecuteEdit);
            ViewCommand = new DelegateCommand<object>(ExecuteViewAsync, CanExecuteView);
            DeleteCommand = new AsyncCommand<object>(ExecuteDelete, CanExecuteDelete);
            SearchCommand = new DelegateCommand(ExecuteSearch, CanExecuteSearch);
            RefreshCommand = new DelegateCommand(ExecuteRefresh, CanExecuteRefresh);

            ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);
            ExportToPdfCommand = new DelegateCommand<object>(ExportToPdf);
        }

        private bool CanExecuteDelete(object arg) => this is ISupportDeletion && SelectedEntity != null && AuthorizedToDelete();

        private bool CanExecuteEdit(object view) => SelectedEntity?.Id > 0 && AuthorizedToEdit();

        private bool CanExecuteNew(object view) => view != null && AuthorizedToAdd();

        private bool CanExecuteRefresh() => !IsLoading;
        private bool CanExecuteSearch() => true;

        private bool CanExecuteView(object view) => view != null && SelectedEntity?.Id > 0 && AuthorizedToView();

        [ShowException(AspectPriority = 2)]
        private async Task ExecuteDelete(object obj)
        {
            MessageResult result = MessageBoxService.Show(
                string.Format(CommonResources.Confirmation_Delete, typeof(TEntity).Name),
                CommonResources.Confirmation_Caption,
                MessageButton.YesNo, MessageIcon.Warning, MessageResult.No);

            if (result == MessageResult.Yes)
            {
                await DeleteAsync(SelectedEntity);
                Refresh();
            }
        }

        [BackgroundTask(AspectPriority = 2)]
        private async void ExecuteEdit(object view)
        {
            try
            {
                TEntity entity = await GetEntityAsync(SelectedEntity.Id);

                var parameters = new DialogParameters
                {
                    {"entity", (entity, false)}, {"caption", Caption}, {"captionimage", CaptionImage}
                };

                await OnBeforeNavigation(parameters);

                var lookupItems = await GetLookupItemsDictionaryAsync(entity);
                if (lookupItems.Count > 0)
                {
                    parameters.Add("lookupitems", lookupItems);
                }
                _dialogService.ShowDialog(view.ToString(), parameters);
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private async void ExecuteNew(object view)
        {
            var parameters = new DialogParameters { { "caption", Caption }, { "captionimage", CaptionImage } };

            await OnBeforeNavigation(parameters);

            _dialogService.ShowDialog(view.ToString(), parameters);
        }
        [BackgroundTask(AspectPriority = 2)]
        private void ExecuteRefresh() => Refresh();

        [ShowException(AspectPriority = 2)]
        private void ExecuteSearch()
        {
            if (OnSearch())
            {
                IsLoaded = false;
                LoadEntities(false);
            }
        }
        //private void ExecuteView(object view) => _dialogService.ShowDialog(view.ToString(), new NavigationParameters { { "entity", (SelectedEntity.Id, true) } });

        [BackgroundTask(AspectPriority = 2)]
        private async void ExecuteViewAsync(object view)
        {
            try
            {
                TEntity entity = await GetEntityAsync(SelectedEntity.Id);

                var parameters = new DialogParameters
                    {{"entity", (entity, true)}, {"caption", Caption}, {"captionimage", CaptionImage}};

                await OnBeforeNavigation(parameters);

                Dictionary<string, object> lookupItems = await GetLookupItemsDictionaryAsync(entity);
                if (lookupItems.Count > 0)
                {
                    parameters.Add("lookupitems", lookupItems);
                }

                _dialogService.ShowDialog(view.ToString(), parameters);
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExportDocument(ExportDocumentType documentType, object tableView)
        {

            SaveFileDialogService.Filter = documentType == ExportDocumentType.Pdf ? "PDF Files (*.pdf)|*.pdf" : "Excel Files (*.xlsx)|*.xlsx";
            if (SaveFileDialogService.ShowDialog())
            {
                string path = $"{SaveFileDialogService.File.DirectoryName}\\{SaveFileDialogService.File.Name}";
                if (documentType == ExportDocumentType.Pdf) ((TableView)tableView).ExportToPdf(path);
                else ((TableView)tableView).ExportToXlsx(path, new XlsxExportOptions(TextExportMode.Value));
                MessageResult warningResult = MessageBoxService.ShowMessage(CommonResources.File_Open_Message, CommonResources.File_Open_Caption, MessageButton.OKCancel);
                if (warningResult == MessageResult.OK)
                    Process.Start(path);
            }
        }

        private void ExportToPdf(object tableView) => ExportDocument(ExportDocumentType.Pdf, tableView);

        private async Task<CancellationTokenSource> LoadCoreAsync()
        {
            IsLoading = true;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            try
            {
                List<TEntityList> items;
                if (IsSearchable)
                {
                    items = await ApplyFilterAsync(cancellationTokenSource);
                }
                else
                {
                    items = await FillEntitiesAsync(cancellationTokenSource);
                }

                _entities = new ObservableCollection<TEntityList>(items);
            }
            finally
            {
                IsLoading = false;
                IsLoaded = true;
            }
            return cancellationTokenSource;
        }

        protected virtual bool AuthorizedToAdd() => _authorizationCache.CheckAccess($"Create{GetEntityName()}");



        protected virtual bool AuthorizedToDelete() => _authorizationCache.CheckAccess($"Delete{GetEntityName()}");

        protected virtual bool AuthorizedToEdit() => _authorizationCache.CheckAccess($"Create{GetEntityName()}");

        protected virtual bool AuthorizedToView() => _authorizationCache.CheckAccess($"View{GetEntityName()}");




        protected abstract Task<TEntity> GetEntityAsync(int id);

        protected virtual Task<LookupItemDictionary> GetLookupItemsDictionaryAsync(TEntity entity)
        {
            return Task.FromResult(new LookupItemDictionary());
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

        protected virtual Task OnBeforeNavigation(NavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        protected virtual void Refresh()
        {
            if (IsActive)
            {
                LoadEntities(false);
            }
        }

        protected virtual void SetEntities(ObservableCollection<TEntityList> items)
        {
            _entities = items;
        }

        protected bool IsLoaded { get; private set; }

        public virtual Task<List<TEntityList>> ApplyFilterAsync(CancellationTokenSource cancellationTokenSource) => Task.FromResult(new List<TEntityList>());

        public virtual Task<bool> DeleteAsync(TEntityList entity) => Task.FromResult(false);

        public void ExportToExcel(object tableView) => ExportDocument(ExportDocumentType.Excel, tableView);

        /// <summary>
        /// Fills the entities.
        /// </summary>
        /// <param name="tokenSource">The token source.</param>
        /// <returns>Task&lt;List&lt;TEntityList&gt;&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Task<List<TEntityList>> FillEntitiesAsync(CancellationTokenSource tokenSource) => Task.FromResult(new List<TEntityList>());

        public virtual string GetEntityName()
        {
            string typeName = typeof(TEntity).Name;
            var value = typeName.EndsWith("List") ? typeName.IndexOf("List", StringComparison.Ordinal) : typeName.IndexOf("Model", StringComparison.Ordinal);
            var length = value >= 0 ? value : typeName.Length;
            string entityName = typeName.Substring(0, length);//.Humanize();
            return entityName;
        }

        public virtual bool OnSearch()
        {
            ButtonResult result=ButtonResult.None;
            _dialogService.ShowDialog("FilterView",null,dialogResult=>
            {
                result = dialogResult.Result;
            });
            return result==ButtonResult.OK;
        }

        public ICommand DeleteCommand { get; set; }
        public ObservableCollection<TEntityList> Entities
        {
            get
            {
                if (!IsLoaded)
                    LoadEntities(false);
                return _entities;
            }
        }

        public DelegateCommand<object> ExportToExcelCommand { get; }

        public DelegateCommand<object> ExportToPdfCommand { get; set; }

        public virtual bool IsLoading { get; protected set; }

        public bool IsSearchable => this is ISearchable;


        public override bool KeepAlive => true;

        public DelegateCommand RefreshCommand { get; set; }

        public ICommand SearchCommand { get; }

        public virtual TEntityList SelectedEntity { get; set; }


        //protected virtual IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
        #region Commands

        public DelegateCommand<object> NewCommand { get; }
        public DelegateCommand<object> EditCommand { get; }
        public DelegateCommand<object> ViewCommand { get; }

        #endregion

    }

    public enum ExportDocumentType
    {
        Excel, Pdf
    }

    public enum ChildViewType
    {
        PopupView,
        TabView
    }

}

