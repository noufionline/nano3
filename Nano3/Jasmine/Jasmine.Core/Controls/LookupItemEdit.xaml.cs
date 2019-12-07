using System.Windows;
using System.Windows.Data;

namespace Jasmine.Core.Controls
{
    /// <summary>
    /// Interaction logic for LookupItemList.xaml
    /// </summary>
    public partial class LookupItemEdit
    {

        static LookupItemEdit()
        {
            var fpm = new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, true, UpdateSourceTrigger.PropertyChanged);
            EditValueProperty.OverrideMetadata(typeof(LookupItemEdit), fpm);
        }
        public LookupItemEdit() => InitializeComponent();

        public string Route
        {
            get => (string)GetValue(RouteProperty);
            set => SetValue(RouteProperty, value);
        }

        public static readonly DependencyProperty RouteProperty =
            DependencyProperty.Register("Route", typeof(string), typeof(LookupItemEdit), new PropertyMetadata(string.Empty));

        public string View
        {
            get => (string)GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }

        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(string), typeof(LookupItemEdit), new PropertyMetadata("LookupItemView"));

        public (string, string) Lookup { get { return (Route, View); } }

        public bool CanManage
        {
            get => (bool)GetValue(CanManageProperty);
            set => SetValue(CanManageProperty, value);
        }

        public static readonly DependencyProperty CanManageProperty =
            DependencyProperty.Register("CanManage", typeof(bool), typeof(LookupItemEdit), new PropertyMetadata(false));

    }
}
