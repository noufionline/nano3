using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class StockItems
    {
        public StockItems()
        {
            SteelOpeningStocks = new HashSet<SteelOpeningStocks>();
        }

        public int StockItemId { get; set; }
        public string ItemCode { get; set; }
        public int ProductId { get; set; }
        public int OriginId { get; set; }
        public string Diameter { get; set; }
        public decimal Length { get; set; }

        public virtual ICollection<SteelOpeningStocks> SteelOpeningStocks { get; set; }
        public virtual SteelOrigins Origin { get; set; }
        public virtual SteelProducts Product { get; set; }
    }
}
