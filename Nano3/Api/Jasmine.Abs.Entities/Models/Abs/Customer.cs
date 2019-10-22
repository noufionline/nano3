using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jasmine.Abs.Entities.Models.Abs
{
    public class Customer
    {
        public Customer()
        {
            SalesOrders = new List<SalesOrder>();
            Projects = new List<Project>();
        }
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SunAccountCode { get; set; }
        public string VatRegNo { get; set; }
        public string NameOnTradeLicense { get; set; }
        public string OldCustomerName { get; set; }
        public int? PartnerId { get; set; }
        public bool Mapped { get; set; }
        public List<SalesOrder> SalesOrders { get; set; }
        public List<Project> Projects { get; set; }
    }
}