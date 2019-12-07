using System.Collections;
using System.Collections.Generic;
using DevExpress.Data;
using DevExpress.Spreadsheet;

namespace Jasmine.Core.Contracts
{
    public interface IWorkSheetBuilder
    {
        Worksheet CreateWorkSheet<T>(List<T> items, Worksheet worksheet, string range) where T : class;
        Worksheet CreateWorkSheet(IList items, Worksheet worksheet, string range);
        Worksheet CreateWorkSheet(IDataContainerBase report, Worksheet worksheet);
    }
}