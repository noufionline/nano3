// ***********************************************************************
// Assembly         : Jasmine.Core
// Author           : Noufal
// Created          : 12-06-2017
//
// Last Modified By : Noufal
// Last Modified On : 12-07-2017
// ***********************************************************************
// <copyright file="AsyncViewModelBase.cs" company="CICON">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using DevExpress.Mvvm;
using DevExpress.XtraReports.UI;
using FluentValidation.Results;
using Humanizer;
using Jasmine.Core.Aspects;
using Jasmine.Core.Audit;
using Jasmine.Core.Contracts;
using Jasmine.Core.Dialogs;
using Jasmine.Core.Events;
using Jasmine.Core.Mvvm.LookupItems;
using Jasmine.Core.Properties;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Model;
using Prism;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.XtraSpreadsheet.Import.Xls;
using Jasmine.Core.Common;
using Jasmine.Core.Tracking;
using Prism.Services.Dialogs;
using IDialogService = Prism.Services.Dialogs.IDialogService;
using PrismCommand = Prism.Commands.DelegateCommand<string>;
using Jasmine.Core.Helpers;

namespace Jasmine.Core.Mvvm
{

    /// <summary>
    /// Enum SaveMode
    /// </summary>
    public enum SaveMode
    {
        /// <summary>
        /// The save
        /// </summary>
        Save,
        /// <summary>
        /// The save and new
        /// </summary>
        SaveAndNew,
        /// <summary>
        /// The save and close
        /// </summary>
        SaveAndClose
    }

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
    public abstract class AsyncViewModelBase : DxMvvmServicesBase, IViewModelBase, IActiveAware, IAbsDialogAware,IDialogAware,
        IConfirmNavigationRequest, IRegionMemberLifetime, INotifyPropertyChanged
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IAuthorizationCache _authorizationCache;
        
