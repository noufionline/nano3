using System.Windows;

namespace Jasmine.Core.Controls
{
    /// <summary>
    /// Interaction logic for LookupItemList.xaml
    /// </summary>
    public partial class LookupItemEditSett
    {
        public LookupItemEditSett()
        {            
            
            InitializeComponent();
        }        

        public string Route
        {
            get => (string)GetValue(RouteProperty);
            set => SetValue(RouteProperty, value);
        }

        public static readonly DependencyProperty RouteProperty =
            DependencyProperty.Register("Route", typeof(string), typeof(LookupItemEditSett), new PropertyMetadata(string.Empty));

        public bool CanManage
        {
            get => (bool)GetValue(CanManageProperty);
            set => SetValue(CanManageProperty, value);
        }

        public static readonly DependencyProperty CanManageProperty =
            DependencyProperty.Register("CanManage", typeof(bool), typeof(LookupItemEditSett),new PropertyMetadata(false));
        
    }
}
