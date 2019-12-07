using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using DevExpress.Data;
using DevExpress.Office;
using DevExpress.Spreadsheet;
using Humanizer;
using Jasmine.Core.Attributes;
using Jasmine.Core.Contracts;


namespace Jasmine.Core.Services
{
    public class WorkSheetBuilder : IWorkSheetBuilder
    {
        private int _row = 0;
        // private int i = 1;

        int _bodyStartingRow = 0;
        private int _coumnHeadingRow = 0;

        public Worksheet CreateWorkSheet<T>(List<T> items, Worksheet worksheet, string range) where T : class
        {
            _row = 0;
            _bodyStartingRow = 0;
            _coumnHeadingRow = 0;

            PropertyInfo[] props = typeof(T).GetProperties();

            IEnumerable<Attribute> r = typeof(T).GetCustomAttributes();
            ExcelReportAttribute reportAttribute = r.FirstOrDefault() as ExcelReportAttribute;

            if (reportAttribute != null)
            {
                worksheet.Cells[_row, 0].Value = reportAttribute.Heading;
                worksheet.Cells[_row, 0].Font.Bold = true;
                worksheet.Cells[_row, 0].Font.Size = 16;

                string headerRange =
                    $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Range[headerRange].Merge();


                if (!string.IsNullOrWhiteSpace(reportAttribute.SubHeading))
                {
                    _row++;

                    worksheet.Cells[_row, 0].Value = reportAttribute.SubHeading;
                    worksheet.Cells[_row, 0].Font.Color = Color.Maroon;
                    worksheet.Cells[_row, 0].Font.Bold = true;
                    worksheet.Cells[_row, 0].Font.Size = 14;

                    headerRange =
                        $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                    worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Range[headerRange].Merge();
                }



                _row++;

                worksheet.Cells[_row, 0].Value = $"({range})";
                worksheet.Cells[_row, 0].Font.Bold = true;
                worksheet.Cells[_row, 0].Font.Color = Color.Navy;
                worksheet.Cells[_row, 0].Font.Size = 11;


                headerRange =
                    $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Range[headerRange].Merge();

                _row++;

            }

            _coumnHeadingRow = _row;

            _row++;

            _bodyStartingRow = _row;

            for (int column = 0; column < props.Length; column++)
            {
                string heading = props[column].Name;
                PropertyInfo prop = props[column];
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    IColumnWithFormat decAttr = attr as TonnageAttribute;
                    SetAttributeValue(items, worksheet, column, prop, decAttr);

                    IColumnWithFormat priceAttribute = attr as PriceAttribute;
                    SetAttributeValue(items, worksheet, column, prop, priceAttribute);

                    IColumnWithFormat dateAttribute = attr as DateAttribute;
                    SetAttributeValue(items, worksheet, column, prop, dateAttribute);
                }


                if (string.IsNullOrWhiteSpace(worksheet.Cells[_row, column].Value.ToString()))
                {
                    heading = props[column].Name.Humanize(LetterCasing.Title);
                    worksheet.Cells[_coumnHeadingRow, column].Value = heading;
                }
            }

            for (int j = 0; j < items.Count; j++)
            {
                PropertyInfo[] p = items[j].GetType().GetProperties();
                for (int k = 0; k < p.Length; k++)
                {
                    worksheet.Cells[_row, k].SetValue(p[k].GetValue(items[j]));
                }
                _row++;
            }

            worksheet.Columns.AutoFit(0, props.Length);

            string bodyRange = $"{worksheet.Cells[_bodyStartingRow, 0].GetReferenceA1()}:{worksheet.Cells[items.Count + _bodyStartingRow - 1, props.Length - 1].GetReferenceA1()}";

            DrawBody(worksheet, bodyRange);

            string columnHeadingRange = $"{worksheet.Cells[_bodyStartingRow - 1, 0].GetReferenceA1()}:{worksheet.Cells[_bodyStartingRow - 1, props.Length - 1].GetReferenceA1()}";
            DrawHeader(worksheet, columnHeadingRange, Color.FromArgb(224, 214, 211));

            string autoFilterRange = $"{worksheet.Cells[_bodyStartingRow - 1, 0].GetReferenceA1()}:{worksheet.Cells[items.Count + _bodyStartingRow, props.Length - 1].GetReferenceA1()}";
            worksheet.AutoFilter.Apply(worksheet.Range[autoFilterRange]);

            return

        worksheet;
        }

