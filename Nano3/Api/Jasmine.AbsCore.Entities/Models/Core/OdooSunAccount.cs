using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class OdooSunAccount
    {
        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        [StringLength(250)]
        public string PartnerName { get; set; }
        public int? ProjectId { get; set; }
        [StringLength(250)]
        public string ProjectName { get; set; }
        [Required]
        [StringLength(50)]
        public string SunAccountCode { get; set; }
        [StringLength(255)]
        public string SunDb { get; set; }
        public bool IsProject { get; set; }
    }
}
