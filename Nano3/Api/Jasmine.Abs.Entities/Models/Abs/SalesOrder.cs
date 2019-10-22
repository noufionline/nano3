using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Abs
{
    public class SalesOrder
    {
        public SalesOrder()
        {
            Documents = new List<Document>();
        }
        public int SalesOrderId { get; set; }
        public string OurOrderRef { get; set; }

        public ICollection<Document> Documents { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public int OrderStatusId { get; set; }
    }
}