using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using FluentValidation.Results;
using Jasmine.Core.Common;
using Jasmine.Core.Contracts;
using Jasmine.Core.Helpers;
using Jasmine.Core.Mvvm.LookupItems;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using Prism;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using IDialogService = Prism.Services.Dialogs.IDialogService;

namespace Jasmine.Core.Mvvm
{
    /// <summary>
    /// Class AsyncViewModelBase.
    /// </summary>
    /// <seealso cref="Jasmine.Core.Mvvm.DxMvvmServicesBase" />
    /// <seealso cref="Jasmine.Core.Contracts.IViewModelBase" />
    /// <seealso cref="IActiveAware" />
    /// <seealso>
    ///     <cref>Jasmine.Core.Contracts.IDialogAwareAsync</cref>
    /// </seealso>
    /// <seealso cref="IConfirmNavigationRequest" />
    /// <seealso cref="IRegionMemberLifetime" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    [NotifyPropertyChanged]
    public abstract class DialogAwareViewModelBase : DxMvvmServicesBase, IViewModelBase, IDialogAware, INotifyPropertyChanged
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IAuthorizationCache _authorizationCache;



        /// <summary>
        /// Initializes a new instance of the <see cref="DialogAwareViewModelBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="authorizationCache"></param>
        protected DialogAwareViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService,
            IAuthorizationCache authorizationCache)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _authorizationCache = authorizationCache;

            CloseCommand = new DelegateCommand(ExecuteClose, CanExecuteClose);
            LookupItemCreationCommand = new global::Prism.Commands.DelegateCommand<string>(ExecuteLookupItemCreation, CanExecuteLookupItemCreation);
            LookupItemRefreshCommand = new AsyncCommand<string>(ExecuteLookupItemRefreshAsync);
            LoadLookupItemsCommand = new AsyncCommand<string>(ExecuteLoadLookupItemsAsync);
        }

        public AsyncCommand<string> LoadLookupItemsCommand { get; set; }

        private async Task ExecuteLoadLookupItemsAsync(string route)
        {
            await RefreshLookupItemsAsync(route);
        }

        protected virtual Task RefreshLookupItemsAsync(string route)
        {
            return Task.CompletedTask;
        }



        public Guid AlfrescoRootFolderId =>
            GetRootFolder();
        [PostSharp.Patterns.Model.Pure]
        private Guid GetRootFolder()
        {
            return Guid.Parse(ConfigurationManager
                .AppSettings["AlfrescoDocumentLibrary"]);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when [request close].
        /// </summary>


        protected virtual bool CanExecuteClose()
        {
            return true;
        }

        /// <summary>
        /// Executes the close.
        /// </summary>
        protected void ExecuteClose()
        {
            OnRequestClose(new DialogResult());
        }




        protected string GetBaseAddress()
        {
            string enviorment = ConfigurationManager.AppSettings.Get("Environment");
            if (enviorment == "Production")
            {
                return ConfigurationManager.AppSettings.Get("HttpBaseAddress");

            }
            return ConfigurationManager.AppSettings.Get("LocalHttpBaseAddress");
        }




        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Saves the before closing asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public virtual Task<bool> SaveBeforeClosingAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public virtual string Caption { get; set; } = "Loading...";
        /// <summary>
        /// Gets or sets the caption image.
        /// </summary>
        /// <value>The caption image.</value>
        public virtual ImageSource CaptionImage { get; set; }


        /// <summary>
        /// Gets or sets the close command.
        /// </summary>
        /// <value>The close command.</value>
        public ICommand CloseCommand { get; set; }



        /// <summary>
        /// Gets the dispatcher service.
        /// </summary>
        /// <value>The dispatcher service.</value>
        public IDispatcherService DispatcherService => GetService<IDispatcherService>();


        /// <summary>
        /// Gets the message box service.
        /// </summary>
        /// <value>The message box service.</value>
        public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
        /// <summary>
        /// Gets the open file dialog service.
        /// </summary>
        /// <value>The open file dialog service.</value>
        public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
        /// <summary>
        /// Gets the save file dialog service.
        /// </summary>
        /// <value>The save file dialog service.</value>
        public ISaveFileDialogService SaveFileDialogService => GetService<ISaveFileDialogService>();
        /// <summary>
        /// Gets or sets a value indicating whether [show busy indicator].
        /// </summary>
        /// <value><c>true</c> if [show busy indicator]; otherwise, <c>false</c>.</value>
        public bool ShowBusyIndicator { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show validation summary].
        /// </summary>
        /// <value><c>true</c> if [show validation summary]; otherwise, <c>false</c>.</value>
        public virtual bool ShowValidationSummary { get; set; } = true;
        /// <summary>
        /// Gets the splash screen service.
        /// </summary>
        /// <value>The splash screen service.</value>
        public ISplashScreenService SplashScreenService => GetService<ISplashScreenService>();

        public ObservableCollection<ValidationFailure> ValidationSummary { get; set; }






        #region CreateLookupItemCommand


        /// <summary>
        /// Gets or sets the lookup item creation command.
        /// </summary>
        /// <value>The lookup item creation command.</value>
        public global::Prism.Commands.DelegateCommand<string> LookupItemCreationCommand { get; set; }


        /// <summary>
        /// Executes the lookup item creation.
        /// </summary>
        /// <param name="routeName"></param>
        /// <exception cref="RouteNotFoundException">Route attribute for LookupItemEdit is not found</exception>
        private async void ExecuteLookupItemCreation(string routeName)
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



        #endregion


        #region Prism IDialogAware

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            if (DevExpress.Xpf.Core.DXSplashScreen.IsActive)
                DevExpress.Xpf.Core.DXSplashScreen.Close();
        }

        public virtual string Title { get; set; }

        public WindowKind WindowKind { get; set; } = WindowKind.Ribbon;

        public event Action<IDialogResult> RequestClose;

        protected void OnRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }


        #endregion

    }
}