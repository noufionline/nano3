// ***********************************************************************
// Assembly         : Jasmine.Core
// Author           : Noufal
// Created          : 12-07-2017
//
// Last Modified By : Noufal
// Last Modified On : 12-08-2017
// ***********************************************************************
// <copyright file="LookupItemViewModel.cs" company="CICON">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using DevExpress.Mvvm;
using FluentValidation;
using Humanizer;
using Jasmine.Core.Aspects;
using Jasmine.Core.Contracts;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Xaml;
using Prism.Events;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IDialogService = Prism.Services.Dialogs.IDialogService;
using MessageButton = DevExpress.Mvvm.MessageButton;
using MessageIcon = DevExpress.Mvvm.MessageIcon;
using static PostSharp.Patterns.Diagnostics.FormattedMessageBuilder;
namespace Jasmine.Core.Mvvm.LookupItems
{
    /// <summary>
    /// Enum LookupItemManageMode
    /// </summary>
    public enum LookupItemManageMode
    {
        /// <summary>
        /// The view
        /// </summary>
        View,
        /// <summary>
        /// The new
        /// </summary>
        New,
        /// <summary>
        /// The edit
        /// </summary>
        Edit,
        /// <summary>
        /// The delete
        /// </summary>
        Delete,
        /// <summary>
        /// The Refresh
        /// </summary>
        Refresh
    }

    /// <summary>
    /// Class LookupItemViewModel.
    /// </summary>
    public class LookupItemViewModel : ViewModelBase<LookupItemModel>
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly LogSource _logger;
        /// <summary>
        /// The lookup item service
        /// </summary>
        readonly ILookupItemService _lookupItemService;

        private readonly IDialogService _dialogService;

