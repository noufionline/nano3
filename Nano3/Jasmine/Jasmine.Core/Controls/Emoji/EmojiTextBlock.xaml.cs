using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Jasmine.Core.Controls.Emoji
{
    /// <summary>
    /// Interaction logic for EmojiTextBlock.xaml
    /// </summary>
    public partial class TextBlock : System.Windows.Controls.TextBlock
    {
        static TextBlock()
        {
            TextProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(
                (string)System.Windows.Controls.TextBlock.TextProperty.GetMetadata(typeof(System.Windows.Controls.TextBlock)).DefaultValue,
                (o, e) => (o as TextBlock).OnTextChanged(e.NewValue as string)));

            ForegroundProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(
                (Brush)ForegroundProperty.GetMetadata(typeof(System.Windows.Controls.TextBlock)).DefaultValue,
                (o, e) => (o as TextBlock).OnForegroundChanged(e.NewValue as Brush)));

            //FontSizeProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(
            //    (double)FontSizeProperty.GetMetadata(typeof(System.Windows.Controls.TextBlock)).DefaultValue,
            //    (o, e) => (o as TextBlock).OnFontSizeChanged((double)e.NewValue)));
        }

        public TextBlock()
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// Override System.Windows.Controls.TextBlock.Text using our own dependency
        /// property. This is necessary because we do not want the parent callbacks
        /// to run, ever, and OverrideMetadata does not properly hide them.
        /// Also note that calling GetValue/SetValue is a lot faster when used directly
        /// on the DependencyPropertyDescriptor because it bypasses the reflection code.
        /// </summary>
        public new string Text
        {
            get => m_text_dpd.GetValue(this) as string;
            set => m_text_dpd.SetValue(this, value);
        }

        /// <summary>
        /// Override System.Windows.Controls.TextBlock.TextProperty
        /// </summary>
        public static new readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextBlock));

        
        private void OnTextChanged(String text)
        {
            Inlines.Clear();
            int pos = 0;
            foreach (Match m in EmojiData.MatchMultiple.Matches(text))
            {
                Inlines.Add(text.Substring(pos, m.Index - pos));
                Inlines.Add(new EmojiInline()
                {
                    FallbackBrush = Foreground,
                    Text = text.Substring(m.Index, m.Length),
                    FontSize = EmojiFontSize,
                });

                pos = m.Index + m.Length;
            }
            Inlines.Add(text.Substring(pos));
        }

        private void OnForegroundChanged(Brush brush)
        {
            foreach (var inline in Inlines)
                if (inline is EmojiInline)
                    (inline as EmojiInline).Foreground = brush;
        }

        //private void OnFontSizeChanged(double size)
        //{
        //    foreach (var inline in Inlines)
        //        if (inline is EmojiInline)
        //            (inline as EmojiInline).FontSize = size;
        //}

        private static readonly DependencyPropertyDescriptor m_text_dpd =
            DependencyPropertyDescriptor.FromProperty(TextProperty, typeof(TextBlock));






        public static readonly DependencyProperty EmojiFontSizeProperty = DependencyProperty.Register("EmojiFontSize", typeof(double), typeof(TextBlock), new FrameworkPropertyMetadata((double)FontSizeProperty.GetMetadata(typeof(System.Windows.Controls.TextBlock)).DefaultValue, new PropertyChangedCallback(OnEmojiFontSizeChanged), new CoerceValueCallback(OnCoerceEmojiFontSize)));


        private static object OnCoerceEmojiFontSize(DependencyObject o, object value)
        {
            TextBlock textBlock1 = o as TextBlock;
            if (textBlock1 != null)
                return textBlock1.OnCoerceEmojiFontSize((double)value);
            else
                return value;
        }

        private static void OnEmojiFontSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBlock textBlock1 = o as TextBlock;
            if (textBlock1 != null)
                textBlock1.OnEmojiFontSizeChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual double OnCoerceEmojiFontSize(double value)
        {            
            return value;
        }

        protected virtual void OnEmojiFontSizeChanged(double oldValue, double newValue)
        {            
            foreach (var inline in Inlines)
                if (inline is EmojiInline)
                    (inline as EmojiInline).FontSize = newValue;
        }

        public double EmojiFontSize
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (double)GetValue(EmojiFontSizeProperty);
            }
            set
            {
                SetValue(EmojiFontSizeProperty, value);
            }
        }

    }

    
}