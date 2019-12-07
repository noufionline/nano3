using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using Humanizer;
using Jasmine.Core.Aspects;
using Jasmine.Core.Contracts;
using Jasmine.Core.Events;
using Jasmine.Core.Properties;
using PostSharp.Patterns.Diagnostics;
using Prism.Events;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Services.Dialogs;
using IDialogService = Prism.Services.Dialogs.IDialogService;
using Jasmine.Core.Helpers;

namespace Jasmine.Core.Mvvm
{
    public abstract class ODataCollectionViewModelBase<TEntity> : AsyncViewModelBase where TEntity : class, IEntity
    {
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAuthorizationCache _authCache;


        private readonly LogSource _logger;
        protected ODataCollectionViewModelBase(IDialogService dialogService,
            IEventAggregator eventAggregator, IAuthorizationCache authCache) : base(eventAggregator, dialogService, authCache)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _authCache = authCache;

            _logger = LogSource.Get();



            _eventAggregator.GetEvent<EntitySavedEvent>().Subscribe(Refresh);
            _eventAggregator.GetEvent<EntityDeletedEvent>().Subscribe(Refresh);

            NewCommand = new DelegateCommand<object>(ExecuteNew, CanExecuteNew);
            EditCommand = new AsyncCommand<object>(ExecuteEditAsync, CanExecuteEdit);
            ViewCommand = new AsyncCommand<object>(ExecuteViewAsync, CanExecuteView);
            DeleteCommand = new AsyncCommand<object>(ExecuteDelete, CanExecuteDelete);

            RefreshCommand = new DelegateCommand(ExecuteRefresh, CanExecuteRefresh);

            ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);
            ExportToPdfCommand = new DelegateCommand<object>(ExportToPdf);
        }

        public DelegateCommand<object> ExportToPdfCommand { get; set; }

        public DelegateCommand<object> ExportToExcelCommand { get; }

        private void ExportToPdf(object tableView) => ExportDocument(ExportDocumentType.Pdf, tableView);

        public void ExportToExcel(object tableView) => ExportDocument(ExportDocumentType.Excel, tableView);

        private bool CanExecuteDelete(object arg) => this is ISupportDeletion && SelectedEntity != null && AuthorizedToDelete();



        protected virtual bool AuthorizedToDelete() => _authCache.CheckAccess($"Delete {GetEntityName()}");

        [ShowException(AspectPriority = 2)]
        private async Task ExecuteDelete(object obj)
        {
            MessageResult result = MessageBoxService.Show(
                string.Format(CommonResources.Confirmation_Delete, typeof(TEntity).Name),
                CommonResources.Confirmation_Caption,
                MessageButton.YesNo, MessageIcon.Warning, MessageResult.No);

            if (result == MessageResult.Yes)
            {
                var id = GetId(obj);
                if (id > 0)
                {
                    await DeleteAsync(id);
                    Refresh();
                }
            }
        }

        public virtual Task<bool> DeleteAsync(int id) => Task.FromResult(false);

        public ICommand DeleteCommand { get; set; }

        private bool CanExecuteRefresh() => !IsLoading;
        // [BackgroundTask(AspectPriority = 2)]
        private void ExecuteRefresh() => Refresh();

        public DelegateCommand RefreshCommand { get; set; }

        protected virtual void Refresh()
        {

        }

        public virtual bool IsLoading { get; protected set; }
        protected bool IsLoaded { get; private set; }




        public bool IsSearchable => this is ISearchable;



        public virtual bool OnSearch()
        {
             ButtonResult result=ButtonResult.None;
            _dialogService.ShowDialog("FilterView",null,callback=> 
            {
                result=callback.Result;    
            });
            return result==ButtonResult.OK;
        }



        public object SelectedEntity { get; set; }

        private bool CanExecuteEdit(object view)
        {
            if (SelectedEntity == null)
            {
                return false;

            }
            var id = GetId(SelectedEntity);
            return id > 0 && AuthorizedToEdit();
        }

        protected virtual bool AuthorizedToEdit() => _authCache.CheckAccess($"Create {GetEntityName()}");

        [BackgroundTask(AspectPriority = 2)]
        private async Task ExecuteEditAsync(object view)
        {
            var id = GetId(SelectedEntity);
            TEntity entity = await GetEntityAsync(id);
            _dialogService.ShowDialog(view.ToString(),
                new DialogParameters { { "entity", (entity, false) }, { "caption", Caption }, { "captionimage", CaptionImage } });

        }

        protected int GetId(object entity) => GetPropertyValue<int>(entity, "Id");

        protected object GetPropertyValue(object entity, string name)
        {
            return entity?.GetType().GetProperty(name)?.GetValue(entity, null);
        }

        protected T GetPropertyValue<T>(object entity, string name)
        {
            object obj = GetPropertyValue(entity, name);
            if (obj == null) return default;
            return (T)obj;
        }






        //public static T GetPropValue<T>(this Object obj, String name) {
        //    Object retval = GetPropValue(obj, name);
        //    if (retval == null) { return default(T); }

        //    // throws InvalidCastException if types are incompatible
        //    return (T) retval;
        //}
        protected abstract Task<TEntity> GetEntityAsync(int id);

        private bool CanExecuteView(object view)
        {
            if (SelectedEntity == null)
            {
                return false;

            }
            var id = GetId(SelectedEntity);
            return view != null && id > 0 && AuthorizedToView();
        }

        protected virtual bool AuthorizedToView() => _authCache.CheckAccess($"View {GetEntityName()}");

        [BackgroundTask(AspectPriority = 2)]
        private async Task ExecuteViewAsync(object view)
        {
            var id = GetId(SelectedEntity);
            TEntity entity = await GetEntityAsync(id);
            _dialogService.ShowDialog(view.ToString(),
                new DialogParameters { { "entity", (entity, true) }, { "caption", Caption }, { "captionimage", CaptionImage } });

        }

        private bool CanExecuteNew(object view) => view != null && AuthorizedToAdd();

        protected virtual bool AuthorizedToAdd() => _authCache.CheckAccess($"Create {GetEntityName()}");

        [BackgroundTask(AspectPriority = 2)]
        private void ExecuteNew(object view)
        {
            var parameters = new DialogParameters { { "caption", Caption }, { "captionimage", CaptionImage } };
            _dialogService.ShowDialog(view.ToString(), parameters);
        }

        #region Commands

        public ICommand NewCommand { get; }
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
            string entityName = typeName.Substring(0, typeName.IndexOf("Model", StringComparison.Ordinal)).Humanize();
            return entityName;
        }

    }
}