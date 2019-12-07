using Jasmine.Core.Contracts;
using Prism.Events;
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
using IDialogService=Prism.Services.Dialogs.IDialogService;
namespace Jasmine.Core.Notification.Views
{
    /// <summary>
    /// Interaction logic for ManageNotificationView.xaml
    /// </summary>
    public partial class ManageNotificationView : UserControl
    {
        public ManageNotificationView(IEventAggregator eventAggregator, IDialogService dialogService,
            INotificationManager notificationManager, IAuthorizationCache authorizationCache)
        {
            InitializeComponent();
            DataContext = new ManageNotificationViewModel(eventAggregator, dialogService, notificationManager, authorizationCache);
        }
    }
}