        private void SetAttributeValue<T>(List<T> items, Worksheet worksheet, int column, PropertyInfo prop, IColumnWithFormat decAttr) where T : class
        {
            if (decAttr != null)
            {
                string format = decAttr.Format;
                worksheet.Columns[column].NumberFormat = format;


                worksheet.Cells[_coumnHeadingRow, column].Value = string.IsNullOrWhiteSpace(decAttr.Heading)
                    ? prop.Name.Humanize(LetterCasing.Title)
                    : decAttr.Heading;

                string sumRange = $"{worksheet.Cells[items.Count + _bodyStartingRow, column].GetReferenceA1()}";
                if (decAttr.Summary)
                {

                    string @from = worksheet.Cells[_bodyStartingRow, column].GetReferenceA1();
                    string to = worksheet.Cells[items.Count + _bodyStartingRow - 1, column].GetReferenceA1();
                    string formula = $"=SUBTOTAL(9,{from}:{to})";
                    worksheet.Range[sumRange].Formula = formula;
                    DrawSummary(worksheet, sumRange);
                }
            }
        }


        public Worksheet CreateWorkSheet(IList items, Worksheet worksheet, string range)
        {
            _row = 0;
            _bodyStartingRow = 0;
            _coumnHeadingRow = 0;

            Type t = items.GetType().GetGenericArguments().Single();

            PropertyInfo[] props = t.GetProperties();

            IEnumerable<Attribute> r = t.GetCustomAttributes();
            ExcelReportAttribute reportAttribute = r.FirstOrDefault() as ExcelReportAttribute;

            if (reportAttribute != null)
            {
                worksheet.Cells[_row, 0].Value = reportAttribute.Heading;
                worksheet.Cells[_row, 0].Font.Bold = true;
                worksheet.Cells[_row, 0].Font.Size = 16;

                string headerRange =
                    $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Range[headerRange].Merge();
                worksheet.Range[headerRange].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                worksheet.Rows[_row].Height = 20;

                if (!string.IsNullOrWhiteSpace(reportAttribute.SubHeading))
                {
                    _row++;

                    worksheet.Cells[_row, 0].Value = reportAttribute.SubHeading;
                    worksheet.Cells[_row, 0].Font.Color = Color.Maroon;
                    worksheet.Cells[_row, 0].Font.Bold = true;
                    worksheet.Cells[_row, 0].Font.Size = 14;

                    headerRange =
                        $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                    worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Range[headerRange].Merge();
                }



                _row++;

                worksheet.Cells[_row, 0].Value = $"({range})";
                worksheet.Cells[_row, 0].Font.Bold = true;
                worksheet.Cells[_row, 0].Font.Color = Color.Navy;
                worksheet.Cells[_row, 0].Font.Size = 11;


                headerRange =
                    $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Range[headerRange].Merge();

                _row++;

            }

            return ExtractWorkSheet(items, worksheet, props);
        }

        private Worksheet ExtractWorkSheet(IList items, Worksheet worksheet, PropertyInfo[] props)
        {
            _coumnHeadingRow = _row;

            _row++;

            _bodyStartingRow = _row;

            for (int column = 0; column < props.Length; column++)
            {
                string heading = props[column].Name;
                PropertyInfo prop = props[column];
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    IColumnWithFormat decAttr = attr as TonnageAttribute;
                    SetAttributeValue(items, worksheet, column, prop, decAttr);

                    IColumnWithFormat priceAttribute = attr as PriceAttribute;
                    SetAttributeValue(items, worksheet, column, prop, priceAttribute);


                    IColumnWithFormat dateAttribute = attr as DateAttribute;
                    SetAttributeValue(items, worksheet, column, prop, dateAttribute);
                }


                if (string.IsNullOrWhiteSpace(worksheet.Cells[_row, column].Value.ToString()))
                {
                    heading = props[column].Name.Humanize(LetterCasing.Title);
                    worksheet.Cells[_coumnHeadingRow, column].Value = heading;
                }
            }

            for (int j = 0; j < items.Count; j++)
            {
                PropertyInfo[] p = items[j].GetType().GetProperties();
                for (int k = 0; k < p.Length; k++)
                {
                    worksheet.Cells[_row, k].SetValue(p[k].GetValue(items[j]));
                }
                _row++;
            }

            worksheet.Columns.AutoFit(0, props.Length);

            string bodyRange = $"{worksheet.Cells[_bodyStartingRow, 0].GetReferenceA1()}:{worksheet.Cells[items.Count + _bodyStartingRow - 1, props.Length - 1].GetReferenceA1()}";

            DrawBody(worksheet, bodyRange);

