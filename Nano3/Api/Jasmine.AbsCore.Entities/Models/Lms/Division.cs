using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class Division:TrackableEntityBase
    {
        public Division()
        {
            InboundDocuments = new HashSet<InboundDocument>();
            InboundTripMasters = new HashSet<InboundTripMaster>();
            OutboundDocuments = new HashSet<OutboundDocument>();
            OutboundTripMasters = new HashSet<OutboundTripMaster>();
        }

        public int DivisionId { get; set; }
        [Required]
        [StringLength(50)]
        public string DivisionName { get; set; }
        [Required]
        [Column("DMSDBName")]
        [StringLength(50)]
        public string Dmsdbname { get; set; }
        public int ExtraAllowed { get; set; }
        [Column("Day_Lag")]
        public int DayLag { get; set; }
        [Required]
        public bool? Active { get; set; }
        [StringLength(5)]
        public string Abbr { get; set; }
        [StringLength(10)]
        public string Header { get; set; }

        [InverseProperty("Division")]
        public ICollection<InboundDocument> InboundDocuments { get; set; }
        [InverseProperty("ChargedDivision")]
        public ICollection<InboundTripMaster> InboundTripMasters { get; set; }
        [InverseProperty("Division")]
        public ICollection<OutboundDocument> OutboundDocuments { get; set; }
        [InverseProperty("ChargedDivision")]
        public ICollection<OutboundTripMaster> OutboundTripMasters { get; set; }
    }
}
