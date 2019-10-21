using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class StockLocations
    {
        public StockLocations()
        {
            ReportRepository = new HashSet<ReportRepository>();
            SteelOpeningStocks = new HashSet<SteelOpeningStocks>();
            StockSubLocations = new HashSet<StockSubLocations>();
        }

        public int LocationId { get; set; }
        public string Location { get; set; }

        public virtual ICollection<ReportRepository> ReportRepository { get; set; }
        public virtual ICollection<SteelOpeningStocks> SteelOpeningStocks { get; set; }
        public virtual ICollection<StockSubLocations> StockSubLocations { get; set; }
    }
}
