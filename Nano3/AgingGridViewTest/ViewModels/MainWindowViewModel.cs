using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Dapper;
using DevExpress.Mvvm;
using PostSharp.Patterns.Model;
using BindableBase = Prism.Mvvm.BindableBase;

namespace AgingGridViewTest.ViewModels
{
    [NotifyPropertyChanged]
    public class MainWindowViewModel
    {
        public string Title { get; set; }= "Prism Application";

        public MainWindowViewModel()
        {
            LoadDataCommand=new AsyncCommand(LoadDataAsync);
        }

        private async Task LoadDataAsync()
        {
            Items = new ObservableCollection<AgingData>(await AgingData.GetDataAsync());
            RecordCount = Items.Count;
        }


        public ObservableCollection<AgingData> Items { get; set; }=new ObservableCollection<AgingData>();
        public AsyncCommand LoadDataCommand { get; set; }

        public int RecordCount { get; set; } = 100;
    }


    public class AgingData
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string PartnerName { get; set; }
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DocumentDate { get; set; }

        public string MonthName { get; set; }
        public DateTime LastDayOfTheMonth { get; set; }





        public void Process()
        {
            var months = GetMonths(DocumentDate);
            var date =
                DateTime.Today.AddMonths(-months);
            if (months >= 16)
            {
                date = DateTime.Today.AddMonths(-16);
            }
            LastDayOfTheMonth = new DateTime(date.Year,date.Month,DateTime.DaysInMonth(date.Year,date.Month));

            switch (months)
            {
                case 0:
                    MonthName = "Current Month";
                    break;
                case 1:
                    MonthName = "Previous Month";
                    break;
                case 2:
                    MonthName = "30 Days";
                    break;
                case 3:
                    MonthName = "60 Days";
                    break;
                case 4:
                    MonthName = "90 Days";
                    break;
                case 5:
                    MonthName = "120 Days";
                    break;
                case 6:
                    MonthName = "150 Days";
                    break;
                case 7:
                    MonthName = "180 Days";
                    break;
                case 8:
                    MonthName = "210 Days";
                    break;
                case 9:
                    MonthName = "240 Days";
                    break;
                case 10:
                    MonthName = "270 Days";
                    break;
                case 11:
                    MonthName = "300 Days";
                    break;
                case 12:
                    MonthName = "330 Days";
                    break;
                case 13 :
                    MonthName = "360 Days";
                    break;
                case 14:
                    MonthName = "390 Days";
                    break;
                case 15:
                    MonthName = "420 Days";
                    break;
                case var d when d>=16   :
                    MonthName = "450 Days & Above";
                    break;




            }

        }


    
        

        private int GetMonths(DateTime date)
        {
            var years = DateTime.Today.Year - date.Year;
            var totalMonths = years * 12;

            var months = DateTime.Today.Month - date.Month;
            return (totalMonths + months);

        }

        public static async Task<List<AgingData>> GetDataAsync()
        {
            string connectionString = "Data Source=192.168.30.31;User ID=sa;Password=fkt;Initial Catalog=AbsCoreDevelopment";

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                string commandText=@"WITH GroupInfo (PartnerId,PartnerName, GroupName,AccountCode)
AS (SELECT PGM.PartnerId,
P.Name AS PartnerName,
           PG.Name AS GroupName,PGM.AccountCode
    FROM dbo.PartnerGroupMappings AS PGM
        INNER JOIN dbo.PartnerGroups AS PG
            ON PG.Id = PGM.PartnerGroupId INNER JOIN dbo.Partners AS P ON P.Id = PGM.PartnerId)
SELECT  SSUAI.AccountCode,
        SSUAI.AccountName,
		GI.PartnerName,
		GI.GroupName,
		SSUAI.DocumentDate,
        SUM(SSUAI.Amount) AS Amount  FROM dbo.SunSystemUnAllocatedInvoices AS SSUAI
		LEFT OUTER JOIN GroupInfo GI ON GI.AccountCode = SSUAI.AccountCode
		GROUP BY SSUAI.AccountCode,
                 SSUAI.AccountName,
                 GI.PartnerName,
                 GI.GroupName,
                 SSUAI.DocumentDate
";
                var items = (await con.QueryAsync<AgingData>(commandText)).ToList();
                foreach (var item in items)
                {
                    item.Process();
                }

                return items;
            }
        }
    }
}
