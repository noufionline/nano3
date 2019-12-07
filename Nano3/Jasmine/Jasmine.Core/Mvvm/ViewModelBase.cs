using DevExpress.Mvvm;
using DevExpress.XtraReports.UI;
using FluentValidation.Results;
using Humanizer;
using Jasmine.Core.Aspects;
using Jasmine.Core.Contracts;
using Jasmine.Core.Dialogs;
using Jasmine.Core.Events;
using Jasmine.Core.Mvvm.LookupItems;
using Jasmine.Core.Properties;
using Jasmine.Core.Tracking;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Model;
using Prism;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Docking;
using DevExpress.XtraReports;
using FluentValidation;
using Jasmine.Core.Audit;
using Jasmine.Core.Repositories;
using Prism.Services.Dialogs;
using DelegateCommand = Prism.Commands.DelegateCommand;
using IDialogService = Prism.Services.Dialogs.IDialogService;
using static PostSharp.Patterns.Diagnostics.FormattedMessageBuilder;
using Jasmine.Core.Helpers;
using AutoBogus;
using PostSharp.Patterns.Xaml;

namespace Jasmine.Core.Mvvm
{

    [NotifyPropertyChanged]
    public abstract class ViewModelBase : DxMvvmServicesBase, IViewModelBase, IActiveAware, IAbsDialogAware, IConfirmNavigationRequest, IRegionMemberLifetime, INotifyPropertyChanged
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;


