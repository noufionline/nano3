namespace Jasmine.Core.Contracts
{
    public class ReportOptions : IReportOptions
    {
        public int Rank { get; set; }
        public string ReportHeading { get; set; }
        public string ReportSubHeading { get; set; }
        public string ReportDivision { get; set; }
        public string ReportRangeHeading { get; set; }
    }
}