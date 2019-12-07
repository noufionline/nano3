using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Ribbon;
using Jasmine.Core.Contracts;
using Jasmine.Core.Mvvm;

namespace Jasmine.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for DXRibbonChildWindow.xaml
    /// </summary>
    public partial class DxRibbonChildWindow : IDialog
    {

        #region Members

        IDialogAware _viewModel;

        #endregion //Members
        public DxRibbonChildWindow()
        {
            RunTypeInitializers(Assembly.GetAssembly(typeof(LayoutHelper)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(TextEdit)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(ComboBoxEdit)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(DateEdit)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(SpinEdit)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(ColorEdit)));
            //RunTypeInitializers(Assembly.GetAssembly(typeof(RichEditControl)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(LookUpEdit)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(DockLayoutManager)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(BarManager)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(TreeListControl)));
            RunTypeInitializers(Assembly.GetAssembly(typeof(LayoutControl)));

            ThemeManager.SetTheme(new TextEdit(), Theme.Office2016White);
            ThemeManager.SetTheme(new ComboBoxEdit(), Theme.Office2016White);
            ThemeManager.SetTheme(new DateEdit(), Theme.Office2016White);
            ThemeManager.SetTheme(new SpinEdit(), Theme.Office2016White);
            ThemeManager.SetTheme(new ColorEdit(), Theme.Office2016White);
            //ThemeManager.SetTheme(new RichEditControl(), Theme.Office2016White);
            ThemeManager.SetTheme(new LookUpEdit(), Theme.Office2016White);
            ThemeManager.SetTheme(new DockLayoutManager(), Theme.Office2016White);
            ThemeManager.SetTheme(new BarManager(), Theme.Office2016White);
            ThemeManager.SetTheme(new TreeListControl(), Theme.Office2016White);
            ThemeManager.SetTheme(new LayoutControl(), Theme.Office2016White);

            InitializeComponent();
            Closed += RibbonDialog_Closed;
            Closing += RibbonDialog_Closing;
            SizeChanged += RibbonDialog_Resized;
            PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape) Close();
            };

            Loaded += MainWindow_Loaded;
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutHelper.FindElementByName(this, "PART_Minimize").IsHitTestVisible = false;
            LayoutHelper.FindElementByName(this, "PART_Minimize").Opacity = 0;




        }


        private static void RunTypeInitializers(Assembly a)
        {
            Type[] types = a.GetExportedTypes();
            foreach (Type type in types)
                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }


        private void BarItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            FlyoutControl.PlacementTarget = e.Link.LinkControl;
            FlyoutControl.IsOpen = true;
        }

        #region IDialog

        public new void Show()
        {
            base.Show();



            CalculateDialogWindowsPosition();
        }



        public new bool? ShowDialog()
        {



            CalculateDialogWindowsPosition();
           bool? result= base.ShowDialog();
           return _viewModel?.DialogResult ?? result;
        }


        #endregion //IDialog

        #region Event Handlers

        void ViewModel_RequestClose()
        {
            Close();
        }

        void RibbonDialog_Closing(object sender, CancelEventArgs e)
        {
            if (_viewModel != null && !_viewModel.CanCloseDialog())
                e.Cancel = true;
        }

        void RibbonDialog_Closed(object sender, EventArgs e)
        {
            if (_viewModel != null)
                _viewModel.RequestClose -= ViewModel_RequestClose;
        }

        private void RibbonDialog_Resized(object sender, SizeChangedEventArgs e)
        {
            DXRibbonWindow window = ((DXRibbonWindow)sender);
            window.Top = (Application.Current.MainWindow.Height - e.NewSize.Height) / 2;
            window.Left = (Application.Current.MainWindow.Width - e.NewSize.Width) / 2;
        }

        #endregion //Event Handlers

        #region Methods

        private void CalculateDialogWindowsPosition()
        {
            WindowCollection windows = Application.Current.Windows;

            int x = 0;
            foreach (object window in windows)
            {
                if (window is IDialog)
                {
                    if (window == this)
                    {
                        if (x > 1) //don't want the Shell
                        {
                            for (int i = x - 1; i > 0; i--)
                            {
                                if (windows[i] is IDialog)
                                {
                                    Window priorWindow = windows[i];
                                    Left = priorWindow.Left - 40;
                                    Top = priorWindow.Top + 40;
                                    break;
                                }
                            }

                        }

                        break;

                    }
                }

                x++;
            }
        }


        #endregion //Methods


    }
}
