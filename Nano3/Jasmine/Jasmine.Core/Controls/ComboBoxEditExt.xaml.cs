using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Utils;
using PostSharp.Patterns.Model;
using Prism.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Jasmine.Core.Controls
{
    /// <summary>
    /// Interaction logic for ComboBoxEditExt.xaml
    /// </summary>
    
    public partial class ComboBoxEditExt : ComboBoxEdit,INotifyPropertyChanged
    {

        static ComboBoxEditExt()
        {
            Type ownerType = typeof(ComboBoxEditExt);

            var fpm = new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, true, UpdateSourceTrigger.PropertyChanged);

            EditValueProperty.OverrideMetadata(ownerType, fpm);

            RefreshCommandProperty = DependencyPropertyManager.Register(nameof(RefreshCommand), typeof(ICommand), ownerType,
               new PropertyMetadata(null));

            ManageCommandProperty = DependencyPropertyManager.Register(nameof(ManageCommand), typeof(ICommand), ownerType,
               new PropertyMetadata(null));

            CommandParameterProperty = DependencyPropertyManager.Register(nameof(CommandParameter), typeof(object), ownerType,
               new PropertyMetadata(null));
        }

        public ComboBoxEditExt() => InitializeComponent();

        //public string RouteName
        //{
        //    get => (string)GetValue(RouteNameProperty);
        //    set => SetValue(RouteNameProperty, value);
        //}

        //public static readonly DependencyProperty RouteNameProperty =
        //    DependencyProperty.Register("RouteName", typeof(string), typeof(ComboBoxEditExt));


        //public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        //    "Title", typeof(string), typeof(ComboBoxEditExt), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty RefreshCommandProperty;
        public static readonly DependencyProperty ManageCommandProperty;
        public static readonly DependencyProperty CommandParameterProperty;
        private Visibility _manageCommandVisibility;
        private Visibility _refreshCommandVisibility;

        public Visibility ManageCommandVisibility
        {
            get => _manageCommandVisibility;
            set
            {
                if (_manageCommandVisibility.Equals(value)) return;
                _manageCommandVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility RefreshCommandVisibility
        {
            get => _refreshCommandVisibility;
            set
            {
                if(_refreshCommandVisibility.Equals(value)) return;
                _refreshCommandVisibility = value;
                OnPropertyChanged();
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

         protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



}
