using DevExpress.Xpf.Utils;
using Prism.Commands;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Jasmine.Core.Properties;
using DevExpress.Xpf.Grid.LookUp;

namespace Jasmine.Core.Controls
{

 
    public partial class LookupItemEditor: INotifyPropertyChanged
    {
        static LookupItemEditor()
        {
            Type ownerType = typeof(LookupItemEditor);

            var fpm = new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, true, UpdateSourceTrigger.PropertyChanged);

            EditValueProperty.OverrideMetadata(typeof(LookupItemEditor), fpm);

            RefreshCommandProperty = DependencyPropertyManager.Register(nameof(RefreshCommand), typeof(ICommand), ownerType,
               new PropertyMetadata(null));

            ManageCommandProperty = DependencyPropertyManager.Register(nameof(ManageCommand), typeof(ICommand), ownerType,
               new PropertyMetadata(null));

            CommandParameterProperty = DependencyPropertyManager.Register(nameof(CommandParameter), typeof(object), ownerType,
               new PropertyMetadata(null));

            AutoSelectProperty = DependencyProperty.Register("AutoSelect", typeof(bool), ownerType, new PropertyMetadata(false));
        }

        public LookupItemEditor()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty RefreshCommandProperty;
        public static readonly DependencyProperty ManageCommandProperty;
        public static readonly DependencyProperty CommandParameterProperty;
        public static readonly DependencyProperty AutoSelectProperty;
        private Visibility _manageCommandVisibility;
        private Visibility _refreshCommandVisibility;


        public Visibility ManageCommandVisibility
        {
            get => _manageCommandVisibility;
            set
            {
                _manageCommandVisibility = value; 
                OnPropertyChanged();
            }
        }

        public Visibility RefreshCommandVisibility
        {
            get => _refreshCommandVisibility;
            set
            {
                _refreshCommandVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool AutoSelect
        {
            get => (bool)GetValue(AutoSelectProperty);
            set => SetValue(AutoSelectProperty, value);
        }

        public ICommand RefreshCommand
        {
            get => (ICommand)GetValue(RefreshCommandProperty);
            set => SetValue(RefreshCommandProperty, value);
        }

        public ICommand ManageCommand
        {
            get => (ICommand)GetValue(ManageCommandProperty);
            set => SetValue(ManageCommandProperty, value);
        }

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override void OnPopupOpened()
        {
            base.OnPopupOpened();

            #region Manage Command

            if (ManageCommand is DelegateCommand<string> manageCommand)
            {
                ManageCommandVisibility = Visibility.Visible;
                manageCommand.RaiseCanExecuteChanged();
            }
            else if (ManageCommand is DevExpress.Mvvm.DelegateCommand<string> devexpressManageCommand)
            {
                ManageCommandVisibility = Visibility.Visible;
                devexpressManageCommand.RaiseCanExecuteChanged();
            }
            else if (ManageCommand is DevExpress.Mvvm.AsyncCommand<string> devexpressAsyncManageCommand)
            {
                ManageCommandVisibility = Visibility.Visible;
                devexpressAsyncManageCommand.RaiseCanExecuteChanged();
            }
            else
            {
                ManageCommandVisibility = Visibility.Collapsed;
            }
            #endregion

            #region Refresh Command

            if (RefreshCommand is DelegateCommand<string> refreshCommand)
            {
                RefreshCommandVisibility = Visibility.Visible;
                refreshCommand.RaiseCanExecuteChanged();
            }
            else if (RefreshCommand is DevExpress.Mvvm.DelegateCommand<string> devexpressRefreshCommand)
            {
                RefreshCommandVisibility = Visibility.Visible;
                devexpressRefreshCommand.RaiseCanExecuteChanged();
            }
            else if (RefreshCommand is DevExpress.Mvvm.AsyncCommand<string> devexpressAsyncRefreshCommand)
            {
                RefreshCommandVisibility = Visibility.Visible;
                devexpressAsyncRefreshCommand.RaiseCanExecuteChanged();
            }
            else
            {
                RefreshCommandVisibility = Visibility.Collapsed;
            }
            #endregion
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (ItemsSource is IList items && items.Count == 1)
                SelectedIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

         protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}