using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("CustomersFromSunSystemBackup")]
    public partial class CustomersFromSunSystemBackup : TrackableEntityBase
    {
        [Key]
        [StringLength(15)]
        public string CustomerCode { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(15)]
        public string AddressCode { get; set; }
        [StringLength(50)]
        public string Line1 { get; set; }
        [StringLength(50)]
        public string Line2 { get; set; }
        [StringLength(50)]
        public string Line3 { get; set; }
        [StringLength(50)]
        public string Line4 { get; set; }
        [StringLength(50)]
        public string Line5 { get; set; }
        [StringLength(50)]
        public string TownOrCity { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(50)]
        public string TelephoneNumber { get; set; }
        [StringLength(50)]
        public string FaxNumber { get; set; }
        [StringLength(100)]
        public string WebPageAddress { get; set; }
        [StringLength(100)]
        public string Comment { get; set; }
        [StringLength(50)]
        public string ShortHeading { get; set; }
        [StringLength(50)]
        public string LookupCode { get; set; }
        [StringLength(50)]
        public string Area { get; set; }
        [StringLength(50)]
        public string CountryCode { get; set; }
        [StringLength(50)]
        public string StateCode { get; set; }
        [StringLength(50)]
        public string TaxIdentificationCode { get; set; }
        [Required]
        [StringLength(15)]
        public string ActualAccount { get; set; }
        public int? PartnerId { get; set; }
        public int? ProjectId { get; set; }
    }
}
