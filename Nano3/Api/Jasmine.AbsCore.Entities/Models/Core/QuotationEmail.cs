using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class QuotationEmail
    {
        [Key]
        public int Id { get; set; }
        public int DivisionId { get; set; }
        [Required]
        [StringLength(50)]
        public string StateTrigger { get; set; }
        [StringLength(250)]
        public string Emails { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("DivisionId")]
        [InverseProperty("QuotationEmails")]
        public virtual Division Division { get; set; }
    }
}
