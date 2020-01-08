using BlankApp1.Reports;
using System.Diagnostics;
using System.Windows;

namespace BlankApp1.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data=Customer.GetCustomers();
            var report = new XtraReport1();
            report.DataSource=data;
            report.CreateDocument();
            report.ExportToPdf(@"C:\Users\Noufal\source\repos\nano3\Nano3\BlankApp1\Reports\Test.pdf");
            Process.Start(@"C:\Users\Noufal\source\repos\nano3\Nano3\BlankApp1\Reports\Test.pdf");
        }
    }
}
