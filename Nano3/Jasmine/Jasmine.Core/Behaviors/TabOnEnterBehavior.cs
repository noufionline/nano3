using DevExpress.Mvvm.UI.Interactivity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jasmine.Core.Behaviors
{
    public class TabOnEnterBehavior : Behavior<UserControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += View_KeyDown;
        }


        protected override void OnDetaching()

        {
            AssociatedObject.KeyDown -= View_KeyDown;
            base.OnDetaching();
        }


        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;
                keyboardFocus?.MoveFocus(tRequest);
                e.Handled = true;
            }
        }
    }
}