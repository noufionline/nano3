using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class AbsDatabas
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string InitialCatalog { get; set; }
        [Required]
        [StringLength(50)]
        public string DivisionName { get; set; }
    }
}
