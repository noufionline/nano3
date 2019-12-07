using System;

namespace Jasmine.Core.Attributes
{
    public class ExcelReportAttribute : Attribute
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }

        public string Range { get; set; }
    }
}