using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.XtraReports.UI;
using FluentValidation;
using FluentValidation.Results;
using Humanizer;
using Jasmine.Core.Aspects;
using Jasmine.Core.Audit;
using Jasmine.Core.Common;
using Jasmine.Core.Contracts;
using Jasmine.Core.Events;
using Jasmine.Core.Properties;
using Jasmine.Core.Tracking;
using PostSharp.Patterns.Diagnostics;
using Prism.Events;
using Prism.Regions;
using IDialogService = Jasmine.Core.Contracts.IDialogService;

namespace Jasmine.Core.Mvvm
{
    /// <inheritdoc />
    /// <summary>
    /// Class AsyncViewModelBase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="T:Jasmine.Core.Mvvm.AsyncViewModelBase" />
    public abstract class ViewModelBase<T> : AsyncViewModelBase, IRegionManagerAware
        where T : class, IEntity, ITrackable, IIdentifiable, IDirty,
        INotifyPropertyChanged, ISupportFluentValidator<T>,
        INotifyDataErrorInfo, new()
    {
        private readonly IDialogService _dialogService;
        private readonly IAuditService _auditService;
        private readonly ILookupItemProviderService _lookupItemProviderService;
        private readonly IList<IValidator<T>> _validators;

        /// <summary>
        /// The change tracker
        /// </summary>
        private ChangeTrackingCollection<T> _changeTracker;




        private T _entity;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="authorizationCache"></param>
        /// <param name="auditService"></param>
        /// <param name="lookupItemProviderService"></param>
        /// <param name="validators"></param>
        protected ViewModelBase(IEventAggregator eventAggregator,
            IDialogService dialogService, IAuthorizationCache authorizationCache,
            IAuditService auditService, 
            ILookupItemProviderService lookupItemProviderService,IList<IValidator<T>> validators) : base(eventAggregator, dialogService, authorizationCache, lookupItemProviderService)
        {
            _dialogService = dialogService;
            _auditService = auditService;
            _lookupItemProviderService = lookupItemProviderService;
            _validators = validators;
            _logger = Logger.GetLogger("Custom", typeof(ViewModelBase<T>));

            SaveCommand = new AsyncCommand<SaveMode>(ExecuteSaveAsync, CanExecuteSave);
            ResetCommand = new AsyncCommand(ExecuteResetAsync, CanExecuteReset);
            DeleteCommand = new AsyncCommand(ExecuteDeleteAsync, CanExecuteDelete);
            PrintCommand = new AsyncCommand(ExecutePrintAsync, CanExecutePrint);
        }


        /// <summary>
        /// Determines whether this instance [can execute reset].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute reset]; otherwise, <c>false</c>.</returns>
        private bool CanExecuteReset()
        {
            return Entity != null && Entity.IsDirty && !IsReadonly;
        }

        /// <summary>
        /// execute delete as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>

        private async Task ExecuteDeleteAsync()
        {
            MessageResult result =
                MessageBoxService.ShowMessage(string.Format(CommonResources.Confirmation_Delete, typeof(T).Name),
                    GetCaption(), MessageButton.YesNo, MessageIcon.Question, MessageResult.No);

            if (result == MessageResult.Yes)
            {
                await DeleteAsync();
                EventAggregator.GetEvent<EntityDeletedEvent>().Publish();
                OnAfterDelete();
                Entity = new T();
                InitializeEntity(Entity);
            }
        }

        /// <summary>
        /// execute print as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        [BackgroundTask(AspectPriority = 2)]
        protected virtual async Task ExecutePrintAsync()
        {
            XtraReport report = await GetReportAsync();


            if (report != null)
            {
                if (OpenReportInPdfViewer)
                {
                    OpenPdf(report, ReportFileName);
                }
                else
                {   
                    _dialogService.ShowReport(report);
                }
            }
        }


        protected virtual string ReportFileName => null;

        private void OpenPdf(XtraReport report, string fileName = null)
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

           
            report.ExportToPdf(path);

            Process.Start(path, @"/r");
        }
        protected virtual bool OpenReportInPdfViewer => false;
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
                _logger.WriteException(LogLevel.Error, navigationResult.Error, "Navigation Failed while creating Print Preview...");
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
                entity.MakeDirty();
                RaiseCanExecuteChanged();
            };

            _changeTracker.Tracking = true;

        }

        private T ClonedEntity { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is executing.
        /// </summary>
        /// <value><c>true</c> if this instance is executing; otherwise, <c>false</c>.</value>
        private bool IsExecuting => SaveCommand.IsExecuting ||
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
        protected virtual bool CanExecutePrint() => Entity != null && Entity.Id > 0 && !Entity.IsDirty;


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
        }


        protected virtual HashSet<string> ExcludedProperties { get; } = new HashSet<string>();

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

        /// <summary>
        /// Gets the report asynchronous.
        /// </summary>
        /// <returns>Task&lt;XtraReport&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Task<XtraReport> GetReportAsync() => throw new NotImplementedException();//Task.FromResult<XtraReport>(null);

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            //navigationContext.Parameters.TryGetValue("fromReport", out bool fromReport);
            //if (!fromReport)

            if (navigationContext.Parameters.ContainsKey("entity"))
            {
                (T entity, bool readOnly) parameter =
                    navigationContext.Parameters.GetValue<(T entity, bool readOnly)>("entity");

                IsReadonly = parameter.readOnly;

                if (navigationContext.Parameters.ContainsKey("lookupitems"))
                {
                    LookupItemDictionary = navigationContext.Parameters.GetValue<LookupItemDictionary>("lookupitems");
                    FillLookupItems(parameter.entity);
                }

                Entity = parameter.entity;
                ClonedEntity = AuditExtentions.Clone(parameter.entity);
            }
            else
            {
                Entity = new T();
                InitializeEntity(Entity);
            }

            ISplashScreenService splashScreen = GetService<ISplashScreenService>();
            if (splashScreen != null && splashScreen.IsSplashScreenActive)
            {
                splashScreen.HideSplashScreen();
            }
        }


        public LookupItemDictionary LookupItemDictionary { get; set; }

        
        public TLookup GetLookupItems<TLookup>(string key)
        {

            if (LookupItemDictionary.ContainsKey(key))
            {
                object value = LookupItemDictionary[key];

                if (value.GetType() == typeof(TLookup))
                    return (TLookup)value;
                if (typeof(TLookup).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                    return (TLookup)value;
                return (TLookup)Convert.ChangeType(value, typeof(TLookup));
            }

            return default;
        }


        public List<LookupItem> GetLookupItems(string key)
        {
            return GetLookupItems<List<LookupItem>>(key);
        }

        public bool TryGetLookupItems(string key, out List<LookupItem> value)
        {
            return TryGetLookupItems<List<LookupItem>>(key, out value);
        }

        public bool TryGetLookupItems<TLookup>(string key, out TLookup value)
        {
            if (LookupItemDictionary.ContainsKey(key))
            {
                object result = LookupItemDictionary[key];
                if (result.GetType() == typeof(TLookup))
                    value = (TLookup) result;
                else if (typeof(TLookup).GetTypeInfo().IsAssignableFrom(result.GetType().GetTypeInfo()))
                    value = (TLookup) result;
                else if (result is object[] objects && !objects.Any())
                {
                    value = default;
                    return false;
                }
                else
                {
                    value = (TLookup) Convert.ChangeType(result, typeof(TLookup));
                }

                return true;

            }

            value = default;
            return false;
        }
        

        protected virtual void FillLookupItems(T entity)
        {
            
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
                _entity = value;
                if (_entity != null)
                {

                   
                    _entity.PropertyChanged += (s, e) => RaiseCanExecuteChanged();
                    _entity.ErrorsChanged += (s, e) =>
                    {
                        RefreshErrors();
                    };

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


        public bool IsReadonly { get; private set; }

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
                        break;
                    case SaveMode.SaveAndClose:
                        ExecuteClose(true,checkCanClose: false);
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
                _logger.WriteException(LogLevel.Error, exception.GetBaseException(), exception.GetBaseException().Message);
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
                Entity = await SaveAsync(Entity);
            }
            else
            {
                T modifiedEntity = _changeTracker.GetChanges().SingleOrDefault();
                if (modifiedEntity != null)
                {
                    //var auditManager = new AuditLogService();
                    //List<AuditLogLine> differences = new List<AuditLogLine>();
                    //auditManager.Compare(ClonedEntity, modifiedEntity, differences);
             
                    Entity = await SaveAsync(modifiedEntity);

                    //var log = auditManager.CreateAuditLog(ClonedEntity, modifiedEntity, Entity);
                    //log.Differences = differences;
                    //await SaveAudit(log);

                }
                else
                {
                    Entity = await GetEntityAsync(Entity.Id);
                }
            }

            EventAggregator.GetEvent<EntitySavedEvent>().Publish();
            _logger.Write(LogLevel.Info, "EntitySavedEvent published");
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


        public int GetLoggedDivisionId() => Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("DivisionId").Value);
        public LookupItem GetLoggedDivisionLookup()=> new LookupItem()
        {
            Id = GetLoggedDivisionId(),
            Name = GetLoggedDivisionName()
        };

        public string GetLoggedDivisionName()=> ClaimsPrincipal.Current.FindFirst("division").Value;
    }
}