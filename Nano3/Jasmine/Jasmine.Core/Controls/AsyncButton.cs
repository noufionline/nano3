using DevExpress.Xpf.Core;
using System.Windows;

namespace Jasmine.Core.Controls
{
    public class AsyncButton : SimpleButton
    {
        static AsyncButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(AsyncButton),
                new FrameworkPropertyMetadata(typeof(AsyncButton)));

        public DataTemplate GlyphTemplate
        {
            get => (DataTemplate)GetValue(GlyphTemplateProperty);
            set => SetValue(GlyphTemplateProperty, value);
        }

        public static readonly DependencyProperty GlyphTemplateProperty =
            DependencyProperty.Register("GlyphTemplate", typeof(DataTemplate), typeof(AsyncButton),
                new PropertyMetadata(null));
    }
}