using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class Project:TrackableEntityBase
    {
        public Project()
        {
            OutboundDocuments = new HashSet<OutboundDocument>();
        }

        public int ProjectId { get; set; }
        [Required]
        [StringLength(150)]
        public string ProjectName { get; set; }
        public int CustomerId { get; set; }
        public int? DivisionId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Projects")]
        public Customer Customer { get; set; }
        [InverseProperty("Project")]
        public ICollection<OutboundDocument> OutboundDocuments { get; set; }
    }
}
