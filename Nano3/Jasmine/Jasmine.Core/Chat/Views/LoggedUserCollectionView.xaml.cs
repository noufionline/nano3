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

namespace Jasmine.Core.Chat.Views
{
    /// <summary>
    /// Interaction logic for LoggedUserView.xaml
    /// </summary>
    public partial class LoggedUserCollectionView : UserControl
    {
        public LoggedUserCollectionView()
        {
            InitializeComponent();
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            SystemSounds.Asterisk.Play();
        }
    }
    
}
