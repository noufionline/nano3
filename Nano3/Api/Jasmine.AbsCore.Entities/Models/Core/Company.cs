using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class Company
    {
        public Company()
        {
            AccountReceivables = new HashSet<AccountReceivable>();
            BankDepositSlips = new HashSet<BankDepositSlip>();
            LcDocuments = new HashSet<LcDocument>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(250)]
        public string BankName { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountNo { get; set; }
        [Required]
        [StringLength(50)]
        public string Branch { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        public byte[] RowVersion { get; set; }

        [InverseProperty("Company")]
        public virtual ICollection<AccountReceivable> AccountReceivables { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<BankDepositSlip> BankDepositSlips { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<LcDocument> LcDocuments { get; set; }
    }
}
