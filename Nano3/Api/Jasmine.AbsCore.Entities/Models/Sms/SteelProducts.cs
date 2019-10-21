using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class SteelProducts
    {
        public SteelProducts()
        {
            StockItems = new HashSet<StockItems>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SteelTypeId { get; set; }

        public virtual ICollection<StockItems> StockItems { get; set; }
        public virtual SteelTypes SteelType { get; set; }
    }
}
