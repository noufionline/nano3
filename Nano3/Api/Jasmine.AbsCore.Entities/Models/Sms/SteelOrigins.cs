using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class SteelOrigins
    {
        public SteelOrigins()
        {
            StockItems = new HashSet<StockItems>();
        }

        public int OriginId { get; set; }
        public string OriginName { get; set; }
        public string OriginCode { get; set; }

        public virtual ICollection<StockItems> StockItems { get; set; }
    }
}
