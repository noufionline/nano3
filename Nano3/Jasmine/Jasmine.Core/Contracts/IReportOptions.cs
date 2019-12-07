using PostSharp.Patterns.Contracts;

namespace Jasmine.Core.Contracts
{
    public interface IReportOptions
    {
        [StrictlyPositive]
        int Rank { get; set; }
        string ReportHeading { get; set; }
        string ReportSubHeading { get; set; }
        string ReportDivision { get; set; }
        string ReportRangeHeading { get; set; }
    }
}