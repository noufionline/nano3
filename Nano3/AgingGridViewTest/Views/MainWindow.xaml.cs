using System;
using System.Diagnostics;
using System.Windows;
using DevExpress.Export;
using DevExpress.Xpf.PivotGrid;
using DevExpress.XtraPrinting;

namespace AgingGridViewTest.Views
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

        //private void PivotGridControl_OnCustomGroupInterval(object sender, PivotCustomGroupIntervalEventArgs e)
        //{
        //    if(!object.ReferenceEquals(e.Field, LastDay)) return;
        //    DateTime day = DateTime.Parse(e.Value.ToString());

        //    e.GroupValue = $"{day:MMM-yyyy}";
        //}

        private void CollapseAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            PartnerNamePivotGridField.CollapseAll();
        }

        private void PivotGrid_OnDataSourceChanged(object sender, RoutedEventArgs e)
        {
            PartnerNamePivotGridField.CollapseAll();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            PivotGrid.ExportToXlsx(@"text.xlsx",
                new PivotGridXlsxExportOptions(TextExportMode.Value){ExportType = ExportType.DataAware});
            Process.Start(@"C:\Users\Noufal\source\repos\nano3\Nano3\AgingGridViewTest\bin\Debug\text.xlsx");
        }
    }
}
