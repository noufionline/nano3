using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("AgingFromSunSystem")]
    public partial class AgingFromSunSystem : TrackableEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountType { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountCode { get; set; }
        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }
        public int? AddressCode { get; set; }
        [Required]
        [StringLength(50)]
        public string TransactionReference { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BaseAmount { get; set; }
        [StringLength(1)]
        public string AllocationMarker { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountingPeriod { get; set; }
        [Required]
        [StringLength(50)]
        public string JournalSource { get; set; }
        public DateTime? DueDate { get; set; }
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        [StringLength(50)]
        public string PaymentTermsGroupCode { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
