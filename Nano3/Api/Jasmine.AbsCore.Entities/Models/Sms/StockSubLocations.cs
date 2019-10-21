using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class StockSubLocations
    {
        public StockSubLocations()
        {
            ReportRepository = new HashSet<ReportRepository>();
            SteelOpeningStocks = new HashSet<SteelOpeningStocks>();
        }

        public int SubLocationId { get; set; }
        public string SubLocation { get; set; }
        public int LocationId { get; set; }

        public virtual ICollection<ReportRepository> ReportRepository { get; set; }
        public virtual ICollection<SteelOpeningStocks> SteelOpeningStocks { get; set; }
        public virtual StockLocations Location { get; set; }
    }
}
