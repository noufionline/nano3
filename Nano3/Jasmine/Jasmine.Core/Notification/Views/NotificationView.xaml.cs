using Jasmine.Core.Contracts;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace Jasmine.Core.Notification.Views
{
    /// <summary>
    /// Interaction logic for NotificationView.xaml
    /// </summary>
    public partial class NotificationView : UserControl
    {        
        public NotificationView(IRegionManager regionManager, INotificationManagerService service, IEventAggregator eventAggregator)
        {            
            InitializeComponent();
            DataContext = new NotificationViewModel(regionManager, service, eventAggregator);
        }
    }
}
