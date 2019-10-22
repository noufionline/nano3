using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("SalesPersonsByDivision")]
    public partial class SalesPersonsByDivision : TrackableEntityBase
    {
        [Key]
        public int DivisionId { get; set; }
        [Key]
        public int SalesPersonId { get; set; }
        [Required]
        public byte[] RowVersion { get; set; }

        [ForeignKey("DivisionId")]
        [InverseProperty("SalesPersonsByDivisions")]
        public virtual Division Division { get; set; }
        [ForeignKey("SalesPersonId")]
        [InverseProperty("SalesPersonsByDivisions")]
        public virtual SalesPerson SalesPerson { get; set; }
    }
}
