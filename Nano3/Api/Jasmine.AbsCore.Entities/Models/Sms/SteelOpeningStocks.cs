using System;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class SteelOpeningStocks
    {
        public int StockId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int LocationId { get; set; }
        public int? SubLocationId { get; set; }
        public int StockItemId { get; set; }
        public decimal BarCountWeight { get; set; }
        public decimal TheoreticalWeight { get; set; }
        public string ItemCode { get; set; }
        public int NoOfRolls { get; set; }

        public virtual StockLocations Location { get; set; }
        public virtual StockItems StockItem { get; set; }
        public virtual StockSubLocations SubLocation { get; set; }
    }
}