        /// <summary>
        /// The mode
        /// </summary>
        private LookupItemManageMode _mode;
        /// <summary>
        /// The route name
        /// </summary>
        string _routeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupItemViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="lookupItemService">The lookup item service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="authorizationCache"></param>
        /// <param name="auditService"></param>
        /// <param name="validators"></param>
        public LookupItemViewModel(IEventAggregator eventAggregator,
            ILookupItemService lookupItemService,
            IDialogService dialogService,
            IAuthorizationCache authorizationCache,
            IAuditService auditService,
            List<IValidator<LookupItemModel>> validators) : base(eventAggregator,
            dialogService,
            authorizationCache,
            auditService, validators)
        {
            _logger = LogSource.Get();
            _lookupItemService = lookupItemService;
            _dialogService = dialogService;
        }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public LookupItemManageMode Mode
        {
            get => _mode;
            private set
            {
                _mode = value;
                ModeChanged = !ModeChanged;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [mode changed].
        /// </summary>
        /// <value><c>true</c> if [mode changed]; otherwise, <c>false</c>.</value>
        public bool ModeChanged { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public List<LookupItemModel> Entities { get; set; }
        /// <summary>
        /// Gets or sets the selected entity.
        /// </summary>
        /// <value>The selected entity.</value>
        public LookupItemModel SelectedEntity { get; set; }

        /// <summary>
        /// on before save Selected LookupItem.
        /// </summary>
        /// <param name="args">The <see cref="T:System.ComponentModel.CancelEventArgs" /> instance containing the event data.</param>
        /// <returns>Task.</returns>
        public override Task OnBeforeSaveAsync(CancelEventArgs args)
        {
            //if (await _lookupItemService.IsDuplicatedAsync(_routeName, Entity))
            //{
            //    Entity.SetError("Name", $"{Caption} Duplicated");
            //    args.Cancel = true;
            //}

            //if (Entity.Id > 0)
            //{
            //    (bool success, string errorMessage) concurrencyCheckResult = await _lookupItemService.CheckConcurrency(_routeName, Entity);
            //    if (!concurrencyCheckResult.success)
            //    {
            //        Entity.SetError("Name", concurrencyCheckResult.errorMessage);
            //        ErrorMessage = concurrencyCheckResult.errorMessage;
            //        args.Cancel = true;
            //    }
            //    else
            //    {
            //        ErrorMessage = string.Empty;
            //    }
            //}
            return Task.CompletedTask;
        }

        public string ErrorMessage { get; set; }

        #region RefreshCollectionCommand

        [Command] public ICommand RefreshCollectionCommand { get; set; }


        private void ExecuteRefreshCollection()
        {
            ExecuteRefresh();
            ErrorMessage = string.Empty;
            Mode = LookupItemManageMode.View;
        }


        protected bool CanExecuteRefreshCollection => !string.IsNullOrEmpty(ErrorMessage);

        #endregion
        /// <summary>
        /// Determines whether this instance [can close dialog].
        /// </summary>
        /// <returns><c>true</c> if this instance [can close dialog]; otherwise, <c>false</c>.</returns>
        public override bool CanCloseDialog() => Mode == LookupItemManageMode.View;

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public override Task<LookupItemModel> SaveAsync(LookupItemModel entity)
        {
            entity.Name = entity.Name.Trim();
            return entity.Id == 0 ? _lookupItemService.SaveAsync(_routeName, entity) : _lookupItemService.UpdateAsync(_routeName, entity);

        }


        /// <summary>
        /// Called when [after save].
        /// </summary>
        /// <param name="mode"></param>
        public override void OnAfterSave(SaveMode mode)
        {
            Mode = LookupItemManageMode.View;
            RaiseCanExecuteChanged();
            RefreshEntities();
        }

        protected override async Task DeleteAsync()
        {
            try
            {
                await _lookupItemService.DeleteAsync(_routeName, SelectedEntity);
                RefreshEntities();
            }
            catch (LookupItemIsInUseException exception)
            {
                ErrorMessage = exception.Message;
                MessageBoxService.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.OK);
            }
        }

        public override bool ShowValidationSummary => false;


        /// <summary>
        /// Gets the entity asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public override Task<LookupItemModel> GetEntityAsync(int id) => Task.FromResult(new LookupItemModel());


        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            string key = "routeName";
            if (parameters.ContainsKey(key))
            {
                _routeName = parameters.GetValue<string>(key);
                Mode = LookupItemManageMode.View;
                RefreshEntities();
                Title = _routeName.Substring(_routeName.LastIndexOf("/") + 1).Titleize();
            }
        }

        /// <summary>
        /// Refreshes the entities.
        /// </summary>
        private void RefreshEntities()
        {
            _lookupItemService.GetAllAsync(_routeName).ContinueWith(task =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Entities = task.Result;
                }
                else if (task.Status == TaskStatus.Faulted && task.Exception != null)
                {
                    _logger.Error.Write(Formatted(task.Exception.Message));
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Determines whether this instance [can execute delete].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute delete]; otherwise, <c>false</c>.</returns>
        protected override bool CanExecuteDelete()
        {
            return SelectedEntity != null && Mode == LookupItemManageMode.View;
        }


        /// <summary>
        /// Determines whether this instance [can execute close].
        /// </summary>
        /// <returns><c>true</c> if this instance [can execute close]; otherwise, <c>false</c>.</returns>
        protected override bool CanExecuteClose()
        {
            return Mode == LookupItemManageMode.View;
        }

        #region NewCommand

        /// <summary>
        /// Gets or sets the new command.
        /// </summary>
        /// <value>The new command.</value>
        [Command]
        public ICommand NewCommand { get; set; }

        /// <summary>
        /// Executes the new.
        /// </summary>
        private void ExecuteNew()
        {
            Mode = LookupItemManageMode.New;
            Entity = new LookupItemModel();
            Entity.StartDirtyTracking();
        }


        /// <summary>
        /// Gets a value indicating whether [show editor].
        /// </summary>
        /// <value><c>true</c> if [show editor]; otherwise, <c>false</c>.</value>
        public bool ShowEditor => Mode == LookupItemManageMode.New || Mode == LookupItemManageMode.Edit;

        /// <summary>
        /// Gets a value indicating whether this instance can execute new.
        /// </summary>
        /// <value><c>true</c> if this instance can execute new; otherwise, <c>false</c>.</value>
        protected bool CanExecuteNew => Mode == LookupItemManageMode.View;

        #endregion

        #region EditCommand

        /// <summary>
        /// Gets or sets the edit command.
        /// </summary>
        /// <value>The edit command.</value>
        [Command]
        public ICommand EditCommand { get; set; }

        /// <summary>
        /// Executes the edit.
        /// </summary>
        private void ExecuteEdit()
        {
            Mode = LookupItemManageMode.Edit;

            _lookupItemService.GetAsync(_routeName, SelectedEntity.Id).ContinueWith(task =>
            {
                Entity = task.Result;
                Entity.StartDirtyTracking();
                RaiseCanExecuteChanged();
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        /// <summary>
        /// Gets a value indicating whether this instance can execute edit.
        /// </summary>
        /// <value><c>true</c> if this instance can execute edit; otherwise, <c>false</c>.</value>
        protected bool CanExecuteEdit => SelectedEntity != null && Mode == LookupItemManageMode.View;

        #endregion


        #region CancelCommand

        /// <summary>
        /// Gets or sets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        [Command]
        public ICommand CancelCommand { get; set; }


        /// <summary>
        /// Executes the cancel.
        /// </summary>
        private void ExecuteCancel()
        {
            Mode = LookupItemManageMode.View;
            RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Gets a value indicating whether this instance can execute cancel.
        /// </summary>
        /// <value><c>true</c> if this instance can execute cancel; otherwise, <c>false</c>.</value>
        protected bool CanExecuteCancel => Mode == LookupItemManageMode.New || Mode == LookupItemManageMode.Edit;

        #endregion

        #region RefreshCommand

        [Command]
        public ICommand RefreshCommand { get; set; }

        [BackgroundTask(AspectPriority = 2)]
        private void ExecuteRefresh()
        {
            RefreshEntities();
        }

        protected bool CanExecuteRefresh => Mode == LookupItemManageMode.View;

        #endregion

    }
}