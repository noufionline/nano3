using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Jasmine.Core.Notification
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Jasmine.Modules.CreditControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Jasmine.Modules.CreditControl;assembly=Jasmine.Modules.CreditControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:NotificationControl/>
    ///
    /// </summary>
    ///     
    public class NotificationControl : Control
    {
        static NotificationControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationControl), new FrameworkPropertyMetadata(typeof(NotificationControl)));
        }      

        #region TimeProperty

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(NotificationControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTimeChanged), new CoerceValueCallback(OnCoerceTime)));

        public string Time
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (string)GetValue(TimeProperty);
            }
            set
            {
                SetValue(TimeProperty, value);
            }
        }

        private static object OnCoerceTime(DependencyObject o, object value)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                return notificationControl.OnCoerceTime((string)value);
            else
                return value;
        }

        private static void OnTimeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                notificationControl.OnTimeChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceTime(string value)
        {            
            return value;
        }

        protected virtual void OnTimeChanged(string oldValue, string newValue)
        {
            
        }
        #endregion

        #region TypeProperty

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(string), typeof(NotificationControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTypeChanged), new CoerceValueCallback(OnCoerceType)));

        public string Type
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (string)GetValue(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }

        private static object OnCoerceType(DependencyObject o, object value)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                return notificationControl.OnCoerceType((string)value);
            else
                return value;
        }

        private static void OnTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                notificationControl.OnTypeChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceType(string value)
        {            
            return value;
        }

        protected virtual void OnTypeChanged(string oldValue, string newValue)
        {
         
        }
        #endregion

        #region MessageProperty

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(NotificationControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMessageChanged), new CoerceValueCallback(OnCoerceMessage)));

        public string Message
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (string)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        private static object OnCoerceMessage(DependencyObject o, object value)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                return notificationControl.OnCoerceMessage((string)value);
            else
                return value;
        }

        private static void OnMessageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                notificationControl.OnMessageChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceMessage(string value)
        {            
            return value;
        }

        protected virtual void OnMessageChanged(string oldValue, string newValue)
        {
         
        }
        #endregion

        #region ContentTemplateProperty

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(NotificationControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnContentTemplateChanged), new CoerceValueCallback(OnCoerceContentTemplate)));

        public DataTemplate ContentTemplate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
            }
        }

        private static object OnCoerceContentTemplate(DependencyObject o, object value)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                return notificationControl.OnCoerceContentTemplate((DataTemplate)value);
            else
                return value;
        }

        private static void OnContentTemplateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                notificationControl.OnContentTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual DataTemplate OnCoerceContentTemplate(DataTemplate value)
        {
            
            return value;
        }

        protected virtual void OnContentTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
            if (_content != null)
            {
                if (ContentTemplate != null)
                {                    
                    _content.ContentTemplate = newValue;
                }
                else
                {
                    _content.ContentTemplate = null;
                }
            }
            
            
        }
        #endregion

        #region FooterTemplateProperty

        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(NotificationControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnFooterTemplateChanged), new CoerceValueCallback(OnCoerceFooterTemplate)));

        public DataTemplate FooterTemplate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (DataTemplate)GetValue(FooterTemplateProperty);
            }
            set
            {
                SetValue(FooterTemplateProperty, value);
            }
        }

        private static object OnCoerceFooterTemplate(DependencyObject o, object value)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                return notificationControl.OnCoerceFooterTemplate((DataTemplate)value);
            else
                return value;
        }

        private static void OnFooterTemplateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NotificationControl notificationControl = o as NotificationControl;
            if (notificationControl != null)
                notificationControl.OnFooterTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual DataTemplate OnCoerceFooterTemplate(DataTemplate value)
        {
            
            return value;
        }

        protected virtual void OnFooterTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
            if (_footer != null)
            {
                if (FooterTemplate != null)
                {                    
                    _footer.ContentTemplate = newValue;
                }
                else
                {
                    _footer.ContentTemplate = null;
                }
            }
                        
        }
        #endregion

        ContentControl _content;
        ContentControl _footer;
        StackPanel _actionPanel;
        Button _expandButton;
        Button _collapseButton;
        Border _notificationPanel;

        protected Border NotificationPanel
        {
            get => _notificationPanel;
            set
            {
                if (_notificationPanel != null)
                {
                    _notificationPanel.MouseEnter -= _notificationPanel_MouseEnter;
                    _notificationPanel.MouseLeave -= _notificationPanel_MouseLeave;
                    _notificationPanel.MouseLeftButtonUp -= _notificationPanel_MouseLeftButtonUp;
                }

                _notificationPanel = value;

                if (_notificationPanel != null)
                {
                    _notificationPanel.MouseEnter += _notificationPanel_MouseEnter;
                    _notificationPanel.MouseLeave += _notificationPanel_MouseLeave;
                    _notificationPanel.MouseLeftButtonUp += _notificationPanel_MouseLeftButtonUp;
                }
                
            }
        }
        StackPanel _resizePanel;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            NotificationPanel = this.GetTemplateChild("PART_NotificationPanel") as Border;
            //if (_notificationPanel != null)
            //{
            //    _notificationPanel.MouseEnter -= _notificationPanel_MouseEnter;
            //    _notificationPanel.MouseLeave -= _notificationPanel_MouseLeave;
            //    _notificationPanel.MouseLeftButtonUp -= _notificationPanel_MouseLeftButtonUp;

            //    _notificationPanel.MouseEnter += _notificationPanel_MouseEnter;
            //    _notificationPanel.MouseLeave += _notificationPanel_MouseLeave;
            //    _notificationPanel.MouseLeftButtonUp += _notificationPanel_MouseLeftButtonUp;
            //}

            _resizePanel = this.GetTemplateChild("PART_ResizePanel") as StackPanel;            

            _content = this.GetTemplateChild("PART_NotificationContentPanel") as ContentControl;
            if (_content != null && ContentTemplate != null)
                _content.ContentTemplate = ContentTemplate;

            _footer = this.GetTemplateChild("PART_FooterContentPanel") as ContentControl;
            if (_footer != null && FooterTemplate != null)
                _footer.ContentTemplate = FooterTemplate;

            _actionPanel = this.GetTemplateChild("ActionPanel") as StackPanel;

            _expandButton = this.GetTemplateChild("PART_ExpandButton") as Button;
            if (_expandButton != null)
                _expandButton.Click += ExpandButton_Click;

            _collapseButton = this.GetTemplateChild("PART_CollapseButton") as Button;
            if (_collapseButton != null)
                _collapseButton.Click += CollapseButton_Click;

        }

        private void _notificationPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => ExpandMessage();
        private void _notificationPanel_MouseLeave(object sender, MouseEventArgs e) => _resizePanel.Visibility = Visibility.Hidden;
        private void _notificationPanel_MouseEnter(object sender, MouseEventArgs e) => _resizePanel.Visibility = Visibility.Visible;
        private void ExpandButton_Click(object sender, RoutedEventArgs e) => ExpandMessage();
        private void CollapseButton_Click(object sender, RoutedEventArgs e) => CollapseMessage();

        private void ExpandMessage()
        {
            if (_actionPanel.Visibility == Visibility.Collapsed)
            {
                _content.MaxHeight = Double.PositiveInfinity;
                _actionPanel.Visibility = Visibility.Visible;
                _expandButton.Visibility = Visibility.Collapsed;
                _collapseButton.Visibility = Visibility.Visible;
            }            
        }

        private void CollapseMessage()
        {
            if (_actionPanel.Visibility == Visibility.Visible)
            {
                _content.MaxHeight = 19;
                _actionPanel.Visibility = Visibility.Collapsed;
                _collapseButton.Visibility = Visibility.Collapsed;
                _expandButton.Visibility = Visibility.Visible;
            }            
        }
    }
}
