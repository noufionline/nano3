using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using Jasmine.Core.Aspects;
using Jasmine.Core.Common;
using Jasmine.Core.Contracts;
using Jasmine.Core.Events;
using Jasmine.Core.Helpers;
using Jasmine.Core.Properties;
using PostSharp.Patterns.Diagnostics;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using IDialogService = Prism.Services.Dialogs.IDialogService;

namespace Jasmine.Core.Mvvm
{
    public abstract class PagedCollectionViewModelBase<TEntity, TEntityList>:OnDemandCollectionViewModelBase<TEntityList>
        where TEntity : class, IEntity
        where TEntityList : class, IEntity
    {
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAuthorizationCache _authorizationCache;

        
        private readonly LogSource _logger;
        protected PagedCollectionViewModelBase(IDialogService dialogService,
            IEventAggregator eventAggregator, IAuthorizationCache authorizationCache) : base(eventAggregator, dialogService, authorizationCache)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _authorizationCache = authorizationCache;
            _logger = LogSource.Get();

            _eventAggregator.GetEvent<EntitySavedEvent>().Subscribe(Refresh);
            _eventAggregator.GetEvent<EntityDeletedEvent>().Subscribe(Refresh);

            NewCommand = new AsyncCommand<object>(ExecuteNew, CanExecuteNew);
            EditCommand = new AsyncCommand<object>(ExecuteEditAsync, CanExecuteEdit);
            ViewCommand = new AsyncCommand<object>(ExecuteViewAsync, CanExecuteView);
            DeleteCommand = new AsyncCommand<object>(ExecuteDelete, CanExecuteDelete);
            RefreshCommand = new DelegateCommand(ExecuteRefresh);

            ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);
            ExportToPdfCommand = new DelegateCommand<object>(ExportToPdf);
            Source = new PagedAsyncSource()
            {
                ElementType = typeof(TEntityList),
                // ReSharper disable once VirtualMemberCallInConstructor
                PageSize = PageSize,
                PageNavigationMode = PageNavigationMode.ArbitraryWithTotalPageCount
            };

            Source.FetchPage += OnFetchPages;

            

        }

        private void ExecuteRefresh() => Refresh();

        protected void Refresh()
        {
            Source.RefreshRows();
            Source.UpdateSummaries();
        }

        public DelegateCommand RefreshCommand { get; set; }

        private void OnFetchPages(object o, FetchPageAsyncEventArgs e)
        {
            e.Result = FetchPagesAsync(e);
        }

        private async Task<FetchRowsResult> FetchPagesAsync(FetchPageAsyncEventArgs e)
        {
          
            var page = e.Skip / e.Take;
            var pageSize = e.Take;

            var issues = await GetListAsync(page,pageSize,e.Filter,e.SortOrder);

            return new FetchRowsResult(issues.OfType<object>().ToArray(), hasMoreRows: issues.Length == PageSize);
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public PagedAsyncSource Source { get; }

    

        public DelegateCommand<object> ExportToPdfCommand { get; set; }

        public DelegateCommand<object> ExportToExcelCommand { get; }

        private void ExportToPdf(object tableView) => ExportDocument(ExportDocumentType.Pdf, tableView);

        public void ExportToExcel(object tableView) => ExportDocument(ExportDocumentType.Excel, tableView);

        private bool CanExecuteDelete(object arg) => this is ISupportDeletion && SelectedEntity != null && AuthorizedToDelete();



        protected virtual bool AuthorizedToDelete() => _authorizationCache.CheckAccess($"Delete{GetEntityName()}");

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
            }
        }

        public virtual Task<bool> DeleteAsync(TEntityList entity) => Task.FromResult(false);

        public ICommand DeleteCommand { get; set; }


        
        private bool CanExecuteSearch() => true;

        public virtual bool OnSearch()
        {
             ButtonResult result=ButtonResult.None;
            _dialogService.ShowDialog("FilterView",null,callback=> 
            {
                result=callback.Result;    
            });
            return result==ButtonResult.OK;
        }

     
        public virtual TEntityList SelectedEntity { get; set; }

        private bool CanExecuteEdit(object view) => SelectedEntity?.Id > 0 && AuthorizedToEdit();

        protected virtual bool AuthorizedToEdit() => _authorizationCache.CheckAccess($"Create{GetEntityName()}");

        [BackgroundTask(AspectPriority = 2)]
        private async Task ExecuteEditAsync(object view)
        {
            TEntity entity = await GetEntityAsync(SelectedEntity.Id);
            
            var parameters = new DialogParameters
            {
                {"entity", (entity, false)}, {"caption", Caption}, {"captionimage", CaptionImage}
            };

            await OnBeforeNavigation(parameters);

            Dictionary<string, object> lookupItems = await GetLookupItemsDictionaryAsync(entity);
            if (lookupItems.Count>0)
            {
                parameters.Add("lookupitems",lookupItems);
            }
            _dialogService.ShowDialog(view.ToString(),parameters);

        }

        protected virtual  Task OnBeforeNavigation(NavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        protected virtual Task<LookupItemDictionary> GetLookupItemsDictionaryAsync(TEntity entity)
        {
            return Task.FromResult(new LookupItemDictionary());
        }




        protected abstract Task<TEntity> GetEntityAsync(int id);

        private bool CanExecuteView(object view) => view != null && SelectedEntity?.Id > 0 && AuthorizedToView();

        protected virtual bool AuthorizedToView() => _authorizationCache.CheckAccess($"View{GetEntityName()}");
        //private void ExecuteView(object view) => _dialogService.ShowDialog(view.ToString(), new NavigationParameters { { "entity", (SelectedEntity.Id, true) } });

        [BackgroundTask(AspectPriority = 2)]
        private async Task ExecuteViewAsync(object view)
        {
            TEntity entity = await GetEntityAsync(SelectedEntity.Id);

            var parameters = new DialogParameters
                {{"entity", (entity, true)}, {"caption", Caption}, {"captionimage", CaptionImage}};
           
            await OnBeforeNavigation(parameters);

            Dictionary<string, object> lookupItems = await GetLookupItemsDictionaryAsync(entity);
            if (lookupItems.Count>0)
            {
                parameters.Add("lookupitems",lookupItems);
            }

            _dialogService.ShowDialog(view.ToString(),parameters);


        }

        private bool CanExecuteNew(object view) => view != null && AuthorizedToAdd();

        protected virtual bool AuthorizedToAdd() => _authorizationCache.CheckAccess($"Create{GetEntityName()}");

        [BackgroundTask(AspectPriority = 2)]
        private async Task ExecuteNew(object view)
        {
            var parameters = new DialogParameters { { "caption", Caption }, { "captionimage", CaptionImage } };
          
            await OnBeforeNavigation(parameters);
           
            _dialogService.ShowDialog(view.ToString(), parameters);
        }


        //protected virtual IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
        #region Commands

        public AsyncCommand<object> NewCommand { get; }
        public AsyncCommand<object> EditCommand { get; }
        public AsyncCommand<object> ViewCommand { get; }

        #endregion

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


        public override bool KeepAlive => true;

        public virtual string GetEntityName()
        {
            string typeName = typeof(TEntity).Name;
            var value=typeName.IndexOf("Model", StringComparison.Ordinal);
            var length = value >= 0 ? value : typeName.Length;
            string entityName = typeName.Substring(0,length);//.Humanize();
            return entityName;
        }
    }
}