using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Spreadsheet;
using ExcelDataReader;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;
using Workbook = DevExpress.Spreadsheet.Workbook;
using Worksheet = DevExpress.Spreadsheet.Worksheet;

namespace ExpenseScheduler.ViewModels
{
    [NotifyPropertyChanged]
    public class MainWindowViewModel
    {

        public string Title { get; set; } = "Expense Schedule";

        public MainWindowViewModel()
        {
            Divisions = new List<DivisionInfo>()
            {
                new DivisionInfo {Database = "CES",Id=1,Name="Cut & Bend Musaffah",Codes=new[]{101}}, 
                new DivisionInfo {Database = "CES",Id=2,Name="Coupler  Musaffah",Codes=new[]{102}}, 
                new DivisionInfo {Database = "CES",Id=3,Name="Epoxy",Codes=new[]{120}}, 
                new DivisionInfo {Database = "CES",Id=4,Name="Wire Mesh",Codes=new[]{110}}, 
                new DivisionInfo {Database = "CES",Id=5,Name="Cage",Codes=new[]{111}}, 
                new DivisionInfo {Database = "CES",Id=6,Name="Melting",Codes=new[]{112}}, 
                new DivisionInfo {Database = "CES",Id=7,Name="Sign & Metal",Codes=new[]{125,126,127}}, 
                new DivisionInfo {Database = "CES",Id=8,Name="Fabrication",Codes=new[]{130}}, 
                new DivisionInfo {Database = "CSF",Id=9,Name="Cut & Bend F4",Codes=new[]{100}}, 
                new DivisionInfo {Database = "CSF",Id=10,Name="Coupler F4",Codes=new[]{101}}, 
                new DivisionInfo {Database = "CSF",Id=11,Name="Cut & Bend UAQ",Codes=new[]{102}}, 
                new DivisionInfo {Database = "CSF",Id=12,Name="Coupler UAQ",Codes=new[]{103}}, 
                new DivisionInfo {Database = "MAX",Id=13,Name="Cut & Bend Maxsteel",Codes=new[]{104,320600}}, 
                new DivisionInfo {Database = "MAX",Id=14,Name="Coupler Maxsteel",Codes=new[]{105,320601}}, 

            };


            ProcessCommand=new AsyncCommand(ExecuteProcess);
        }

        private List<OpeningStock> _openingStocks=new List<OpeningStock>();

        public string FilePath { get; set; } = @"C:\Users\Noufal\Downloads\VisionReportData.xlsx";

        public List<DivisionInfo> Divisions { get; set; }
        
        public AsyncCommand ProcessCommand { get; set; }

        private async Task ExecuteProcess()
        {

            _openingStocks = OpeningStock.GetOpeningStock(FilePath, Divisions);
            var items = ExpenseSchedule.ProcessExpenseSchedules(FilePath,Criteria.Date);
            await CreateExpenseScheduleWorkbook(items);
        }

        public Criteria Criteria { get; set; } = new Criteria(){Date = DateTime.Today};





        public async Task CreateExpenseScheduleWorkbook(List<ExpenseSchedule> items)
        {
            var outputPath = @"C:\Users\Noufal\Downloads\ExpenseScheduleSample.xlsx";

            var catdic = new Dictionary<string, int>
            {
                { "Cost of Steel/Couplers/Raw Materials/Consumable", 1 },
                { "Subcontracting & Other Charges", 2 },
                { "BBS Charges", 3 },
                { "Logistics Charges", 4 },
                { "Direct Costs - Man Power", 5 },
                { "Direct Costs - Others", 6 },
                { "Gen. & Admin. - Man Power", 7 },
                { "Gen. & Admin. - Others", 8 },
                { "Depreciation", 10 }
            };


            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(item.Category))
                {
                    if (catdic.TryGetValue(item.Category, out int seq))
                    {
                        item.CatSeq = seq;
                    }
                }
            }

            var path = GetExportPath($"ExpenseSchedule-{DateTime.Now:ddMMyyyyHHmmss}.xlsx");

            await  Task.Factory.StartNew(() =>
            {
                // Create a new workbook.
                Workbook workbook = new Workbook
                {
                    Unit = DevExpress.Office.DocumentUnit.Point
                };

                // Access the first worksheet in the workbook.
                Worksheet worksheet = workbook.Worksheets[0];


                foreach (var division in Divisions)
                {

                    var closingStockDic =_openingStocks
                        .Where(x=> x.DivisionId==division.Id)
                        .Select(x=> new { x.TransactionDate,x.Amount }).ToDictionary(k=> Convert.ToInt32(k.TransactionDate.AddMonths(-1).ToString("yyyyMM")),v=> v.Amount);

                    PrepareSummaryWorksheetByDivision(workbook.Worksheets.Add(),Criteria.Date,division,items,_openingStocks);

                    PrepareDetailedWorksheet(workbook.Worksheets.Add(),items,Criteria.Date,division,closingStockDic);
                }


                workbook.SaveDocument(outputPath, DocumentFormat.OpenXml);
            });

           

