using DevExpress.Mvvm.UI.Interactivity;
using Jasmine.Core.Contracts;
using System.Windows;
using System.Windows.Controls;

namespace Jasmine.Core.Behaviors
{
    public class CloseBehavior : Behavior<UserControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is FrameworkElement fe)
            {
                if (fe.DataContext is IAbsDialogAware vm)
                {
                    vm.RequestWindowClose += ViewModelRequestWindowClose;
                }
            }
        }

        void ViewModelRequestWindowClose()
        {
            Window parentWindow = Window.GetWindow(AssociatedObject);
            parentWindow?.Close();
        }


        protected override void OnDetaching()

        {

            if (AssociatedObject is FrameworkElement fe)
            {
                if (fe.DataContext is IAbsDialogAware vm)
                {
                    vm.RequestWindowClose -= ViewModelRequestWindowClose;
                    vm.OnClosed();
                }
            }
          
        }


    }
}