        public virtual bool ShowValidationSummary { get; set; } = true;
        protected ViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService, IAuthorizationCache authorizationCache)
        {
            _authorizationCache = authorizationCache;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            CloseCommand = new DelegateCommand(ExecuteClose);
            LookupItemCreationCommand = new AsyncCommand<string>(ExecuteLookupItemCreation, CanExecuteLookupItemCreation);
            LookupItemRefreshCommand = new AsyncCommand<string>(ExecuteLookupItemRefreshAsync);
        }

        public ICommand CloseCommand { get; set; }

        public virtual string Caption { get; set; }
        public virtual ImageSource CaptionImage { get; set; }
        public bool ShowBusyIndicator { get; set; }
        readonly IAuthorizationCache _authorizationCache;



        bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                OnIsActiveChanged();
            }
        }
        public event EventHandler IsActiveChanged;

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("caption"))
            {
                Caption = navigationContext.Parameters.GetValue<string>("caption");
            }
            if (navigationContext.Parameters.TryGetValue<ImageSource>("captionimage", out ImageSource captionImage))
                CaptionImage = captionImage;
        }



        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext,
            Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public virtual bool KeepAlive => false;

        protected virtual void OnIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual bool CanCloseDialog() => true;

        public bool? DialogResult { get; private set; }
        public bool ClosingFromViewModel { get; protected set; }

        public event Action RequestWindowClose;

        public virtual DialogOptions GetDialogOptions()
        {
            return null;
        }

        protected void ExecuteClose()
        {
            if (CanCloseDialog())
            {
                ClosingFromViewModel = true;
                RequestWindowClose?.Invoke();
                DialogResult = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
        public ISaveFileDialogService SaveFileDialogService => GetService<ISaveFileDialogService>();
        public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
        public ISplashScreenService SplashScreenService => GetService<ISplashScreenService>();
        public IDispatcherService DispatcherService => GetService<IDispatcherService>();

        #region CreateLookupItemCommand


        /// <summary>
        /// Gets or sets the lookup item creation command.
        /// </summary>
        /// <value>The lookup item creation command.</value>
        public AsyncCommand<string> LookupItemCreationCommand { get; set; }


        /// <summary>
        /// Executes the lookup item creation.
        /// </summary>
        /// <param name="routeName"></param>
        /// <exception cref="RouteNotFoundException">Route attribute for LookupItemEdit is not found</exception>
        private async Task ExecuteLookupItemCreation(string routeName)
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new RouteNotFoundException("Route attribute for LookupItemEdit is not found");
            var parameter = new DialogParameters { { "routeName", routeName } };
            _dialogService.ShowDialog("LookupItemView", parameter);
            await ExecuteLookupItemRefreshAsync(routeName);
            //_eventAggregator.GetEvent<RefreshLookupItemEvent>().Publish(routeName);
        }

        /// <summary>
        /// Determines whether this instance [can execute lookup item creation] the specified route name.
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute lookup item creation] the specified route name; otherwise, <c>false</c>.</returns>
        protected bool CanExecuteLookupItemCreation(string routeName) => routeName != null && _authorizationCache.CheckLookupItemAccess(routeName);

        #endregion

        #region RefreshLookupItemCommand

        /// <summary>
        /// Gets or sets the lookup item refresh command.
        /// </summary>
        /// <value>The lookup item refresh command.</value>
        public AsyncCommand<string> LookupItemRefreshCommand { get; set; }

        /// <summary>
        /// Executes the lookup item refresh.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <exception cref="RouteNotFoundException">Route attribute for LookupItemEdit is not found</exception>
        private async Task ExecuteLookupItemRefreshAsync(string routeName)
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new RouteNotFoundException("Route attribute for LookupItemEdit is not found");
            await RefreshLookupItemsAsync(routeName);
            //_eventAggregator.GetEvent<RefreshLookupItemEvent>().Publish(routeName);
        }

        protected virtual Task RefreshLookupItemsAsync(string route)
        {
            return Task.CompletedTask;
        }

        #endregion

        public virtual void OnClosed()
        {

        }
    }



    /// <summary>
    /// Class AsyncViewModelBase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TLookup">Lookup contains all the Lookup Items required for a view which comes as one set from server ( this is to avoid multiple calls to the server)</typeparam>
    /// <seealso cref="T:Jasmine.Core.Mvvm.AsyncViewModelBase" />
    public abstract class ViewModelBase<T> : DialogAwareViewModelBase, IDirtyAware, IRegionManagerAware, IConfirmNavigationRequest
        where T : class, IEntity, ITrackable, IMergeable, IIdentifiable, IDirty,
        INotifyPropertyChanged, ISupportFluentValidator<T>,
        INotifyDataErrorInfo, new()
    {
        private readonly IAuditService _auditService;

        /// <summary>
        /// The change tracker
        /// </summary>
        private ChangeTrackingCollection<T> _changeTracker;


        private readonly IDialogService _dialogService;





        private T _entity;
        private readonly IEventAggregator _eventAggregator;
        int _id;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly LogSource _logger;

        private readonly List<IValidator<T>> _validators;
        public string CollectionViewType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="authorizationCache"></param>
        /// <param name="auditService"></param>
        /// <param name="validators"></param>
        protected ViewModelBase(IEventAggregator eventAggregator,
            IDialogService dialogService, IAuthorizationCache authorizationCache,
            IAuditService auditService,
            List<IValidator<T>> validators) : base(eventAggregator, dialogService, authorizationCache)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _auditService = auditService;

            _validators = validators;
            _logger = LogSource.Get();
            NewCommand = new DevExpress.Mvvm.DelegateCommand(ExecuteNew, CanExecuteNew);
            SaveCommand = new AsyncCommand<SaveMode>(ExecuteSaveAsync, CanExecuteSave);
            ResetCommand = new AsyncCommand(ExecuteResetAsync, CanExecuteReset);
            DeleteCommand = new AsyncCommand(ExecuteDeleteAsync, CanExecuteDelete);
            PrintCommand = new AsyncCommand(ExecutePrintAsync, CanExecutePrint);
            CloseTabCommand = new DelegateCommand<UserControl>(ExecuteCloseTab);

            ViewLoaded = false;
            BackCommand = new DelegateCommand(ExecuteBack, CanExecuteBack);
            ForwardCommand = new DelegateCommand(ExecuteForward, CanExecuteForward);
        }


        private IRegionNavigationJournal _journal;
        public IRegionNavigationJournal Journal
        {
            get => _journal;
            private set
            {
                _journal = value;
                BackCommand.RaiseCanExecuteChanged();
                ForwardCommand.RaiseCanExecuteChanged();
            }
        }

        #region BackCommand

        public DelegateCommand BackCommand { get; set; }

        private bool CanExecuteBack() => Journal?.CanGoBack ?? false;
        private void ExecuteBack()
        {
            Journal.GoBack();
        }


        #endregion


        #region ForwardCommand

        public DelegateCommand ForwardCommand { get; set; }

        private bool CanExecuteForward() => Journal?.CanGoForward ?? false;
        private void ExecuteForward() => Journal.GoForward();

        #endregion




        #region NewCommand

        public DevExpress.Mvvm.DelegateCommand NewCommand { get; set; }

        protected bool CanExecuteNew() => Entity?.Id > 0 && !Entity.IsDirty;

        private void ExecuteNew()
        {
            Entity = new T();
            InitializeEntity(Entity);
        }

        #endregion


        /// <summary>
        /// Determines whether this instance [can execute reset].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute reset]; otherwise, <c>false</c>.</returns>
        private bool CanExecuteReset()
        {
            return Entity != null && Entity.IsDirty && !IsReadonly;
        }

        private static void CloseTab(UserControl parameter)
        {
            var docPanel = (DocumentPanel)LayoutHelper.FindLayoutOrVisualParentObject(parameter, typeof(DocumentPanel));
            var docManager = (DockLayoutManager)LayoutHelper.FindLayoutOrVisualParentObject(parameter, typeof(DockLayoutManager));
            if (docPanel != null && docManager != null)
            {
                docManager.DockController.Close(docPanel);

            }
        }
        void ExecuteCloseTab(UserControl parameter)
        {
            CloseTab(parameter);
        }

        /// <summary>
        /// execute delete as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>

        private async Task ExecuteDeleteAsync()
        {
            var name = Entity is ILookupItemModel ? Entity.ToString() : typeof(T).Name;
            MessageBoxResult result =
                MessageBoxService.Show(string.Format(CommonResources.Confirmation_Delete, name),
                    GetCaption(), MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                await DeleteAsync();
                _eventAggregator.GetEvent<NewEntityDeletedEvent>().Publish(CollectionViewType);
                OnAfterDelete();
                Entity = new T();
                Entity.StartDirtyTracking();
                InitializeEntity(Entity);
            }
        }
        /// <summary>
        /// execute reset as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task ExecuteResetAsync()
        {
            MessageResult result = MessageBoxService.ShowMessage(CommonResources.Confirmation_Reset,
                CommonResources.Confirmation_Caption, MessageButton.OKCancel, MessageIcon.Question,
                MessageResult.Cancel);

            if (result == MessageResult.OK)
            {
                await ResetChangesAsync();
            }
        }


        private string GetCaption() =>
            string.IsNullOrWhiteSpace(Caption) ? typeof(T).Name.Humanize(LetterCasing.Title) : Caption;

        private void NavigationCallBack(NavigationResult navigationResult)
        {
            if (navigationResult.Error != null)
            {
                _logger.Error.Write(FormattedMessageBuilder.Formatted("Navigation Failed while creating Print Preview..."));
            }
        }

        private void OpenPdf(IReport report, string fileName = null)
        {
            string fileToWriteTo = Path.GetRandomFileName();



            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (ReportFileName == null)
            {
                fileName = $"Document_{Entity.Id}.pdf";
            }


            string path = Path.Combine(directory, fileName ?? throw new ArgumentNullException(nameof(fileName)));


            ((XtraReport)report).ExportToPdf(path);

            Process.Start(path, @"/r");
        }

        private async Task ParseParameters(NavigationParameters parameters)
        {
            if (parameters.TryGetValue("collectionViewType", out string viewType))
                CollectionViewType = viewType;
            if (parameters.ContainsKey("entity"))
            {
                (int id, bool readOnly) parameter = parameters.GetValue<(int id, bool readOnly)>("entity");
                _id = parameter.id;
                IsReadonly = parameter.readOnly;

                Entity = await GetEntityAsync(parameter.id);

                ClonedEntity = AuditExtentions.Clone(Entity);
            }
            else
            {
                Entity = new T();
                InitializeEntity(Entity);
            }

            Entity.StartDirtyTracking();

            ISplashScreenService splashScreen = GetService<ISplashScreenService>();
            if (splashScreen != null && splashScreen.IsSplashScreenActive)
            {
                splashScreen.HideSplashScreen();
            }
        }

        private void RefreshErrors()
        {
            ShowValidationSummary = Entity.HasErrors;
            ValidationSummary = new ObservableCollection<ValidationFailure>(Entity.ValidationSummary);
        }
        private void StartTracking(T entity)
        {
            _changeTracker = new ChangeTrackingCollection<T>(entity);

            //_changeTracker.ExcludedProperties.Add("IsDirty");
            //_changeTracker.ExcludedProperties.Add("HasErrors");
            //_changeTracker.ExcludedProperties.Add("ValidationSummary");
            //_changeTracker.ExcludedProperties.Add("IsValid");



            if (ExcludedProperties != null)
            {
                foreach (string property in ExcludedProperties)
                {
                    if (!_changeTracker.ExcludedProperties.Contains(property))
                    {
                        _changeTracker.ExcludedProperties.Add(property);
                    }
                }
            }

            _changeTracker.EntityChanged += (s, e) =>
            {
                if (e.PropertyName != "IsDirty")
                {

                    entity.MakeDirty(true, e.PropertyName);
                    RaiseCanExecuteChanged();
                }

            };

            _changeTracker.Tracking = true;

        }

        private T ClonedEntity { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is executing.
        /// </summary>
        /// <value><c>true</c> if this instance is executing; otherwise, <c>false</c>.</value>
        public bool IsExecuting => SaveCommand.IsExecuting ||
                                    ResetCommand.IsExecuting || DeleteCommand.IsExecuting;

        /// <summary>
        /// Determines whether this instance [can execute delete].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute delete]; otherwise, <c>false</c>.</returns>
        protected virtual bool CanExecuteDelete() => Entity != null && Entity.Id > 0 && !IsReadonly && !Entity.IsDirty && !IsExecuting;

        /// <summary>
        /// Determines whether this instance [can execute print].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute print]; otherwise, <c>false</c>.</returns>
        protected virtual bool CanExecutePrint() => Entity != null && Entity.Id > 0 && !Entity.IsDirty && !Entity.HasErrors;


        /// <summary>
        /// Closes this instance.
        /// </summary>
        protected virtual void Close()
        {
            ExecuteClose();
        }

        /// <summary>
        /// Deletes entity asynchronously.
        /// </summary>
        /// <returns>Task.</returns>

        protected abstract Task DeleteAsync();

        /// <summary>
        /// execute print as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        //[BackgroundTask(AspectPriority = 2)]
        protected virtual async Task ExecutePrintAsync()
        {
            IReport report = await GetReportAsync();


            if (report != null)
            {
                if (OpenReportInPdfViewer)
                {
                    OpenPdf(report, ReportFileName);
                }
                else
                {
                    _dialogService.ShowDialog("ReportView", new DialogParameters { { "report", report } });
                    //_dialogService.ShowReport(report);
                }
            }
        }

        /// <summary>
        /// Initializes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void InitializeEntity(T entity)
        {

        }


        /// <summary>
        /// Called when [after delete].
        /// </summary>
        protected virtual void OnAfterDelete()
        {

        }


        protected virtual Task OnAfterEntityInitialized(T entity)
        {
            return Task.CompletedTask;
        }

        protected virtual void OnAfterRaiseCanExecuteChanged()
        {

        }

        /// <summary>
        /// Called when [entity set].
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void OnEntitySet(T entity)
        {

        }

        /// <summary>
        /// reset changes as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        protected async Task ResetChangesAsync()
        {
            if (Entity.Id == 0)
            {
                Entity = new T();
                InitializeEntity(Entity);
            }
            else
            {
                Entity = await GetEntityAsync(Entity.Id);
            }
            RaiseCanExecuteChanged();

            Entity.StartDirtyTracking();
        }


        protected virtual HashSet<string> ExcludedProperties { get; } = new HashSet<string>();
        protected virtual bool OpenReportInPdfViewer => false;


        protected virtual string ReportFileName => null;

        public override bool CanCloseDialog()
        {
            if (Entity == null) return true;

            if (!Entity.IsDirty) return true;

            if (Entity.HasErrors)
            {
                MessageResult warningResult = MessageBoxService.Show(CommonResources.Warning_SomeFieldsContainInvalidData,
                    CommonResources.Warning_Caption,
                    MessageButton.OKCancel, MessageIcon.Warning, MessageResult.Cancel);
                return warningResult == MessageResult.OK;
            }




            MessageResult result = MessageBoxService.Show(CommonResources.Confirmation_Save, GetCaption(),
                MessageButton.OKCancel, MessageIcon.Question, MessageResult.Cancel);

            return result != MessageResult.Cancel;
        }
        /// <summary>
        /// Gets the entity asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public abstract Task<T> GetEntityAsync(int id);

        public int GetLoggedDivisionId() => Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("DivisionId").Value);
        public LookupItem GetLoggedDivisionLookup() => new LookupItem
        {
            Id = GetLoggedDivisionId(),
            Name = GetLoggedDivisionName()
        };

        public string GetLoggedDivisionName() => ClaimsPrincipal.Current.FindFirst("division").Value;

        /// <summary>
        /// Gets the report asynchronous.
        /// </summary>
        /// <returns>Task&lt;XtraReport&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>        
        public virtual Task<XtraReport> GetReportAsync() => throw new NotImplementedException();//Task.FromResult<XtraReport>(null);
        #region Implementation of IDirtyAware

        public bool IsEntityIDirty()
        {
            return Entity != null && Entity.IsDirty;
        }

        #endregion


        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            ViewLoaded = false;
            base.OnDialogOpened(parameters);
            try
            {
                if (parameters == null) parameters = new DialogParameters();
                await ParseParameters(parameters as NavigationParameters);
                await OnAfterEntityInitialized(Entity);
            }
            finally
            {
                ViewLoaded = true;
            }
        }




        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            SaveCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            ResetCommand.RaiseCanExecuteChanged();
            OnAfterRaiseCanExecuteChanged();
        }


        public DelegateCommand<UserControl> CloseTabCommand { get; set; }

        /// <summary>
        /// Gets or sets the delete command.
        /// </summary>
        /// <value>The delete command.</value>
        public AsyncCommand DeleteCommand { get; set; }

        public T Entity
        {
            get => _entity;
            set
            {

                if (_entity != null)
                {
                    _entity.PropertyChanged -= _entity_PropertyChanged;
                    _entity.ErrorsChanged -= _entity_ErrorsChanged;
                }

                _entity = value;
                if (_entity != null)
                {

                    _entity.PropertyChanged += _entity_PropertyChanged;
                    _entity.ErrorsChanged += _entity_ErrorsChanged;

                    OnEntitySet(value);

                    _entity.MakeDirty(false);

                    RaiseCanExecuteChanged();


                    if (_entity.Id > 0)
                    {
                        StartTracking(_entity);
                    }

                    if (_validators?.Count > 0)
                    {
                        _entity.SetValidators(_validators);
                    }

                    _entity.ValidateSelf();

                    RefreshErrors();
                }
            }
        }

        private void _entity_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            RefreshErrors();
        }

        private void _entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        public bool IsReadonly { get; private set; }

        public virtual string LoadingMessage { get; set; } = "Please Wait....";

        /// <summary>
        /// Gets the print command.
        /// </summary>
        /// <value>The print command.</value>
        public AsyncCommand PrintCommand { get; }

        public IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Gets or sets the reset command.
        /// </summary>
        /// <value>The reset command.</value>
        public AsyncCommand ResetCommand { get; set; }

        /// <summary>
        /// Gets or sets the save command.
        /// </summary>
        /// <value>The save command.</value>
        public AsyncCommand<SaveMode> SaveCommand { get; set; }

        public bool ViewLoaded { get; set; }

        #region SaveCommand

        /// <summary>
        /// Determines whether this instance [can execute save] the specified mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns><c>true</c> if this instance [can execute save] the specified mode; otherwise, <c>false</c>.</returns>
        public virtual bool CanExecuteSave(SaveMode mode)
        {
            bool result = Entity?.IsDirty == true && !Entity.HasErrors && !IsReadonly;
            return result;
        }


        /// <summary>
        /// exexute save as an asynchronous operation.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ArgumentOutOfRangeException">mode - null</exception>

        private async Task ExecuteSaveAsync(SaveMode mode)
        {
            var args = new CancelEventArgs();
            await OnBeforeSaveAsync(args);
            bool success = await SaveCoreAsync(args);
            if (success)
            {
                switch (mode)
                {
                    case SaveMode.Save:
                        break;
                    case SaveMode.SaveAndNew:
                        Entity = new T();
                        InitializeEntity(Entity);
                        Entity.StartDirtyTracking();
                        break;
                    case SaveMode.SaveAndClose:
                        OnRequestClose(new DialogResult(ButtonResult.OK));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
                OnAfterSave(mode);
            }

        }

        /// <summary>
        /// save core as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>

        private async Task<bool> SaveCoreAsync(CancelEventArgs e)
        {
            try
            {
                ShowBusyIndicator = true;
                if (!e.Cancel && !Entity.HasErrors)
                {
                    await SaveCoreAsyncBase();
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                _logger.Error.Write(FormattedMessageBuilder.Formatted(exception.GetBaseException().Message));
                throw;
            }
            finally
            {
                ShowBusyIndicator = false;
                RaiseCanExecuteChanged();
            }
        }
        [ShowWaitIndicator(AspectPriority = 2)]
        private async Task SaveCoreAsyncBase()
        {
            if (Entity.Id == 0)
            {
                try
                {
                    Entity = await SaveAsync(Entity);
                }
                catch (EntityValidationException ex)
                {
                    foreach (var error in ex.Errors)
                    {
                        Entity.SetError(error.Key, error.Value);
                    }
                }
            }
            else
            {
                T modifiedEntity = _changeTracker.GetChanges().SingleOrDefault();
                if (modifiedEntity != null)
                {
                    //var auditManager = new AuditLogService();
                    //List<AuditLogLine> differences = new List<AuditLogLine>();
                    //auditManager.Compare(ClonedEntity, modifiedEntity, differences);

                    try
                    {
                        var updatedEntity = await SaveAsync(modifiedEntity);
                        _changeTracker.MergeChanges(updatedEntity);
                    }
                    catch (EntityValidationException ex)
                    {
                        foreach (var error in ex.Errors)
                        {
                            Entity.SetError(error.Key, error.Value);
                        }
                    }

                    //var log = auditManager.CreateAuditLog(ClonedEntity, modifiedEntity, Entity);
                    //log.Differences = differences;
                    //await SaveAudit(log);

                }
                else
                {
                    Entity = await GetEntityAsync(Entity.Id);
                }
            }

            Entity.StartDirtyTracking();

            _eventAggregator.GetEvent<NewEntitySavedEvent>().Publish(CollectionViewType);
            _logger.Info.Write(FormattedMessageBuilder.Formatted("EntitySavedEvent published"));
        }

        [AutoRetry]
        public async Task SaveAudit(AuditLog auditLog)
        {
            await _auditService.SaveAuditLogAsync(auditLog);
        }


        /// <summary>
        /// Handles the <see cref="E:BeforeSaveAsync" /> event.
        /// </summary>
        /// <param name="args">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <returns>Task.</returns>
        public virtual Task OnBeforeSaveAsync(CancelEventArgs args)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when [after save].
        /// </summary>
        /// <param name="mode"></param>
        public virtual void OnAfterSave(SaveMode mode)
        {

        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;T&gt;.</returns>

        public abstract Task<T> SaveAsync(T entity);

        #endregion

        #region Implementation of INavigationAware

        public virtual async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Journal = navigationContext.NavigationService.Journal;
            try
            {
                ViewLoaded = false;
                var parameters = navigationContext.Parameters;
                if (parameters == null) parameters = new NavigationParameters();
                await ParseParameters(parameters);
                await OnAfterEntityInitialized(Entity);
            }
            finally
            {
                ViewLoaded = true;
            }
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {

            if (navigationContext.Parameters.TryGetValue("entity", out (int id, bool readOnly) parameter))
            {
                if (parameter.id > 0 && parameter.id == _id) return true;
            }

            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {

            continuationCallback(true);
        }

        #endregion

        #region Fill Dummy Data
        [Command]
        public ICommand FillDummyDataCommand { get; set; }

        private void ExecuteFillDummyData()
        {
            var faker = ConfigureDummayData();
            faker.Populate(Entity);

        }

        protected virtual Bogus.Faker<T> ConfigureDummayData()
        {
            var faker = new AutoFaker<T>()
                 .Ignore(x => x.Id).Ignore(x => x.ModifiedProperties)
                 .Ignore(x => x.TrackingState).Ignore(x => x.EntityIdentifier);
            return faker;
        }
        #endregion



    }


    /// <inheritdoc />
    /// <summary>
    /// Class AsyncViewModelBase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TLookup">Lookup contains all the Lookup Items required for a view which comes as one set from server ( this is to avoid multiple calls to the server)</typeparam>
    /// <seealso cref="T:Jasmine.Core.Mvvm.AsyncViewModelBase" />
    public abstract class ViewModelBase<T, TLookup> : DialogAwareViewModelBase, IDirtyAware, ISupportAsyncOperation, IRegionManagerAware, IConfirmNavigationRequest
        where T : class, IEntity, ITrackable, IMergeable, IIdentifiable, IDirty,
        INotifyPropertyChanged, ISupportFluentValidator<T>,
        INotifyDataErrorInfo, new()
    where TLookup : class, new()
    {
        private readonly IAuditService _auditService;

        /// <summary>
        /// The change tracker
        /// </summary>
       // private ChangeTrackingCollection<T> _changeTracker;
        private ChangeTracker<T> _changeTracker;
        private readonly IDialogService _dialogService;



        public void SetChangeTracker(ChangeTracker<T> changeTracker)
        {
            _changeTracker = changeTracker;
            _changeTracker.EntityChanged += (s, e) => RaiseCanExecuteChanged();
        }


        private T _entity;
        private readonly IEventAggregator _eventAggregator;
        int _id;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly LogSource _logger;

        private readonly List<IValidator<T>> _validators;
        string _viewType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="authorizationCache"></param>
        /// <param name="auditService"></param>
        /// <param name="validators"></param>
        protected ViewModelBase(IEventAggregator eventAggregator,
            IDialogService dialogService, IAuthorizationCache authorizationCache,
            IAuditService auditService,
            List<IValidator<T>> validators) : base(eventAggregator, dialogService, authorizationCache)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _auditService = auditService;

            _validators = validators;
            _logger = LogSource.Get();
            SaveCommand = new AsyncCommand<SaveMode>(ExecuteSaveAsync, CanExecuteSave);
            NewCommand = new DevExpress.Mvvm.DelegateCommand(ExecuteNew, CanExecuteNew);
            ResetCommand = new AsyncCommand(ExecuteResetAsync, CanExecuteReset);
            DeleteCommand = new AsyncCommand(ExecuteDeleteAsync, CanExecuteDelete);
            PrintCommand = new AsyncCommand(ExecutePrintAsync, CanExecutePrint);
            CloseTabCommand = new DelegateCommand<UserControl>(ExecuteCloseTab);

            ViewLoaded = false;
        }

        #region NewCommand

        public DevExpress.Mvvm.DelegateCommand NewCommand { get; set; }

        protected bool CanExecuteNew() => Entity?.Id > 0 && !Entity.IsDirty;

        private void ExecuteNew()
        {
            Entity = new T();
            InitializeEntity(Entity);
            Entity.StartDirtyTracking();
        }

        #endregion        

        #region Fill Dummy Data
        [Command]
        public ICommand FillDummyDataCommand { get; set; }

        private void ExecuteFillDummyData()
        {
            var faker = ConfigureDummayData();
            faker.Populate(Entity);

        }

        protected virtual Bogus.Faker<T> ConfigureDummayData()
        {
            var faker = new AutoFaker<T>()
                 .Ignore(x => x.Id).Ignore(x => x.ModifiedProperties)
                 .Ignore(x => x.TrackingState).Ignore(x => x.EntityIdentifier);
            return faker;
        }
        #endregion
        /// <summary>
        /// Determines whether this instance [can execute reset].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute reset]; otherwise, <c>false</c>.</returns>
        private bool CanExecuteReset()
        {
            return Entity != null && Entity.IsDirty && !IsReadonly;
        }

        private static void CloseTab(UserControl parameter)
        {
            var docPanel = (DocumentPanel)LayoutHelper.FindLayoutOrVisualParentObject(parameter, typeof(DocumentPanel));
            var docManager = (DockLayoutManager)LayoutHelper.FindLayoutOrVisualParentObject(parameter, typeof(DockLayoutManager));
            if (docPanel != null && docManager != null)
            {
                docManager.DockController.Close(docPanel);

            }
        }
        void ExecuteCloseTab(UserControl parameter)
        {
            CloseTab(parameter);
        }

        /// <summary>
        /// execute delete as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>

        private async Task ExecuteDeleteAsync()
        {
            var name = Entity is ILookupItemModel ? Entity.ToString() : typeof(T).Name;
            MessageBoxResult result =
                MessageBoxService.Show(string.Format(CommonResources.Confirmation_Delete, name),
                    GetCaption(), MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                await DeleteAsync();
                _eventAggregator.GetEvent<NewEntityDeletedEvent>().Publish(_viewType);
                OnAfterDelete();
                Entity = new T();
                Entity.StartDirtyTracking();
                InitializeEntity(Entity);
            }
        }
        /// <summary>
        /// execute reset as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task ExecuteResetAsync()
        {
            MessageResult result = MessageBoxService.ShowMessage(CommonResources.Confirmation_Reset,
                CommonResources.Confirmation_Caption, MessageButton.OKCancel, MessageIcon.Question,
                MessageResult.Cancel);

            if (result == MessageResult.OK)
            {
                await ResetChangesAsync();
            }
        }


        private string GetCaption() =>
            string.IsNullOrWhiteSpace(Caption) ? typeof(T).Name.Humanize(LetterCasing.Title) : Caption;

        private void NavigationCallBack(NavigationResult navigationResult)
        {
            if (navigationResult.Error != null)
            {
                _logger.Error.Write(FormattedMessageBuilder.Formatted("Navigation Failed while creating Print Preview..."));
            }
        }

        private void OpenPdf(IReport report, string fileName = null)
        {
            string fileToWriteTo = Path.GetRandomFileName();



            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (ReportFileName == null)
            {
                fileName = $"Document_{Entity.Id}.pdf";
            }


            string path = Path.Combine(directory, fileName ?? throw new ArgumentNullException(nameof(fileName)));


            ((XtraReport)report).ExportToPdf(path);

            Process.Start(path, @"/r");
        }

        private async Task ParseParameters(NavigationParameters parameters)
        {
            if (parameters.TryGetValue("collectionViewType", out string viewType))
                _viewType = viewType;

            if (parameters.ContainsKey("entity"))
            {
                (int id, bool readOnly) parameter = parameters.GetValue<(int id, bool readOnly)>("entity");
                _id = parameter.id;
                IsReadonly = parameter.readOnly;

                Entity = await GetEntityAsync(parameter.id);

                ClonedEntity = AuditExtentions.Clone(Entity);
            }
            else
            {
                Entity = new T();
                InitializeEntity(Entity);
            }

            Entity.StartDirtyTracking();

            ISplashScreenService splashScreen = GetService<ISplashScreenService>();
            if (splashScreen != null && splashScreen.IsSplashScreenActive)
            {
                splashScreen.HideSplashScreen();
            }
        }

        private void RefreshErrors()
        {
            ShowValidationSummary = Entity.HasErrors;
            ValidationSummary = new ObservableCollection<ValidationFailure>(Entity.ValidationSummary);
        }

        private void StartTracking(T entity)
        {
            if (_changeTracker == null)
            {
                SetChangeTracker(new ChangeTracker<T>());
            }

            _changeTracker.StartTracking(entity, ExcludedProperties.ToArray());
        }
        //private void StartTracking(T entity)
        //{
        //    _changeTracker = new ChangeTrackingCollection<T>(entity);

        //    //_changeTracker.ExcludedProperties.Add("IsDirty");
        //    //_changeTracker.ExcludedProperties.Add("HasErrors");
        //    //_changeTracker.ExcludedProperties.Add("ValidationSummary");
        //    //_changeTracker.ExcludedProperties.Add("IsValid");



        //    if (ExcludedProperties != null)
        //    {
        //        foreach (string property in ExcludedProperties)
        //        {
        //            if (!_changeTracker.ExcludedProperties.Contains(property))
        //            {
        //                _changeTracker.ExcludedProperties.Add(property);
        //            }
        //        }
        //    }

        //    _changeTracker.EntityChanged += (s, e) =>
        //    {
        //        if (e.PropertyName != "IsDirty")
        //        {
        //            entity.MakeDirty(true, e.PropertyName);
        //            RaiseCanExecuteChanged();
        //        }

        //    };

        //    _changeTracker.Tracking = true;

        //}

        private T ClonedEntity { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is executing.
        /// </summary>
        /// <value><c>true</c> if this instance is executing; otherwise, <c>false</c>.</value>
        public bool IsExecuting => SaveCommand.IsExecuting ||
                                    ResetCommand.IsExecuting || DeleteCommand.IsExecuting;

        /// <summary>
        /// Determines whether this instance [can execute delete].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute delete]; otherwise, <c>false</c>.</returns>
        protected virtual bool CanExecuteDelete() => Entity != null && Entity.Id > 0 && !IsReadonly && !Entity.IsDirty && !IsExecuting;

        /// <summary>
        /// Determines whether this instance [can execute print].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute print]; otherwise, <c>false</c>.</returns>
        protected virtual bool CanExecutePrint() => Entity != null && Entity.Id > 0 && !Entity.IsDirty && !Entity.HasErrors;


        /// <summary>
        /// Closes this instance.
        /// </summary>
        protected virtual void Close()
        {
            ExecuteClose();
        }

        /// <summary>
        /// Deletes entity asynchronously.
        /// </summary>
        /// <returns>Task.</returns>

        protected abstract Task DeleteAsync();

        /// <summary>
        /// execute print as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        protected virtual async Task ExecutePrintAsync()
        {
            IReport report = await GetReportAsync();


            if (report != null)
            {
                if (OpenReportInPdfViewer)
                {
                    OpenPdf(report, ReportFileName);
                }
                else
                {
                    _dialogService.ShowDialog("ReportView", new DialogParameters { { "report", report } });
                    // _dialogService.ShowReport(report);
                }
            }
        }

        protected abstract Task<TLookup> GetLookupAsync();

        /// <summary>
        /// Initializes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void InitializeEntity(T entity)
        {

        }


        /// <summary>
        /// Called when [after delete].
        /// </summary>
        protected virtual void OnAfterDelete()
        {

        }


        protected virtual Task OnAfterEntityInitialized(T entity)
        {
            return Task.CompletedTask;
        }

        protected virtual void OnAfterRaiseCanExecuteChanged()
        {

        }

        /// <summary>
        /// Called when [entity set].
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void OnEntitySet(T entity)
        {

        }

        /// <summary>
        /// reset changes as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        protected async Task ResetChangesAsync()
        {
            if (Entity.Id == 0)
            {
                Entity = new T();
                InitializeEntity(Entity);
            }
            else
            {
                Entity = await GetEntityAsync(Entity.Id);
            }
            RaiseCanExecuteChanged();

            Entity.StartDirtyTracking();
        }


        protected virtual HashSet<string> ExcludedProperties { get; } = new HashSet<string>();
        protected virtual bool OpenReportInPdfViewer => false;


        protected virtual string ReportFileName => null;

        public override bool CanCloseDialog()
        {
            if (Entity.HasErrors)
            {
                MessageResult warningResult = MessageBoxService.Show(CommonResources.Warning_SomeFieldsContainInvalidData,
                    CommonResources.Warning_Caption,
                    MessageButton.OKCancel, MessageIcon.Warning, MessageResult.Cancel);
                return warningResult == MessageResult.OK;
            }

            if (!Entity.IsDirty) return true;


            MessageResult result = MessageBoxService.Show(CommonResources.Confirmation_Save, GetCaption(),
                MessageButton.OKCancel, MessageIcon.Question, MessageResult.Cancel);

            return result != MessageResult.Cancel;
        }
        /// <summary>
        /// Gets the entity asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public abstract Task<T> GetEntityAsync(int id);

        public int GetLoggedDivisionId() => Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("DivisionId").Value);
        public LookupItem GetLoggedDivisionLookup() => new LookupItem
        {
            Id = GetLoggedDivisionId(),
            Name = GetLoggedDivisionName()
        };

        public string GetLoggedDivisionName() => ClaimsPrincipal.Current.FindFirst("division").Value;

        /// <summary>
        /// Gets the report asynchronous.
        /// </summary>
        /// <returns>Task&lt;XtraReport&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Task<XtraReport> GetReportAsync() => throw new NotImplementedException();//Task.FromResult<XtraReport>(null);
        #region Implementation of IDirtyAware

        public bool IsEntityIDirty()
        {
            return Entity != null && Entity.IsDirty;
        }

        #endregion


        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            ViewLoaded = false;
            base.OnDialogOpened(parameters);
            try
            {
                Lookup = await GetLookupAsync();
                await ParseParameters(parameters as NavigationParameters);
                await OnAfterEntityInitialized(Entity);
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessage(ex.Message, "Error", MessageButton.OK, MessageIcon.Error);
            }
            finally
            {
                ViewLoaded = true;
            }
        }






        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            SaveCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            ResetCommand.RaiseCanExecuteChanged();
            OnAfterRaiseCanExecuteChanged();
        }


        public DelegateCommand<UserControl> CloseTabCommand { get; set; }

        /// <summary>
        /// Gets or sets the delete command.
        /// </summary>
        /// <value>The delete command.</value>
        public AsyncCommand DeleteCommand { get; set; }
        public T Entity
        {
            get => _entity;
            set
            {

                if (_entity != null)
                {
                    _entity.PropertyChanged -= _entity_PropertyChanged;
                    _entity.ErrorsChanged -= _entity_ErrorsChanged;
                }

                _entity = value;

                if (_entity != null)
                {

                    _entity.PropertyChanged += _entity_PropertyChanged;
                    _entity.ErrorsChanged += _entity_ErrorsChanged;

                    OnEntitySet(value);

                    _entity.MakeDirty(false);

                    RaiseCanExecuteChanged();


                    if (_entity.Id > 0)
                    {
                        StartTracking(_entity);
                    }

                    if (_validators != null)
                    {
                        _entity.SetValidators(_validators);
                        _entity.ValidateSelf();
                    }

                    RefreshErrors();
                }
            }
        }

        private void _entity_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            RefreshErrors();
        }

        private void _entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        protected void MarkAsReadonly(bool readOnly)
        {
            IsReadonly = readOnly;
        }
        public bool IsReadonly { get; private set; }

        public virtual string LoadingMessage { get; set; } = "Please Wait....";

        public TLookup Lookup { get; private set; }

        /// <summary>
        /// Gets the print command.
        /// </summary>
        /// <value>The print command.</value>
        public AsyncCommand PrintCommand { get; }

        public IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Gets or sets the reset command.
        /// </summary>
        /// <value>The reset command.</value>
        public AsyncCommand ResetCommand { get; set; }

        /// <summary>
        /// Gets or sets the save command.
        /// </summary>
        /// <value>The save command.</value>
        public AsyncCommand<SaveMode> SaveCommand { get; set; }

        public bool ViewLoaded { get; set; }

        #region SaveCommand

        /// <summary>
        /// Determines whether this instance [can execute save] the specified mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns><c>true</c> if this instance [can execute save] the specified mode; otherwise, <c>false</c>.</returns>
        public virtual bool CanExecuteSave(SaveMode mode)
        {
            bool result = Entity?.IsDirty == true && !Entity.HasErrors && !IsReadonly;
            //Debug.WriteLine($"Entity.IsDirty={Entity?.IsDirty}");
            //Debug.WriteLine($"Entity.HasErrors={Entity?.HasErrors}");
            //Debug.WriteLine($"IsReadonly={IsReadonly}");
            //Debug.WriteLine($"Result={result}");
            return result;
        }


        /// <summary>
        /// exexute save as an asynchronous operation.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ArgumentOutOfRangeException">mode - null</exception>
        [ShowWaitIndicator(AspectPriority = 2)]
        private async Task ExecuteSaveAsync(SaveMode mode)
        {
            var args = new CancelEventArgs();
            await OnBeforeSaveAsync(args);
            bool success = await SaveCoreAsync(args);
            if (success)
            {
                switch (mode)
                {
                    case SaveMode.Save:
                        break;
                    case SaveMode.SaveAndNew:
                        Entity = new T();
                        InitializeEntity(Entity);
                        Entity.StartDirtyTracking();
                        break;
                    case SaveMode.SaveAndClose:
                        OnRequestClose(new DialogResult(ButtonResult.OK));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
                OnAfterSave(mode);
            }

        }

        /// <summary>
        /// save core as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>

        private async Task<bool> SaveCoreAsync(CancelEventArgs e)
        {
            try
            {
                ShowBusyIndicator = true;
                ViewLoaded = false;
                if (!e.Cancel && !Entity.HasErrors)
                {
                    await SaveCoreAsyncBase();
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                _logger.Error.Write(Formatted(exception.GetBaseException().Message));
                throw;
            }
            finally
            {
                ShowBusyIndicator = false;
                ViewLoaded = true;
                RaiseCanExecuteChanged();
            }
        }
        //    [ShowWaitIndicator(AspectPriority = 2)]
        private async Task SaveCoreAsyncBase()
        {
            if (Entity.Id == 0)
            {
                try
                {
                    Entity = await SaveAsync(Entity);
                }
                catch (EntityValidationException ex)
                {
                    foreach (var error in ex.Errors)
                    {
                        Entity.SetError(error.Key, error.Value);
                    }
                }
            }
            else
            {
                T modifiedEntity = _changeTracker.GetModifiedEntity();
                if (modifiedEntity != null)
                {
                    //var auditManager = new AuditLogService();
                    //List<AuditLogLine> differences = new List<AuditLogLine>();
                    //auditManager.Compare(ClonedEntity, modifiedEntity, differences);

                    try
                    {
                        var updatedEntity = await SaveAsync(modifiedEntity);
                        _changeTracker.MergeChanges(updatedEntity);
                    }
                    catch (EntityValidationException ex)
                    {
                        foreach (var error in ex.Errors)
                        {
                            Entity.SetError(error.Key, error.Value);
                        }
                    }

                    //var log = auditManager.CreateAuditLog(ClonedEntity, modifiedEntity, Entity);
                    //log.Differences = differences;
                    //await SaveAudit(log);

                }
                else
                {
                    Entity = await GetEntityAsync(Entity.Id);
                }
            }

            Entity.StartDirtyTracking();

            _eventAggregator.GetEvent<NewEntitySavedEvent>().Publish(_viewType);
            _logger.Info.Write(FormattedMessageBuilder.Formatted("EntitySavedEvent published"));
        }

        //public T GetChanges()
        //{
        //    return _changeTracker.GetModifiedEntity();
        //}

        [AutoRetry]
        public async Task SaveAudit(AuditLog auditLog)
        {
            await _auditService.SaveAuditLogAsync(auditLog);
        }


        /// <summary>
        /// Handles the <see cref="E:BeforeSaveAsync" /> event.
        /// </summary>
        /// <param name="args">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <returns>Task.</returns>
        public virtual Task OnBeforeSaveAsync(CancelEventArgs args)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when [after save].
        /// </summary>
        /// <param name="mode"></param>
        public virtual void OnAfterSave(SaveMode mode)
        {

        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;T&gt;.</returns>

        public abstract Task<T> SaveAsync(T entity);

        #endregion


        #region Implementation of INavigationAware

        public virtual async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                ViewLoaded = false;
                Lookup = await GetLookupAsync();
                await ParseParameters(navigationContext.Parameters);
                await OnAfterEntityInitialized(Entity);
            }
            finally
            {
                ViewLoaded = true;
            }
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {

            if (navigationContext.Parameters.TryGetValue("entity", out (int id, bool readOnly) parameter))
            {
                if (parameter.id > 0 && parameter.id == _id) return true;
            }

            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {

            continuationCallback(true);
        }

        #endregion

    }

    //[Obsolete("Will be replaced with ViewModelWithFluentValidatorBase<T>",false)]
    //public abstract class ViewModelBase<T> : ViewModelBase
    //    where T : class, IEntity, ITrackable, IIdentifiable, IDirty,
    //    INotifyPropertyChanged, IValidatable, ISupportValidation,
    //    INotifyDataErrorInfo, new()
    //{
    //    private readonly IEventAggregator _eventAggregator;
    //    private readonly IDialogService _dialogService;

    //    private readonly LogSource _logger;

    //    private ChangeTrackingCollection<T> _changeTracker;

    //    protected ViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService) : base(eventAggregator, dialogService)
    //    {
    //        _eventAggregator = eventAggregator;
    //        _dialogService = dialogService;
    //        _logger = LogSource.Get();


    //        SaveCommand = new DelegateCommand(ExexuteSave, CanExecuteSave);
    //        SaveAndNewCommand = new DelegateCommand(SaveAndNew, CanExecuteSave);
    //        SaveAndCloseCommand = new DelegateCommand(SaveAndClose, CanExecuteSave);
    //        ResetCommand = new DelegateCommand(ExecuteReset, CanExecuteReset)
    //            .ObservesProperty(() => Entity)
    //            .ObservesProperty(() => Entity.IsDirty);
    //        DeleteCommand = new DelegateCommand(ExecuteDelete, CanExecuteDelete);
    //        PrintCommand = new DelegateCommand(ExecutePrint, CanExecutePrint);
    //    }

    //    [ShowWaitIndicator(AspectPriority = 2)]
    //    private void ExecutePrint()
    //    {
    //        XtraReport report = GetReport();
    //        if (report != null)
    //        {
    //            var parameter = new DialogParameters { { "Report", report } };
    //            _dialogService.ShowDialog("ReportView", parameter);
    //        }
    //    }

    //    private bool CanExecutePrint()
    //    {
    //        return Entity != null && Entity.Id > 0 && !Entity.IsDirty;
    //    }

    //    public virtual XtraReport GetReport()
    //    {
    //        return null;
    //    }

    //    public DelegateCommand PrintCommand { get; }
    //    protected virtual void OnEntitySet(T entity)
    //    {

    //    }
    //    private bool CanExecuteDelete() =>Entity != null && Entity.Id > 0 && !IsReadonly && !Entity.IsDirty;


    //    private void ExecuteDelete()
    //    {
    //        MessageResult result =
    //            MessageBoxService.ShowMessage(string.Format(CommonResources.Confirmation_Delete, typeof(T).Name),
    //                GetCaption(), MessageButton.YesNo, MessageIcon.Question, MessageResult.No);

    //        if (result == MessageResult.Yes)
    //        {
    //            Delete();
    //            _eventAggregator.GetEvent<EntityDeletedEvent>().Publish();
    //            _logger.Info.Write(Formatted("EntityDeletedEvent published"));
    //            OnAfterDelete();
    //            Entity = new T();

    //        }
    //    }


    //    protected virtual void OnAfterDelete()
    //    {

    //    }

    //    protected abstract void Delete();

    //    private bool CanExecuteReset()
    //    {
    //        return Entity!=null &&  Entity.IsDirty && !IsReadonly;
    //    }

    //    private void ExecuteReset()
    //    {
    //        MessageResult result = MessageBoxService.ShowMessage(CommonResources.Confirmation_Reset,
    //            CommonResources.Confirmation_Caption, MessageButton.OKCancel, MessageIcon.Question,
    //            MessageResult.Cancel);

    //        if (result == MessageResult.OK)
    //        {
    //            ResetChanges();
    //        }
    //    }

    //    protected void ResetChanges()
    //    {
    //        if (Entity.Id == 0)
    //        {
    //            Entity = new T();
    //            InitializeEntity(Entity);
    //        }
    //        else
    //        {
    //            Entity = GetEntity(Entity.Id);
    //        }
    //        RaiseCanExecuteChanged();
    //    }

    //    public abstract T GetEntity(int id);

    //    public DelegateCommand SaveCommand { get; }
    //    public DelegateCommand SaveAndNewCommand { get; }
    //    public DelegateCommand SaveAndCloseCommand { get; }
    //    public DelegateCommand ResetCommand { get; }
    //    public DelegateCommand DeleteCommand { get; }

    //    #region SaveCommand

    //    public virtual bool CanExecuteSave()
    //    {
    //        bool result = Entity?.IsDirty == true && !Entity.HasErrors && !IsReadonly;
    //        return result;
    //    }

    //    private void ExexuteSave()
    //    {
    //        SaveCore();
    //    }


    //    private bool SaveCore()
    //    {

    //        try
    //        {
    //            ShowBusyIndicator = true;
    //            var args = new CancelEventArgs();
    //            OnBeforeSave(args);
    //            if (!args.Cancel && !Entity.HasErrors)
    //            {
    //                if (Entity.Id == 0)
    //                {
    //                    Entity = Save(Entity);
    //                }
    //                else
    //                {
    //                    T modifiedEntity = _changeTracker.GetChanges().SingleOrDefault();
    //                    if (modifiedEntity != null)
    //                    {
    //                        //var result = Save(entityWithChanges);
    //                        //_changeTracker.MergeChanges(result);
    //                        //Entity.IsDirty = false;
    //                        Entity = Save(modifiedEntity);
    //                    }
    //                    else
    //                    {
    //                        Entity = GetEntity(Entity.Id);
    //                        RaiseCanExecuteChanged();
    //                    }
    //                }

    //                RaiseCanExecuteChanged();

    //                OnAfterSave();
    //                _eventAggregator.GetEvent<EntitySavedEvent>().Publish();
    //                _logger.Info.Write(Formatted("EntitySavedEvent published"));
    //                return true;
    //            }
    //            RaiseCanExecuteChanged();
    //            return false;
    //        }
    //        finally
    //        {
    //            ShowBusyIndicator = false;
    //        }
    //    }

    //    public virtual void OnBeforeSave(CancelEventArgs args)
    //    {

    //    }

    //    public virtual void OnAfterSave()
    //    {

    //    }

    //    public abstract T Save(T entity);

    //    #endregion

    //    private void SaveAndNew()
    //    {
    //        if (SaveCore())
    //        {
    //            Entity = new T();

    //        }
    //    }

    //    private void SaveAndClose()
    //    {
    //        if (SaveCore())
    //        {
    //            Close();
    //        }
    //    }

    //    protected virtual void Close()
    //    {
    //        ExecuteClose();
    //    }

    //    public void RaiseCanExecuteChanged()
    //    {
    //        SaveCommand.RaiseCanExecuteChanged();
    //        SaveAndNewCommand.RaiseCanExecuteChanged();
    //        SaveAndCloseCommand.RaiseCanExecuteChanged();
    //        DeleteCommand.RaiseCanExecuteChanged();
    //        ResetCommand.RaiseCanExecuteChanged();

    //        OnAfterRaiseCanExecuteChanged();
    //    }

    //    public override void OnNavigatedTo(NavigationContext navigationContext)
    //    {
    //        base.OnNavigatedTo(navigationContext);
    //        if (navigationContext.Parameters.ContainsKey("entity"))
    //        {
    //            (int id, bool readOnly) parameter = navigationContext.Parameters.GetValue<(int id, bool readOnly)>("entity");

    //            IsReadonly = parameter.readOnly;
    //            Entity = GetEntity(parameter.id);
    //            Entity.StartDirtyTracking();
    //        }
    //        else
    //        {
    //            Entity = new T();
    //            InitializeEntity(Entity);
    //            Entity.StartDirtyTracking();
    //        }

    //        ISplashScreenService splashScreen = GetService<ISplashScreenService>();
    //        if (splashScreen != null && splashScreen.IsSplashScreenActive)
    //        {
    //            splashScreen.HideSplashScreen();
    //        }
    //    }

    //    protected virtual void InitializeEntity(T entity)
    //    {

    //    }


    //    public bool IsReadonly { get; private set; }

    //    public T Entity
    //    {
    //        get => _entity;
    //        set
    //        {
    //            _entity = value;
    //            if (_entity != null)
    //            {
    //                _entity.PropertyChanged += (s, e) => RaiseCanExecuteChanged();
    //                _entity.ErrorsChanged += (s, e) =>
    //                {
    //                    RefreshErrors();
    //                };

    //                if (_entity.Id > 0)
    //                {
    //                    _changeTracker = new ChangeTrackingCollection<T>(_entity);

    //                    _changeTracker.EntityChanged += (s, e) =>
    //                    {
    //                        Entity.MakeDirty(true,e.PropertyName);
    //                        Entity.ValidateSelf();
    //                        RaiseCanExecuteChanged();
    //                    };

    //                    _changeTracker.Tracking = true;
    //                    RaiseCanExecuteChanged();
    //                }

    //                _entity.MakeDirty(false);

    //                RefreshErrors();

    //            }
    //            OnEntitySet(value);

    //        }
    //    }

    //    private void RefreshErrors()
    //    {
    //        ShowValidationSummary = Entity.HasErrors;
    //        ValidationSummary = new ObservableCollection<ValidationFailure>(Entity.ValidationSummary);
    //    }

    //    public ObservableCollection<ValidationFailure> ValidationSummary { get; set; }


    //    private T _entity;

    //    public override bool CanCloseDialog()
    //    {
    //        if (Entity.HasErrors)
    //        {
    //            MessageResult warningResult = MessageBoxService.Show(CommonResources.Warning_SomeFieldsContainInvalidData,
    //                CommonResources.Warning_Caption,
    //                MessageButton.OKCancel, MessageIcon.Warning, MessageResult.Cancel);
    //            return warningResult == MessageResult.OK;
    //        }
    //        if (!Entity.IsDirty) return true;
    //        MessageResult result = MessageBoxService.Show(CommonResources.Confirmation_Save, GetCaption(),
    //            MessageButton.YesNoCancel, MessageIcon.Question, MessageResult.Cancel);
    //        if (result == MessageResult.Yes)
    //        {
    //            return SaveCore();
    //        }
    //        return result != MessageResult.Cancel;
    //    }

    //    private string GetCaption() =>
    //        string.IsNullOrWhiteSpace(Caption) ? typeof(T).Name.Humanize(LetterCasing.Title) : Caption;

    //    protected virtual void OnAfterRaiseCanExecuteChanged()
    //    {

    //    }
    //}
}