            Process.Start(outputPath, @"/r");

        }



        private void PrepareSummaryWorksheetByDivision(Worksheet worksheet,DateTime date,DivisionInfo division,List<ExpenseSchedule> items,List<OpeningStock> openingStock)
        {
             worksheet.ActiveView.ShowGridlines = false;
            worksheet.Name = $"{division.Name}(Summary)";

            worksheet.Cells.Font.Name = "Calibri";

            worksheet.Cells["A1"].SetValue(division.Name);
            worksheet.Cells["A1"].Font.Size = 20;
            worksheet.Cells["A1"].Font.Bold = true;
            worksheet.Cells["A1"].Font.Color = Color.Red;

            worksheet.Range["J1:N1"].SetValue($"Printed On: {DateTime.Now:dd-MMM-yyyy hh:mm tt}");
            worksheet.Range["J1:N1"].Font.Size = 10;
            worksheet.Range["J1:N1"].Font.Bold = true;
            worksheet.Range["J1:N1"].Font.Color = Color.DarkBlue;
            worksheet.Range["J1:N1"].Merge();
            worksheet.Range["J1:N1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
            worksheet.Range["J1:N1"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;


            worksheet.Cells["A2"].SetValue("DETAILED EXPENSES SCHEDULE");


            worksheet.Cells["A2"].Font.Color = Color.DarkSlateBlue;


            worksheet.Cells["A3"].SetValue("Descriptions");
            worksheet.Columns[0].WidthInPixels = 400;
            for (int i = 1; i <= 12; i++)
            {
                var monthName = new DateTime(Criteria.Date.Year, i, 1).ToString("MMM-yy", CultureInfo.InvariantCulture);
                worksheet.Rows[2].RowHeight = 30;
                worksheet.Rows[2][i].SetValue(monthName);
                worksheet.Rows[3][i].SetValue("AED");
            }

            worksheet.Cells["N3"].SetValue("Total");
            worksheet.Cells["N4"].SetValue("AED");

            worksheet.Range["A3:A4"].Merge();



            var headerRange = worksheet.Range["A3:N4"];
            headerRange.FillColor = Color.WhiteSmoke;
            headerRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            headerRange.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            headerRange.Font.Bold = true;
            headerRange.Borders.SetAllBorders(Color.DarkGray, BorderLineStyle.Thin);

            int year = date.Year;

            int currentRow = 4;
            var details = items.Where(i => division.Codes.Contains(i.DivisionId) && i.Database == division.Database).ToList();

            var dic = new Dictionary<string, (int Year, int Month)>();
            for (int i = 1; i <= 12; i++)
            {
                dic.Add($"{year}/{i:000}", (year, i));
            }

            var summary = GetSummary(details);

          
            for (int i = 0; i < summary.Count; i++)
            {
                worksheet.Rows[currentRow].RowHeight = 30;
                if (summary.Count > 1)
                {
                    worksheet.Rows[currentRow][0].SetValue(summary[i].Category?.ToUpper());
                    worksheet.Rows[currentRow][0].Font.Bold = true;
                }

                worksheet.Rows[currentRow][1].SetValue(summary[i].Jan);
                worksheet.Rows[currentRow][2].SetValue(summary[i].Feb);
                worksheet.Rows[currentRow][3].SetValue(summary[i].Mar);
                worksheet.Rows[currentRow][4].SetValue(summary[i].Apr);
                worksheet.Rows[currentRow][5].SetValue(summary[i].May);
                worksheet.Rows[currentRow][6].SetValue(summary[i].Jun);
                worksheet.Rows[currentRow][7].SetValue(summary[i].Jul);
                worksheet.Rows[currentRow][8].SetValue(summary[i].Aug);
                worksheet.Rows[currentRow][9].SetValue(summary[i].Sep);
                worksheet.Rows[currentRow][10].SetValue(summary[i].Oct);
                worksheet.Rows[currentRow][11].SetValue(summary[i].Nov);
                worksheet.Rows[currentRow][12].SetValue(summary[i].Dec);

                worksheet.Rows[currentRow][13].Formula = $"=SUM(B{currentRow + 1}:M{currentRow + 1})";
                worksheet.Rows[currentRow][13].Font.Bold = true;
                currentRow++;
            }

            worksheet.Rows[currentRow].RowHeight = 30;

            worksheet.Range[$"A3:N{currentRow}"].Borders.SetAllBorders(Color.DarkGray, BorderLineStyle.Thin);
            worksheet.Range[$"B5:N{currentRow}"].NumberFormat = "_(* #,##0_);_(* (#,##0);_(* -??_);_(@_)";

            DevExpress.Spreadsheet.CellRange printArea = worksheet[$"A1:N{currentRow}"];
            worksheet.SetPrintRange(printArea);

            worksheet.PrintOptions.FitToPage = true;
            worksheet.PrintOptions.FitToWidth = 1;
            worksheet.PrintOptions.FitToHeight = 1;

            worksheet.ActiveView.Orientation = PageOrientation.Landscape;
        }

     
        private void PrepareDetailedWorksheet(Worksheet worksheet, IEnumerable<ExpenseSchedule> items,DateTime date,DivisionInfo division,Dictionary<int,decimal> closingStockDic)
        {
            // Access the first worksheet in the workbook.

    

            var printOptions = worksheet.PrintOptions;
            printOptions.PrintTitles.SetRows(0, 3);
            printOptions.Scale = 70;
            printOptions.FitToWidth = 1;
            printOptions.CenterHorizontally = true;

            //printOptions.CenterVertically=true;
            worksheet.ActiveView.PaperKind = System.Drawing.Printing.PaperKind.A4;
            worksheet.ActiveView.Orientation = PageOrientation.Landscape;


            WorksheetHeaderFooterOptions options = worksheet.HeaderFooterOptions;

            // Specify that the first page has a unique header and footer.
            options.DifferentFirst = true;
            // Insert the current date into the footer's left section.
            options.FirstFooter.Left = "&D &T";
            options.OddFooter.Left = "&D &T";
            options.EvenFooter.Left = "&D &T";
            // Insert the current page number into the footer's right section.
            options.FirstFooter.Right = $"Page {"&P"} of {"&N"}";
            options.OddFooter.Right = $"Page {"&P"} of {"&N"}";
            options.EvenFooter.Right = $"Page {"&P"} of {"&N"}";


            worksheet.Name = $"{division.Name}(Detailed)";
            worksheet.ActiveView.ShowGridlines = false;
            worksheet.Cells.Font.Name = "Calibri";

            worksheet.Cells["B1"].SetValue(division.Name);
            worksheet.Cells["B1"].Font.Size = 20;
            worksheet.Cells["B1"].Font.Bold = true;
            worksheet.Cells["B1"].Font.Color = Color.Red;


            worksheet.Range["J1:O1"].SetValue($"Printed On: {DateTime.Now:dd-MMM-yyyy hh:mm tt}");
            worksheet.Range["J1:O1"].Font.Size = 10;
            worksheet.Range["J1:O1"].Font.Bold = true;
            worksheet.Range["J1:O1"].Font.Color = Color.DarkBlue;
            worksheet.Range["J1:O1"].Merge();
            worksheet.Range["J1:O1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
            worksheet.Range["J1:O1"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

            worksheet.Cells["B2"].SetValue("DETAILED EXPENSES SCHEDULE");
            worksheet.Cells["B2"].Font.Size = 20;
            worksheet.Cells["B2"].Font.Bold = true;
            worksheet.Cells["B2"].Font.Color = Color.DarkSlateBlue;

            worksheet.Cells["M2"].SetValue("Processed For:");
            worksheet.Range["M2:N2"].Merge();

            worksheet.Cells["O2"].SetValue(new DateTime(Criteria.Date.Year, Criteria.Date.Month, 1));
            worksheet.Cells["O2"].NumberFormat = "MMM-yyyy";


            worksheet.Range["M2:O2"].Font.Bold = true;
            worksheet.Range["M2:O2"].FillColor = Color.WhiteSmoke;
            worksheet.Range["M2:O2"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            worksheet.Range["M2:O2"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            worksheet.Range["M2:O2"].Borders.SetAllBorders(Color.DarkGray, BorderLineStyle.Thin);
            worksheet.Range["M2:O2"].Borders.BottomBorder.LineStyle = BorderLineStyle.Double;


            worksheet.Cells["A3"].SetValue("Account Code");
            worksheet.Cells["A3"].ColumnWidth = 100;
            worksheet.Cells["B3"].SetValue("Descriptions");
            worksheet.Columns[1].WidthInPixels = 400;
            for (int i = 2; i <= 13; i++)
            {
                var monthName = new DateTime(Criteria.Date.Year, i - 1, 1).ToString("MMM-yy", CultureInfo.InvariantCulture);
                worksheet.Rows[2].RowHeight = 30;
                worksheet.Rows[2][i].SetValue(monthName);
                worksheet.Rows[3][i].SetValue("AED");
            }

            worksheet.Cells["O3"].SetValue("Total");
            worksheet.Cells["O4"].SetValue("AED");

            worksheet.Range["A3:A4"].Merge();
            worksheet.Range["B3:B4"].Merge();


            var headerRange = worksheet.Range["A3:O4"];
            headerRange.FillColor = Color.WhiteSmoke;
            headerRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            headerRange.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            headerRange.Font.Bold = true;
            headerRange.Borders.SetAllBorders(Color.DarkGray, BorderLineStyle.Thin);

            int month = Criteria.Date.Month;
            var currentMonthColumn = string.Empty;
            switch (month)
            {
                case 1: currentMonthColumn = "C"; break;
                case 2: currentMonthColumn = "D"; break;
                case 3: currentMonthColumn = "E"; break;
                case 4: currentMonthColumn = "F"; break;
                case 5: currentMonthColumn = "G"; break;
                case 6: currentMonthColumn = "H"; break;
                case 7: currentMonthColumn = "I"; break;
                case 8: currentMonthColumn = "J"; break;
                case 9: currentMonthColumn = "K"; break;
                case 10: currentMonthColumn = "L"; break;
                case 11: currentMonthColumn = "M"; break;
                case 12: currentMonthColumn = "N"; break;
            }

            //worksheet.FreezeRows(3);

            var currentRow = 4;
            var categories = items
            .Where(i => division.Codes.Contains(i.DivisionId) && i.Database == division.Database)
            .GroupBy(x => new { x.CatSeq, x.Category })
            .OrderBy(x => x.Key.CatSeq)
            .Select(x => new { Name = x.Key.Category, Items = x.ToList() }).ToList();

            var summaryInfo = new List<CategorySummaryInfo>();

            foreach (var category in categories)
            {
                //TODO design a table to create mapping for Category and Sub Category Names with Custom names

                if (!string.IsNullOrWhiteSpace(category.Name) && category.Name.ToLower().StartsWith("cost of steel"))
                {
                    worksheet.Rows[currentRow][1].SetValue("Cost of Raw Materials & Consumables");
                }
                else
                {
                    worksheet.Rows[currentRow][1].SetValue(category.Name ?? "N/A");
                }

                worksheet.Rows[currentRow][1].Font.Size = 16;
                worksheet.Rows[currentRow][1].Font.Color = Color.Maroon;
                worksheet.Rows[currentRow][1].Font.Bold = true;
                int startRow = currentRow + 1;
                summaryInfo.Add(new CategorySummaryInfo { Name = category.Name, Start = currentRow + 1 });
                var subCategories = category.Items.GroupBy(i => i.SubCategory).Select(i => new { Name = i.Key, Items = i.ToList() }).ToList();
                currentRow++;

                int categoryStartingRow = currentRow + 1;

                var categoriesWithoutSubCategories = subCategories.Where(c => c.Name == null).ToList();
                foreach (var x in categoriesWithoutSubCategories)
                {
                    var expenses = x.Items.GroupBy(i => new { i.AccountCode, i.AccountName })
                    .Select(sc => new ExpenseSchedule
                    {
                        SubCategory = x.Name,
                        AccountCode = sc.Key.AccountCode,
                        AccountName = sc.Key.AccountName,
                        Jan = sc.Sum(i => i.Jan),
                        Feb = sc.Sum(i => i.Feb),
                        Mar = sc.Sum(i => i.Mar),
                        Apr = sc.Sum(i => i.Apr),
                        May = sc.Sum(i => i.May),
                        Jun = sc.Sum(i => i.Jun),
                        Jul = sc.Sum(i => i.Jul),
                        Aug = sc.Sum(i => i.Aug),
                        Sep = sc.Sum(i => i.Sep),
                        Oct = sc.Sum(i => i.Oct),
                        Nov = sc.Sum(i => i.Nov),
                        Dec = sc.Sum(i => i.Dec),
                    }).ToList();

                    bool singleRowOnly = expenses.Count == 1;
                    int currentMonth = Convert.ToInt32(DateTime.Today.ToString("yyyyMM"));
                    int closingStockRow = currentRow + expenses.Count + 2;
                    if (category?.Name?.ToLower().StartsWith("cost of steel") ?? false)
                    {
                        worksheet.Rows[currentRow][1].SetValue("Opening Stock");
                        worksheet.Rows[currentRow][1].Font.Bold = true;
                        worksheet.Rows[currentRow][2].SetValue(GetClosingStock(closingStockDic, 12, date.Year-1));


                        if (month > 1) worksheet.Rows[currentRow][3].Formula = $"=C{closingStockRow} * -1";
                        if (month > 2) worksheet.Rows[currentRow][4].Formula = $"=D{closingStockRow} * -1";
                        if (month > 3) worksheet.Rows[currentRow][5].Formula = $"=E{closingStockRow} * -1";
                        if (month > 4) worksheet.Rows[currentRow][6].Formula = $"=F{closingStockRow} * -1";
                        if (month > 5) worksheet.Rows[currentRow][7].Formula = $"=G{closingStockRow} * -1";
                        if (month > 6) worksheet.Rows[currentRow][8].Formula = $"=H{closingStockRow} * -1";
                        if (month > 7) worksheet.Rows[currentRow][9].Formula = $"=I{closingStockRow} * -1";
                        if (month > 8) worksheet.Rows[currentRow][10].Formula = $"=J{closingStockRow} * -1";
                        if (month > 9) worksheet.Rows[currentRow][11].Formula = $"=K{closingStockRow} * -1";
                        if (month > 10) worksheet.Rows[currentRow][12].Formula = $"=L{closingStockRow} * -1";
                        if (month > 11) worksheet.Rows[currentRow][13].Formula = $"=M{closingStockRow} * -1";
                        worksheet.Rows[currentRow][14].Formula = $"=C{currentRow + 1}";
                        worksheet.Rows[currentRow][14].Font.Bold = true;
                        worksheet.Range[$"A{currentRow + 1}:O{currentRow + 1}"].FillColor = Color.Beige;
                        worksheet.Range[$"A{currentRow + 1}:O{currentRow + 1}"].Borders.SetAllBorders(Color.DarkGray, BorderLineStyle.Thin);
                        worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].NumberFormat = "_(* #,##0_);_(* (#,##0);_(* -??_);_(@_)";
                        currentRow++;
                    }
                    foreach (var expense in expenses)
                    {
                        worksheet.Rows[currentRow][0].SetValue(expense.AccountCode);
                        worksheet.Rows[currentRow][0].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        worksheet.IgnoredErrors.Add(worksheet.Rows[currentRow][0], IgnoredErrorType.NumberAsText);
                        if (expense.AccountName?.ToUpper() == "PURCHASES - RAW MATERIALS/CONS")
                        {
                            worksheet.Rows[currentRow][1].SetValue("PURCHASES - RAW MATERIALS/CONSUMABLES");
                        }
                        else
                        {
                            worksheet.Rows[currentRow][1].SetValue(expense.AccountName?.ToUpper());
                        }
                        worksheet.Rows[currentRow][2].SetValue(expense.Jan);
                        worksheet.Rows[currentRow][3].SetValue(expense.Feb);
                        worksheet.Rows[currentRow][4].SetValue(expense.Mar);
                        worksheet.Rows[currentRow][5].SetValue(expense.Apr);
                        worksheet.Rows[currentRow][6].SetValue(expense.May);
                        worksheet.Rows[currentRow][7].SetValue(expense.Jun);
                        worksheet.Rows[currentRow][8].SetValue(expense.Jul);
                        worksheet.Rows[currentRow][9].SetValue(expense.Aug);
                        worksheet.Rows[currentRow][10].SetValue(expense.Sep);
                        worksheet.Rows[currentRow][11].SetValue(expense.Oct);
                        worksheet.Rows[currentRow][12].SetValue(expense.Nov);
                        worksheet.Rows[currentRow][13].SetValue(expense.Dec);
                        worksheet.Rows[currentRow][14].Formula = $"=SUM(C{currentRow + 1}:N{currentRow + 1})";
                        worksheet.Rows[currentRow][14].Font.Bold = true;
                        worksheet.Range[$"A{currentRow + 1}:O{currentRow + 1}"].Borders.SetAllBorders(Color.DarkGray, BorderLineStyle.Thin);
                        worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].NumberFormat = "_(* #,##0_);_(* (#,##0);_(* -??_);_(@_)";


                        if (singleRowOnly)
                        {
                            Style styleTotal = worksheet.Workbook.Styles[BuiltInStyleId.Total];
                            styleTotal.Borders.TopBorder.Color = Color.DimGray;
                            styleTotal.Borders.BottomBorder.Color = Color.DimGray;
                            styleTotal.Fill.BackgroundColor = Color.WhiteSmoke;
                            worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].Style = styleTotal;
                            worksheet.Range[$"A{currentRow + 1}:B{currentRow + 1}"].FillColor = Color.WhiteSmoke;
                        }
                        currentRow++;
                    }
                    if (category?.Name?.ToLower().StartsWith("cost of steel") ?? false)
                    {
                        worksheet.Rows[currentRow][1].SetValue("Closing Stock");
                        worksheet.Rows[currentRow][1].Font.Bold = true;

                        worksheet.Rows[currentRow][2].SetValue(GetClosingStock(closingStockDic, 1) * -1);
                        worksheet.Rows[currentRow][3].SetValue(GetClosingStock(closingStockDic, 2) * -1);
                        worksheet.Rows[currentRow][4].SetValue(GetClosingStock(closingStockDic, 3) * -1);
                        worksheet.Rows[currentRow][5].SetValue(GetClosingStock(closingStockDic, 4) * -1);
                        worksheet.Rows[currentRow][6].SetValue(GetClosingStock(closingStockDic, 5) * -1);
                        worksheet.Rows[currentRow][7].SetValue(GetClosingStock(closingStockDic, 6) * -1);
                        worksheet.Rows[currentRow][8].SetValue(GetClosingStock(closingStockDic, 7) * -1);
                        worksheet.Rows[currentRow][9].SetValue(GetClosingStock(closingStockDic, 8) * -1);
                        worksheet.Rows[currentRow][10].SetValue(GetClosingStock(closingStockDic, 9) * -1);
                        worksheet.Rows[currentRow][11].SetValue(GetClosingStock(closingStockDic, 10) * -1);
                        worksheet.Rows[currentRow][12].SetValue(GetClosingStock(closingStockDic, 11) * -1);
                        worksheet.Rows[currentRow][13].SetValue(GetClosingStock(closingStockDic, 12) * -1);

                        worksheet.Rows[currentRow][14].Formula = $"={currentMonthColumn}{currentRow + 1}";
                        worksheet.Rows[currentRow][14].Font.Bold = true;

                        worksheet.Range[$"A{currentRow + 1}:O{currentRow + 1}"].Borders.SetAllBorders(Color.DarkGray, BorderLineStyle.Thin);
                        worksheet.Range[$"A{currentRow + 1}:O{currentRow + 1}"].FillColor = Color.Beige;
                        worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].NumberFormat = "_(* #,##0_);_(* (#,##0);_(* -??_);_(@_)";
                        currentRow++;
                    }

                    if (expenses.Count > 1)
                    {

                        var categoryName = category.Name;
                        if (!string.IsNullOrWhiteSpace(categoryName) && categoryName.ToLower().StartsWith("cost of steel"))
                        {
                            categoryName = "Cost of Raw Materials & Consumables";
                        }

                        var totalSummaryText = $"Total {categoryName}";
                        worksheet.Rows[currentRow][1].SetValue(totalSummaryText);
                        worksheet.Rows[currentRow][1].Font.Bold = true;


                        worksheet.Rows[currentRow][2].Formula = $"=SUM(C{categoryStartingRow}:C{currentRow})";

                        worksheet.Rows[currentRow][3].Formula = $"=SUM(D{categoryStartingRow}:D{currentRow})";
                        worksheet.Rows[currentRow][4].Formula = $"=SUM(E{categoryStartingRow}:E{currentRow})";
                        worksheet.Rows[currentRow][5].Formula = $"=SUM(F{categoryStartingRow}:F{currentRow})";
                        worksheet.Rows[currentRow][6].Formula = $"=SUM(G{categoryStartingRow}:G{currentRow})";
                        worksheet.Rows[currentRow][7].Formula = $"=SUM(H{categoryStartingRow}:H{currentRow})";
                        worksheet.Rows[currentRow][8].Formula = $"=SUM(I{categoryStartingRow}:I{currentRow})";
                        worksheet.Rows[currentRow][9].Formula = $"=SUM(J{categoryStartingRow}:J{currentRow})";
                        worksheet.Rows[currentRow][10].Formula = $"=SUM(K{categoryStartingRow}:K{currentRow})";
                        worksheet.Rows[currentRow][11].Formula = $"=SUM(L{categoryStartingRow}:L{currentRow})";
                        worksheet.Rows[currentRow][12].Formula = $"=SUM(M{categoryStartingRow}:M{currentRow})";
                        worksheet.Rows[currentRow][13].Formula = $"=SUM(N{categoryStartingRow}:N{currentRow})";
                        worksheet.Rows[currentRow][14].Formula = $"=SUM(O{categoryStartingRow}:O{currentRow})";


                        worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].NumberFormat = "_(* #,##0_);_(* (#,##0);_(* -??_);_(@_)";

                        Style styleTotal = worksheet.Workbook.Styles[BuiltInStyleId.Total];
                        styleTotal.Borders.TopBorder.Color = Color.DimGray;
                        styleTotal.Borders.BottomBorder.Color = Color.DimGray;
                        styleTotal.Fill.BackgroundColor = Color.WhiteSmoke;
                        worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].Style = styleTotal;
                        worksheet.Range[$"A{currentRow + 1}:B{currentRow + 1}"].FillColor = Color.WhiteSmoke;
                        worksheet.Range[$"A{currentRow + 1}:B{currentRow + 1}"].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        worksheet.Range[$"A{currentRow + 1}:B{currentRow + 1}"].Borders.BottomBorder.Color = Color.LightGray;
                        currentRow++;
                    }
                    currentRow++;
                }




                foreach (var subCategory in subCategories)
                {
                    if (subCategory != null)
                    {

                        var expenses = subCategory.Items.GroupBy(i => new { i.AccountCode, i.AccountName })
                        .Select(sc => new ExpenseSchedule
                        {
                            SubCategory = subCategory.Name,
                            AccountCode = sc.Key.AccountCode,
                            AccountName = sc.Key.AccountName,
                            Jan = sc.Sum(i => i.Jan),
                            Feb = sc.Sum(i => i.Feb),
                            Mar = sc.Sum(i => i.Mar),
                            Apr = sc.Sum(i => i.Apr),
                            May = sc.Sum(i => i.May),
                            Jun = sc.Sum(i => i.Jun),
                            Jul = sc.Sum(i => i.Jul),
                            Aug = sc.Sum(i => i.Aug),
                            Sep = sc.Sum(i => i.Sep),
                            Oct = sc.Sum(i => i.Oct),
                            Nov = sc.Sum(i => i.Nov),
                            Dec = sc.Sum(i => i.Dec),
                        }).ToList();


                        string subCategoryName;
                        if (category != null && !string.IsNullOrWhiteSpace(category.Name) && category.Name.ToLower().Contains("man power"))
                        {
                            subCategoryName = $"{subCategory?.Name} - Man Power";
                        }
                        else
                        {
                            subCategoryName = subCategory.Name;
                        }

                        if (!string.IsNullOrWhiteSpace(subCategory.Name))
                        {
                            worksheet.Rows[currentRow][1].SetValue(subCategoryName);
                            worksheet.Rows[currentRow][1].Font.Size = 12;
                            worksheet.Rows[currentRow][1].Font.Color = Color.DarkSlateBlue;
                            worksheet.Rows[currentRow][1].Font.Bold = true;
                            worksheet.Range[$"A{currentRow + 1}:O{currentRow + 1}"].FillColor = Color.WhiteSmoke;
                            worksheet.Range[$"B{currentRow + 1}:O{currentRow + 1}"].Borders.SetOutsideBorders(Color.LightGray, BorderLineStyle.Thin);
                            currentRow++;
                            //Expense Start
                            int subCategoryStartingRow = currentRow + 1;
                            foreach (var expense in expenses)
                            {
                                worksheet.Rows[currentRow][0].SetValue(expense.AccountCode);
                                worksheet.Rows[currentRow][0].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                                worksheet.IgnoredErrors.Add(worksheet.Rows[currentRow][0], IgnoredErrorType.NumberAsText);
                                worksheet.Rows[currentRow][1].SetValue(expense.AccountName?.ToUpper());
                                worksheet.Rows[currentRow][2].SetValue(expense.Jan);
                                worksheet.Rows[currentRow][3].SetValue(expense.Feb);
                                worksheet.Rows[currentRow][4].SetValue(expense.Mar);
                                worksheet.Rows[currentRow][5].SetValue(expense.Apr);
                                worksheet.Rows[currentRow][6].SetValue(expense.May);
                                worksheet.Rows[currentRow][7].SetValue(expense.Jun);
                                worksheet.Rows[currentRow][8].SetValue(expense.Jul);
                                worksheet.Rows[currentRow][9].SetValue(expense.Aug);
                                worksheet.Rows[currentRow][10].SetValue(expense.Sep);
                                worksheet.Rows[currentRow][11].SetValue(expense.Oct);
                                worksheet.Rows[currentRow][12].SetValue(expense.Nov);
                                worksheet.Rows[currentRow][13].SetValue(expense.Dec);
                                worksheet.Rows[currentRow][14].Formula = $"=SUM(C{currentRow + 1}:N{currentRow + 1})";
                                worksheet.Rows[currentRow][14].Font.Bold = true;
                                worksheet.Range[$"A{currentRow + 1}:O{currentRow + 1}"].Borders.SetAllBorders(Color.DarkGray, BorderLineStyle.Thin);
                                worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].NumberFormat = "_(* #,##0_);_(* (#,##0);_(* -??_);_(@_)";
                                currentRow++;
                            }

                            var catSummaryInfo = summaryInfo.SingleOrDefault(x => x.Name == category.Name);
                            if (catSummaryInfo != null)
                            {
                                var subCatSummaryInfo = new SubCategorySummaryInfo
                                {
                                    Name = subCategoryName,
                                    Start = subCategoryStartingRow,
                                    End = currentRow
                                };

                                if (subCatSummaryInfo.Start != subCatSummaryInfo.End)
                                {


                                    var categoryName = category.Name;
                                    if (!string.IsNullOrWhiteSpace(categoryName) && categoryName.ToLower().StartsWith("cost of steel"))
                                    {
                                        categoryName = "Cost of Steel";
                                    }

                                    var totalSummaryText = $"Total {subCatSummaryInfo.Name ?? categoryName}";
                                    worksheet.Rows[currentRow][1].SetValue(totalSummaryText);
                                    worksheet.Rows[currentRow][1].Font.Bold = true;
                                    worksheet.Rows[currentRow][2].Formula = $"=SUM(C{subCatSummaryInfo.Start}:C{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][3].Formula = $"=SUM(D{subCatSummaryInfo.Start}:D{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][4].Formula = $"=SUM(E{subCatSummaryInfo.Start}:E{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][5].Formula = $"=SUM(F{subCatSummaryInfo.Start}:F{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][6].Formula = $"=SUM(G{subCatSummaryInfo.Start}:G{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][7].Formula = $"=SUM(H{subCatSummaryInfo.Start}:H{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][8].Formula = $"=SUM(I{subCatSummaryInfo.Start}:I{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][9].Formula = $"=SUM(J{subCatSummaryInfo.Start}:J{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][10].Formula = $"=SUM(K{subCatSummaryInfo.Start}:K{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][11].Formula = $"=SUM(L{subCatSummaryInfo.Start}:L{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][12].Formula = $"=SUM(M{subCatSummaryInfo.Start}:M{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][13].Formula = $"=SUM(N{subCatSummaryInfo.Start}:N{subCatSummaryInfo.End})";
                                    worksheet.Rows[currentRow][14].Formula = $"=SUM(O{subCatSummaryInfo.Start}:O{subCatSummaryInfo.End})";

                                    worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].NumberFormat = "_(* #,##0_);_(* (#,##0);_(* -??_);_(@_)";


                                    Style styleTotal = worksheet.Workbook.Styles[BuiltInStyleId.Total];
                                    styleTotal.Borders.TopBorder.Color = Color.WhiteSmoke;
                                    styleTotal.Borders.BottomBorder.Color = Color.LightGray;
                                    styleTotal.Fill.BackgroundColor = Color.WhiteSmoke;
                                    worksheet.Range[$"C{currentRow + 1}:O{currentRow + 1}"].Style = styleTotal;
                                    worksheet.Range[$"A{currentRow + 1}:B{currentRow + 1}"].FillColor = Color.WhiteSmoke;
                                    worksheet.Range[$"A{currentRow + 1}:B{currentRow + 1}"].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                                    worksheet.Range[$"A{currentRow + 1}:B{currentRow + 1}"].Borders.BottomBorder.Color = Color.LightGray;
                                    currentRow++;
                                    worksheet.Rows[currentRow].RowHeight = 20;

                                }
                                catSummaryInfo.SubCategories.Add(subCatSummaryInfo);
                            }
                            //Expense End
                            currentRow++;

                        }



                    }

                }// Sub Category End

                var grandSummaryInfo = summaryInfo.SingleOrDefault(x => x.Name == category.Name);
                if (grandSummaryInfo != null)
                {
                    grandSummaryInfo.End = currentRow;
                    if (grandSummaryInfo.Start != grandSummaryInfo.End && grandSummaryInfo.SubCategories.Count > 1)
                    {
                        var summaryString = string.Join(",", grandSummaryInfo.SubCategories.Select(x => $"C{x.End + 1}").ToArray());
                        currentRow++;
                        worksheet.Rows[currentRow - 1][1].SetValue(grandSummaryInfo.Name);
                        worksheet.Rows[currentRow - 1][1].Font.Bold = true;
                        worksheet.Rows[currentRow - 1][2].Formula = $"=SUM({BuildSummaryStringFor("C", grandSummaryInfo.SubCategories)})";
                        worksheet.Rows[currentRow - 1][3].Formula = $"=SUM({BuildSummaryStringFor("D", grandSummaryInfo.SubCategories)})";
                        worksheet.Range[$"A{currentRow}:O{currentRow}"].FillColor = Color.LightGray;
                        worksheet.Range[$"A{currentRow}:B{currentRow}"].Borders.SetOutsideBorders(Color.LightGray, BorderLineStyle.Thin);

                        worksheet.Range[$"C{currentRow}:O{currentRow}"].Borders.TopBorder.Color = Color.DimGray;
                        worksheet.Range[$"C{currentRow}:O{currentRow}"].Borders.BottomBorder.Color = Color.DimGray;
                        worksheet.Range[$"C{currentRow}:O{currentRow}"].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        worksheet.Range[$"C{currentRow}:O{currentRow}"].Borders.BottomBorder.LineStyle = BorderLineStyle.Double;
                        worksheet.Range[$"C{currentRow}:O{currentRow}"].NumberFormat = "_(* #,##0_);_(* (#,##0);_(* -??_);_(@_)";
                        worksheet.Range[$"C{currentRow}:O{currentRow}"].Font.Bold = true;
                        currentRow++;
                    }
                }

            }



            worksheet.Range[$"{currentMonthColumn}4:{currentMonthColumn}{currentRow + 1}"].Font.Bold = true;

            worksheet.Columns[0].Visible = false;
            for (int i = 1; i <= 14; i++)
            {
                worksheet.Columns[i].AutoFit();
            }
        }

        private string BuildSummaryStringFor(string columnName, List<SubCategorySummaryInfo> items)
        {
            return string.Join(",", items.Select(x => $"{columnName}{x.End + 1}").ToArray());
        }



        private decimal GetClosingStock(Dictionary<int, decimal> dic, int month, int year = 0)
        {
            if (year == 0) year = DateTime.Today.Year;
            var key = Convert.ToInt32(new DateTime(year, month, 1).ToString("yyyyMM"));
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            return 0;
        }

        private string GetExportPath(string fileName)
        {
            string fileToWriteTo = Path.GetRandomFileName();

            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string path = Path.Combine(directory, fileName);
            return path;
        }
        // Define other methods, classes and namespaces here

        public List<ExpenseSchedule> GetSummary(IEnumerable<ExpenseSchedule> items)
        {
            var summary = items.OrderBy(x => x.CatSeq).GroupBy(r => new { r.Database, r.Division, r.DivisionId, r.CatSeq, r.Category, r.Year })
                    .Select(x => new ExpenseSchedule
                    {
                        DivisionId = x.Key.DivisionId,
                        Division = x.Key.Division,
                        Database = x.Key.Database,
                        Category = x.Key.Category,
                        Year = x.Key.Year,
                        Jan = x.Sum(r => r.Jan),
                        Feb = x.Sum(r => r.Feb),
                        Mar = x.Sum(r => r.Mar),
                        Apr = x.Sum(r => r.Apr),
                        May = x.Sum(r => r.May),
                        Jun = x.Sum(r => r.Jun),
                        Jul = x.Sum(r => r.Jul),
                        Aug = x.Sum(r => r.Aug),
                        Sep = x.Sum(r => r.Sep),
                        Oct = x.Sum(r => r.Oct),
                        Nov = x.Sum(r => r.Nov),
                        Dec = x.Sum(r => r.Dec)
                    }).ToList();

            return summary;
        }

        private List<ExpenseSchedule> GetExpenses(IEnumerable<ExpenseSchedule> items)
        {
            var expenses = items.GroupBy(r => new { r.Database, r.Category, r.AccountCode, r.AccountName, r.Year })
                        .Select(x => new ExpenseSchedule
                        {
                            Database = x.Key.Database,
                            Category = x.Key.Category,
                            AccountCode = x.Key.AccountCode,
                            AccountName = x.Key.AccountName,
                            Year = x.Key.Year,
                            Jan = x.Sum(r => r.Jan),
                            Feb = x.Sum(r => r.Feb),
                            Mar = x.Sum(r => r.Mar),
                            Apr = x.Sum(r => r.Apr),
                            May = x.Sum(r => r.May),
                            Jun = x.Sum(r => r.Jun),
                            Jul = x.Sum(r => r.Jul),
                            Aug = x.Sum(r => r.Aug),
                            Sep = x.Sum(r => r.Sep),
                            Oct = x.Sum(r => r.Oct),
                            Nov = x.Sum(r => r.Nov),
                            Dec = x.Sum(r => r.Dec)
                        }).ToList();

            return expenses;
        }

        public class CategorySummaryInfo
        {
            public string Name { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
            public int SpaceRequired { get; set; }

            public List<SubCategorySummaryInfo> SubCategories = new List<SubCategorySummaryInfo>();

        }
        public class SubCategorySummaryInfo
        {
            public string Name { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
            public int SpaceRequired { get; set; }
        }
        public class ExpenseSchedule
        {
            public ExpenseSchedule() { }
            public static List<ExpenseSchedule> ProcessExpenseSchedules(string path,DateTime date)
            {
                int year = date.Year;
                int currentMonth = date.Month;

                var dic = new Dictionary<string, (int Year, int Month)>();
                for (int i = 1; i <= 12; i++)
                {
                    dic.Add($"{year}/{i:000}", (year, i));
                }


                int row = 0;
                int headerRow = 5;
                List<ExpenseSchedule> records = new List<ExpenseSchedule>();
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read())
                        {
                            row++;
                            if (row >= headerRow + 1 && reader.GetString(0) != null)
                            {
                                var item = new ExpenseSchedule(reader.GetString(0), currentMonth, Convert.ToDecimal(reader.GetValue(4)), dic);
                                item.Database = reader.Name;
                                item.AccountCode = reader.GetString(1);
                                item.DivisionId = Convert.ToInt32(reader.GetValue(2));
                                item.SubCategoryId = Convert.ToInt32(reader.GetValue(3));
                                records.Add(item);
                            }
                        }

                        reader.NextResult();
                        row = 0;
                        while (reader.Read())
                        {
                            row++;
                            if (row >= headerRow + 1 && reader.GetString(0) != null)
                            {
                                var item = new ExpenseSchedule(reader.GetString(0), currentMonth, Convert.ToDecimal(reader.GetValue(4)), dic);
                                item.Database = reader.Name;
                                item.AccountCode = reader.GetString(1);


                                if (int.TryParse(reader.GetValue(2).ToString(), out int divisionId))
                                {
                                    item.DivisionId = divisionId;
                                }


                                item.SubCategoryId = Convert.ToInt32(reader.GetValue(3));
                                records.Add(item);
                            }
                        }

                        reader.NextResult();
                        row = 0;
                        while (reader.Read())
                        {
                            row++;
                            if (row >= headerRow + 1 && reader.GetString(0) != null)
                            {
                                var item = new ExpenseSchedule(reader.GetString(0), currentMonth, Convert.ToDecimal(reader.GetValue(5)), dic);
                                item.Database = reader.Name;
                                item.AccountCode = reader.GetString(1);
                                item.DivisionId = Convert.ToInt32(reader.GetValue(2));
                                item.T8Code = Convert.ToInt32(reader.GetValue(3));
                                item.SubCategoryId = Convert.ToInt32(reader.GetValue(4));
                                records.Add(item);
                            }
                        }

                        var masterRecords = new List<MasterData>();

                        //Master Data

                        reader.NextResult();
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.GetString(0) != null)
                            {
                                var item = new MasterData
                                {
                                    Database = reader.Name.Replace("_SUM", ""),
                                    Category = reader.GetString(0),
                                    TCode = Convert.ToInt32(reader.GetValue(1)),
                                    SubCategory = reader.GetString(2),
                                    AccountCode = Convert.ToInt32(reader.GetValue(3)).ToString(),
                                    AccountName = reader.GetString(4)
                                };
                                masterRecords.Add(item);
                            }
                        }

                        reader.NextResult();
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.GetString(0) != null)
                            {
                                var item = new MasterData
                                {
                                    Database = reader.Name.Replace("_SUM", ""),
                                    Category = reader.GetString(0),
                                    TCode = Convert.ToInt32(reader.GetValue(1)),
                                    SubCategory = reader.GetString(2),
                                    AccountCode = reader.GetValue(3)?.ToString(),
                                    AccountName = reader.GetString(4)
                                };
                                masterRecords.Add(item);
                            }
                        }


                        reader.NextResult();
                        reader.Read();
                        while (reader.Read())
                        {
                            if (reader.GetString(0) != null)
                            {
                                var item = new MasterData
                                {
                                    Database = reader.Name.Replace("_SUM", ""),
                                    Category = reader.GetString(0),
                                    TCode = Convert.ToInt32(reader.GetValue(1)),
                                    SubCategory = reader.GetString(2),
                                    AccountCode = reader.GetString(3),
                                    AccountName = reader.GetString(4)
                                };
                                masterRecords.Add(item);
                            }
                        }
                        //masterRecords.Dump();
                        
                        foreach (var record in records)
                        {
                            if (record.T8Code > 0)
                            {
                                var mr = masterRecords
                                        .SingleOrDefault(r =>
                                        r.Database == record.Database &&
                                        r.AccountCode == record.T8Code.ToString() &&
                                        r.TCode == record.SubCategoryId);



                                if (mr != null)
                                {
                                    record.Category = mr.Category;
                                    record.SubCategory = mr.SubCategory;
                                    record.AccountName = mr.AccountName;
                                }
                            }
                            else
                            {
                                var mr = masterRecords
                                .SingleOrDefault(r => r.Database == record.Database &&
                                            r.AccountCode == record.AccountCode &&
                                            r.TCode == record.SubCategoryId);

                                if (mr != null)
                                {
                                    record.Category = mr.Category;
                                    record.SubCategory = mr.SubCategory;
                                    record.AccountName = mr.AccountName;
                                }
                            }


                        //    record.Division = divisionName;

                            //var div = divisions.SingleOrDefault(x => x.Database == record.Database && x.DivisionId == record.DivisionId);
                            //if (div != null)
                            //{
                            //    record.Division = division.DivisionName;
                            //}

                        }
                    }
                }

                return records;
            }






            public ExpenseSchedule(string period, int currentMonth, decimal amount, Dictionary<string, (int Year, int Month)> periodDictionary)
            {
                Period = period;
                (int Year, int Month) yearInfo;
                int year = 0;
                int month = 0;
                if (periodDictionary != null)
                {
                    if (periodDictionary.TryGetValue(period, out yearInfo))
                    {
                        year = yearInfo.Year;
                        month = yearInfo.Month;
                    }
                }
                else
                {
                    var periodInfo = period.Split('/');
                    year = Convert.ToInt32(periodInfo[0]);
                    month = Convert.ToInt32(periodInfo[1]);
                }


                Year = year;

                amount = currentMonth >= month ? amount : 0;

                Amount = amount;

                switch (month)
                {
                    case 1: Jan = amount; break;
                    case 2: Feb = amount; break;
                    case 3: Mar = amount; break;
                    case 4: Apr = amount; break;
                    case 5: May = amount; break;
                    case 6: Jun = amount; break;
                    case 7: Jul = amount; break;
                    case 8: Aug = amount; break;
                    case 9: Sep = amount; break;
                    case 10: Oct = amount; break;
                    case 11: Nov = amount; break;
                    case 12: Dec = amount; break;
                }
            }

            public string Database { get; set; }
            public int DivisionId { get; set; }
            public string Division { get; set; }
            public int T8Code { get; set; }
            public string Period { get; set; }
            public int CatSeq { get; set; }
            public string Category { get; set; }
            public string SubCategory { get; set; }
            public int SubCategoryId { get; set; }
            public string AccountCode { get; set; }
            public string AccountName { get; set; }
            public int Year { get; set; }
            public decimal Amount { get; set; }
            public decimal Jan { get; set; }
            public decimal Feb { get; set; }
            public decimal Mar { get; set; }
            public decimal Apr { get; set; }
            public decimal May { get; set; }
            public decimal Jun { get; set; }
            public decimal Jul { get; set; }
            public decimal Aug { get; set; }
            public decimal Sep { get; set; }
            public decimal Oct { get; set; }
            public decimal Nov { get; set; }
            public decimal Dec { get; set; }
        }

        public class MasterData
        {
            public string Database { get; set; }
            public string Category { get; set; }
            public int TCode { get; set; }
            public string SubCategory { get; set; }
            public string AccountCode { get; set; }
            public string AccountName { get; set; }
        }

        //public class Division
        //{
        //    public string Database { get; set; }
        //    public int DivisionId { get; set; }
        //    public string DivisionName { get; set; }

        //    public static List<Division> GetDivisions()
        //    {
        //        return new List<Division>
        //{
        //    new Division {Database="CES",DivisionId=101,DivisionName="Cut & Bend Mussafah"},
        //    new Division {Database="CES",DivisionId=102,DivisionName="Coupler Mussafah"},
        //    new Division {Database="CES",DivisionId=110,DivisionName="Wire Mesh"},
        //    new Division {Database="CES",DivisionId=111,DivisionName="Cage"},
        //    new Division {Database="CES",DivisionId=112,DivisionName="Melting"},
        //    new Division {Database="CES",DivisionId=120,DivisionName="Epoxy"},
        //    new Division {Database="CES",DivisionId=125,DivisionName="Signs"},
        //    new Division {Database="CES",DivisionId=126,DivisionName="Metal Works"},
        //    new Division {Database="CES",DivisionId=127,DivisionName="Site"},
        //    new Division {Database="CES",DivisionId=130,DivisionName="Fabrication"},
        //    new Division {Database="CSF",DivisionId=100,DivisionName="Cut & Bend DIP"},
        //    new Division {Database="CSF",DivisionId=101,DivisionName="Coupler DIP"},
        //    new Division {Database="CSF",DivisionId=102,DivisionName="Cut & Bend UAQ"},
        //    new Division {Database="CSF",DivisionId=103,DivisionName="Coupler UAQ"},
        //    new Division {Database="CSF",DivisionId=104,DivisionName="Cut & Bend Maxsteel"},
        //    new Division {Database="CSF",DivisionId=105,DivisionName="Coupler Maxsteel"},
        //    new Division {Database="MAXSTEEL",DivisionId=104,DivisionName="Cut & Bend Maxsteel"},
        //    new Division {Database="MAXSTEEL",DivisionId=105,DivisionName="Coupler Maxsteel"}
        //};
        //    }
        //}


        public class OpeningStock
        {
            public int DivisionId { get; set; }
            public DateTime TransactionDate { get; set; }
            public decimal Amount { get; set; }


            public static List<OpeningStock> GetOpeningStock(string path, List<DivisionInfo> divisions)
            {
                int row = 0;
                var list = new List<OpeningStock>();

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.Read();
                        while (reader.Read() && reader.GetValue(0) != null)
                        {
                            var name = reader.GetValue(1);
                            var id = divisions.SingleOrDefault(d => d.Name == name?.ToString())?.Id ?? 0;
                            for (int i = 1; i < 12; i++)
                            {
                                var amount = Convert.ToDecimal(reader.GetValue(i + 1) ?? 0);
                                if (amount > 0)
                                {
                                    list.Add(new OpeningStock { DivisionId = id, TransactionDate = new DateTime(2020, i, 1), Amount = amount });
                                }
                            }
                        }
                    }
                }

                return list;
            }
        }


    }

    [NotifyPropertyChanged]
    public class DivisionInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Database { get; set; }
        public int[] Codes { get; set; }

    }

    [NotifyPropertyChanged]
    public class Criteria
    {
      //  public DivisionInfo SelectedDivision { get; set; }

      //  public string Database { get; set; }

        public DateTime Date { get; set; }
    }



    public class OpeningStock
    {
        public int DivisionId { get; set; }
        public string DivisionName{get;set;}
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }


        public static List<OpeningStock> GetOpeningStock(string path,List<DivisionInfo> divisions)
        {
            int row=0;
            var list=new List<OpeningStock>();
		
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.NextResult();
                    reader.NextResult();
                    reader.NextResult();
                    reader.NextResult();
                    reader.NextResult();
                    reader.NextResult();
                    reader.NextResult();
                    reader.Read();
                    while (reader.Read() && reader.GetValue(0)!=null)
                    { 
                        var name=reader.GetValue(1)?.ToString();
                        var id=divisions.SingleOrDefault(d => d.Name==name)?.Id ?? 0;
                        for (int i = 1; i < 12; i++)
                        {
                            var amount = Convert.ToDecimal(reader.GetValue(i + 1) ?? 0);
                            if (amount > 0)
                            {
                                list.Add(new OpeningStock{DivisionId = id,DivisionName=name, TransactionDate=new DateTime(2020,i,1), Amount=amount});		
                            }
                        }
                    }
                }
            }
		
            return list;
        }

    }
}
