using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.Dto.Abs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SunAccountCode { get; set; }
        public string VatRegNo { get; set; }
        public string NameOnTradeLicense { get; set; }
    }
}