        private IRegionNavigationJournal _journal;
        private bool _isActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncViewModelBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="authorizationCache"></param>
        protected AsyncViewModelBase(IEventAggregator eventAggregator, IDialogService dialogService,
            IAuthorizationCache authorizationCache)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _authorizationCache = authorizationCache;
            
      
            CloseCommand = new DelegateCommand(ExecuteClose, CanExecuteClose);
            LookupItemCreationCommand = new PrismCommand(ExecuteLookupItemCreation, CanExecuteLookupItemCreation);
            LookupItemRefreshCommand = new AsyncCommand<string>(ExecuteLookupItemRefreshAsync);
            LoadLookupItemsCommand = new AsyncCommand<string>(ExecuteLoadLookupItemsAsync);
            BackCommand = new DelegateCommand(ExecuteBack, CanExecuteBack);
            ForwardCommand = new DelegateCommand(ExecuteForward, CanExecuteForward);
        }

        
        public AsyncCommand<string> LoadLookupItemsCommand { get; set; }

        private  async Task ExecuteLoadLookupItemsAsync(string route)
        {
            await LoadLookupItemsAsync(route,false);
        }

        protected virtual Task LoadLookupItemsAsync(string route, bool invalidateCache)
        {
            return Task.CompletedTask;
        }
        ////protected virtual async Task<ObservableCollection<LookupItem>> UpdateLookupItemsAsync(string route, bool invalidateCache, ObservableCollection<LookupItem> items)
        ////{
        ////    if (invalidateCache==false)
        ////    {
        ////        var lookupItems = (await _lookupItemProviderService.GetLookupItemsAsync(route)).ToObservableList();
        ////        if (items?.Count != 1 || items.Count == 1 && 
        ////            lookupItems.Any(i => i.Id == items[0].Id && i.Name.Equals(items[0].Name)))
        ////        {
        ////            return lookupItems;
        ////        }
        ////    }
        ////    return (await _lookupItemProviderService.GetLookupItemsAsync(route, invalidateCache: true)).ToObservableList();
        ////}

        //protected virtual async Task<ObservableCollection<LookupItem>> GetLookupItemsAsync(string route,int id)
        //{
        //    return (await _lookupItemProviderService.GetLookupItemsAsync($"{route}/{id}", invalidateCache: true)).ToObservableList();
        //}
      
        // protected virtual async Task<ObservableCollection<TLookupItem>> UpdateLookupItemsAsync<TLookupItem>(
        //     string route, bool invalidateCache, ObservableCollection<TLookupItem> items) where TLookupItem:class,ILookupItem
        //{
        //    if (invalidateCache==false)
        //    {
        //        var lookupItems = (await _lookupItemProviderService.GetLookupItemsAsync<TLookupItem>(route)).ToObservableList();
        //        if (items?.Count != 1 || items.Count == 1 && 
        //            lookupItems.Any(i => i.Id == items[0].Id && i.Name.Equals(items[0].Name)))
        //        {
        //            return lookupItems;
        //        }
        //    }
        //    return (await _lookupItemProviderService.GetLookupItemsAsync<TLookupItem>(route,true)).ToObservableList();
        //}


       
        protected virtual async Task<ObservableCollection<TLookupItem>> UpdateLookupItemsAsync<TLookupItem>(
            Func<bool, Task<List<TLookupItem>>> predicate,
            bool invalidateCache,
            IEnumerable<TLookupItem> customers) where TLookupItem:ILookupItem
        {
            if (invalidateCache==false)
            {
                var items = (await predicate.Invoke(false)).ToObservableList();
                if (items?.Count != 1 || items.Count == 1 && 
                    items.Any(i => i.Id == items[0].Id && i.Name.Equals(items[0].Name)))
                {
                    return items;
                }
            }
            return (await predicate.Invoke(true)).ToObservableList();
        }
       
        /// <summary>
        /// Notifies that the value for <see cref="P:Prism.IActiveAware.IsActive" /> property has changed.
        /// </summary>
        public event EventHandler IsActiveChanged;


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
        public event Action RequestWindowClose;

        protected virtual bool CanExecuteClose() => true;

        /// <summary>
        /// Executes the close.
        /// </summary>
        protected void ExecuteClose()
        {
            ExecuteClose(true);
        }


        protected void ExecuteClose(bool dialogResult,bool checkCanClose=true)
        {
            if (checkCanClose && CanCloseDialog() == false) return;
            
            ClosingFromViewModel = true;
            RequestWindowClose?.Invoke();
            DialogResult = dialogResult;

            OnRequestClose(new DialogResult(dialogResult==true ? ButtonResult.OK : ButtonResult.Cancel));
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
        /// Called when [is active changed].
        /// </summary>
        protected virtual void OnIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this,EventArgs.Empty);
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
        /// Determines whether this instance [can close dialog].
        /// </summary>
        /// <returns><c>true</c> if this instance [can close dialog]; otherwise, <c>false</c>.</returns>
        public virtual bool CanCloseDialog() => true;

   

        /// <summary>
        /// Determines whether this instance accepts being navigated away from.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <param name="continuationCallback">The callback to indicate when navigation can proceed.</param>
        /// <remarks>Implementors of this method do not need to invoke the callback before this method is completed,
        /// but they must ensure the callback is eventually invoked.</remarks>
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext,
            Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        /// <summary>
        /// Gets the dialog options.
        /// </summary>
        /// <returns>DialogOptions.</returns>
        public virtual DialogOptions GetDialogOptions()
        {
            return null;
        }

        /// <summary>
        /// Called to determine if this instance can handle the navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns><see langword="true" /> if this instance accepts the navigation request; otherwise, <see langword="false" />.</returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }


        /// <summary>
        /// Called when [closed].
        /// </summary>
        public virtual void OnClosed()
        {

        }

        /// <summary>
        /// Called when the implementer is being navigated away from.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        /// <summary>
        /// Called when the implementer has been navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            Journal = navigationContext.NavigationService.Journal;
            if (navigationContext.Parameters.ContainsKey("caption"))
            {
                Caption = navigationContext.Parameters.GetValue<string>("caption");
            }
            if (navigationContext.Parameters.TryGetValue("captionimage", out ImageSource captionImage))
                CaptionImage = captionImage;

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
        public virtual string Caption { get; set; }
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
        /// Gets or sets a value indicating whether [closing from view model].
        /// </summary>
        /// <value><c>true</c> if [closing from view model]; otherwise, <c>false</c>.</value>
        public bool ClosingFromViewModel { get; protected set; }

        //public virtual bool SaveBeforeClosingRequested { get; set; }

        /// <summary>
        /// Gets a value indicating whether [dialog result].
        /// </summary>
        /// <value><c>null</c> if [dialog result] contains no value, <c>true</c> if [dialog result]; otherwise, <c>false</c>.</value>
        public bool? DialogResult { get; private set; }
        /// <summary>
        /// Gets the dispatcher service.
        /// </summary>
        /// <value>The dispatcher service.</value>
        public IDispatcherService DispatcherService => GetService<IDispatcherService>();

        /// <summary>
        /// Gets or sets a value indicating whether the object is active.
        /// </summary>
        /// <value><see langword="true" /> if the object is active; otherwise <see langword="false" />.</value>
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

        public IRegionNavigationJournal Journal
        {
            get => _journal;
            private set
            {
                _journal = value;
                BackCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance should be kept-alive upon deactivation.
        /// </summary>
        /// <value><c>true</c> if [keep alive]; otherwise, <c>false</c>.</value>
        public virtual bool KeepAlive => false;

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



        #region CreateLookupItemCommand


        /// <summary>
        /// Gets or sets the lookup item creation command.
        /// </summary>
        /// <value>The lookup item creation command.</value>
        public PrismCommand LookupItemCreationCommand { get; set; }


        /// <summary>
        /// Executes the lookup item creation.
        /// </summary>
        /// <param name="routeName"></param>
        /// <exception cref="RouteNotFoundException">Route attribute for LookupItemEdit is not found</exception>
        private void ExecuteLookupItemCreation(string routeName)
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new RouteNotFoundException("Route attribute for LookupItemEdit is not found");
            var parameter = new DialogParameters { { "routeName", routeName } };
            _dialogService.ShowDialog("LookupItemView", parameter);
            ExecuteLookupItemRefreshAsync(routeName).ContinueWith(task =>
                _eventAggregator.GetEvent<RefreshLookupItemEvent>().Publish(routeName),TaskScheduler.FromCurrentSynchronizationContext());
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
            return LoadLookupItemsAsync(route, true);
        }
        #endregion


        #region Prism IDialogAware

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
        protected virtual void OnRequestClose(IDialogResult obj)
        {
            RequestClose?.Invoke(obj);
        }

        #endregion
    }

    
}