            string columnHeadingRange = $"{worksheet.Cells[_bodyStartingRow - 1, 0].GetReferenceA1()}:{worksheet.Cells[_bodyStartingRow - 1, props.Length - 1].GetReferenceA1()}";
            DrawHeader(worksheet, columnHeadingRange, Color.FromArgb(224, 214, 211));

            string autoFilterRange = $"{worksheet.Cells[_bodyStartingRow - 1, 0].GetReferenceA1()}:{worksheet.Cells[items.Count + _bodyStartingRow, props.Length - 1].GetReferenceA1()}";
            worksheet.AutoFilter.Apply(worksheet.Range[autoFilterRange]);

            return worksheet;
        }

        public Worksheet CreateWorkSheet(IDataContainerBase report, Worksheet worksheet)
        {

            _row = 0;
            _bodyStartingRow = 0;
            _coumnHeadingRow = 0;

            var items = report.DataSource as IList;

            IReportOptions reportOptions = (report as IReportBase)?.Options;

            if (items != null)
            {
                Type t = items.GetType().GetGenericArguments().Single();

                PropertyInfo[] props = t.GetProperties();

                IEnumerable<Attribute> r = t.GetCustomAttributes();
                if (reportOptions != null)
                {
                    worksheet.Cells[_row, 0].Value = reportOptions.ReportHeading;
                    worksheet.Cells[_row, 0].Font.Bold = true;
                    worksheet.Cells[_row, 0].Font.Size = 16;

                    string headerRange =
                        $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                    worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Range[headerRange].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Range[headerRange].Merge();


                    if (!string.IsNullOrWhiteSpace(reportOptions.ReportSubHeading))
                    {
                        _row++;

                        worksheet.Cells[_row, 0].Value = reportOptions.ReportSubHeading;
                        worksheet.Cells[_row, 0].Font.Color = Color.Maroon;
                        worksheet.Cells[_row, 0].Font.Bold = true;
                        worksheet.Cells[_row, 0].Font.Size = 14;

                        headerRange =
                            $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                        worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        worksheet.Range[headerRange].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        worksheet.Range[headerRange].Merge();
                    }



                    _row++;

                    worksheet.Cells[_row, 0].Value = $"({reportOptions.ReportRangeHeading})";
                    worksheet.Cells[_row, 0].Font.Bold = true;
                    worksheet.Cells[_row, 0].Font.Color = Color.Navy;
                    worksheet.Cells[_row, 0].Font.Size = 11;


                    headerRange =
                        $"{worksheet.Cells[_row, 0].GetReferenceA1()}:{worksheet.Cells[_row, props.Length - 1].GetReferenceA1()}";

                    worksheet.Range[headerRange].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Range[headerRange].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Range[headerRange].Merge();


                    _row++;

                }


                _coumnHeadingRow = _row;

                _row++;

                _bodyStartingRow = _row;

                for (int column = 0; column < props.Length; column++)
                {
                    PropertyInfo prop = props[column];
                    object[] attrs = prop.GetCustomAttributes(true);
                    foreach (object attr in attrs)
                    {
                        IColumnWithFormat decAttr = attr as TonnageAttribute;
                        SetAttributeValue(items, worksheet, column, prop, decAttr);

                        IColumnWithFormat priceAttribute = attr as PriceAttribute;
                        SetAttributeValue(items, worksheet, column, prop, priceAttribute);


                        IColumnWithFormat dateAttribute = attr as DateAttribute;
                        SetAttributeValue(items, worksheet, column, prop, dateAttribute);
                    }


                    if (string.IsNullOrWhiteSpace(worksheet.Cells[_row, column].Value.ToString()))
                    {
                        string heading = props[column].Name.Humanize(LetterCasing.Title);
                        worksheet.Cells[_coumnHeadingRow, column].Value = heading;
                    }
                }

                foreach (object item in items)
                {
                    PropertyInfo[] p = item.GetType().GetProperties();
                    for (int k = 0; k < p.Length; k++)
                    {
                        worksheet.Cells[_row, k].SetValue(p[k].GetValue(item));
                    }
                    _row++;
                }

                worksheet.Columns.AutoFit(0, props.Length);

                string bodyRange = $"{worksheet.Cells[_bodyStartingRow, 0].GetReferenceA1()}:{worksheet.Cells[items.Count + _bodyStartingRow - 1, props.Length - 1].GetReferenceA1()}";

                DrawBody(worksheet, bodyRange);

                string columnHeadingRange = $"{worksheet.Cells[_bodyStartingRow - 1, 0].GetReferenceA1()}:{worksheet.Cells[_bodyStartingRow - 1, props.Length - 1].GetReferenceA1()}";
                DrawHeader(worksheet, columnHeadingRange, Color.FromArgb(224, 214, 211));

                string autoFilterRange = $"{worksheet.Cells[_bodyStartingRow - 1, 0].GetReferenceA1()}:{worksheet.Cells[items.Count + _bodyStartingRow, props.Length - 1].GetReferenceA1()}";
                worksheet.AutoFilter.Apply(worksheet.Range[autoFilterRange]);

                worksheet.Columns.AutoFit(0, props.Length - 1);
                worksheet.Rows.AutoFit(_coumnHeadingRow + 1, items.Count);
            }





            return worksheet;
        }

        private void SetAttributeValue(IList items, Worksheet worksheet, int column, PropertyInfo prop, IColumnWithFormat decAttr)
        {
            if (decAttr != null)
            {
                string format = decAttr.Format;
                worksheet.Columns[column].NumberFormat = format;


                worksheet.Cells[_coumnHeadingRow, column].Value = string.IsNullOrWhiteSpace(decAttr.Heading)
                    ? prop.Name.Humanize(LetterCasing.Title)
                    : decAttr.Heading;

                string sumRange = $"{worksheet.Cells[items.Count + _bodyStartingRow, column].GetReferenceA1()}";
                if (decAttr.Summary)
                {

                    string @from = worksheet.Cells[_bodyStartingRow, column].GetReferenceA1();
                    string to = worksheet.Cells[items.Count + _bodyStartingRow - 1, column].GetReferenceA1();
                    string formula = $"=SUBTOTAL(9,{from}:{to})";
                    worksheet.Range[sumRange].Formula = formula;
                    DrawSummary(worksheet, sumRange);
                }
            }
        }

        private static void DrawHeader(Worksheet worksheet, int row, int column, Color? color = null)
        {
            Cell rangeInQuestion = worksheet.Cells[row, column];
            Formatting formatting = rangeInQuestion.BeginUpdateFormatting();

            if (color != null)
                formatting.Fill.BackgroundColor = color.Value;

            formatting.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            formatting.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            formatting.Borders.SetAllBorders(Color.Black, BorderLineStyle.Hair);
            formatting.Borders.BottomBorder.LineStyle = BorderLineStyle.Double;
            formatting.Font.Bold = true;
            worksheet.Workbook.Unit = DocumentUnit.Point;
            rangeInQuestion.RowHeight = 18;
            rangeInQuestion.EndUpdateFormatting(formatting);
            //worksheet.Range[range].AutoFitRows();
        }


        private static void DrawHeader(Worksheet worksheet, string range, Color? color = null)
        {
            CellRange rangeInQuestion = worksheet.Range[range];
            Formatting formatting = rangeInQuestion.BeginUpdateFormatting();

            if (color != null)
                formatting.Fill.BackgroundColor = color.Value;

            formatting.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            formatting.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            formatting.Borders.SetAllBorders(Color.Black, BorderLineStyle.Hair);
            formatting.Borders.BottomBorder.LineStyle = BorderLineStyle.Double;
            formatting.Font.Bold = true;
            worksheet.Workbook.Unit = DocumentUnit.Point;
            rangeInQuestion.RowHeight = 20;
            rangeInQuestion.EndUpdateFormatting(formatting);
            //worksheet.Range[range].AutoFitRows();
        }
        private static void DrawSummary(Worksheet sheet, string range, Color? color = null)
        {
            CellRange rangeInQuestion = sheet.Range[range];
            Formatting formatting = rangeInQuestion.BeginUpdateFormatting();

            if (color != null)
                formatting.Fill.BackgroundColor = color.Value;

            formatting.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
            formatting.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            formatting.Borders.SetAllBorders(Color.Black, BorderLineStyle.Hair);
            formatting.Borders.BottomBorder.LineStyle = BorderLineStyle.Double;
            formatting.Font.Bold = true;
            sheet.Workbook.Unit = DocumentUnit.Point;
            rangeInQuestion.RowHeight = 20;
            rangeInQuestion.EndUpdateFormatting(formatting);
        }

        private static void DrawBody(Worksheet sheet, string range)
        {
            CellRange rangeInQuestion = sheet.Range[range];
            Formatting formatting = rangeInQuestion.BeginUpdateFormatting();
            formatting.Borders.SetAllBorders(Color.Black, BorderLineStyle.Hair);
            sheet.Workbook.Unit = DocumentUnit.Point;
            rangeInQuestion.RowHeight = 20;
            rangeInQuestion.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            rangeInQuestion.EndUpdateFormatting(formatting);
        }
    }
}