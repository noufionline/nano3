﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class Division
    {
        public Division()
        {
            QuotationEmails = new HashSet<QuotationEmail>();
            Quotations = new HashSet<Quotation>();
            SalesPersonsByDivisions = new HashSet<SalesPersonsByDivision>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        public string Abbr { get; set; }
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

        [InverseProperty("Division")]
        public virtual ICollection<QuotationEmail> QuotationEmails { get; set; }
        [InverseProperty("Division")]
        public virtual ICollection<Quotation> Quotations { get; set; }
        [InverseProperty("Division")]
        public virtual ICollection<SalesPersonsByDivision> SalesPersonsByDivisions { get; set; }
    }
}
