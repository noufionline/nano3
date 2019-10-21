using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class PaymentReceiptVoucher
    {
        [Key]
        public int Id { get; set; }
        public int ReceiptNo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime ReceiptDate { get; set; }
        public int ReceivedById { get; set; }
        public int AccountReceivableId { get; set; }
        [StringLength(150)]
        public string Comment { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("AccountReceivableId")]
        [InverseProperty("PaymentReceiptVouchers")]
        public virtual AccountReceivable AccountReceivable { get; set; }
        [ForeignKey("ReceivedById")]
        [InverseProperty("PaymentReceiptVouchers")]
        public virtual PaymentReceiver ReceivedBy { get; set; }
    }
}
