using System.Windows.Input;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors;

namespace Jasmine.Core.Behaviors
{
    public class TextEditCaretIndexBehavior:Behavior<TextEdit>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
        }

        private void AssociatedObject_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextEdit textEdit) textEdit.CaretIndex = textEdit.Text.Length;
        }

        protected override void OnDetaching()

        {
            AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
            base.OnDetaching();
        }
    }
}