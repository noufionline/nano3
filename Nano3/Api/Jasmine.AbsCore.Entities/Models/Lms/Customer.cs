using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class Customer:TrackableEntityBase
    {
        public Customer()
        {
            OutboundDocuments = new HashSet<OutboundDocument>();
            Projects = new HashSet<Project>();
        }

        public int CustomerId { get; set; }
        [Required]
        [StringLength(255)]
        public string CustomerName { get; set; }

        [InverseProperty("Customer")]
        public ICollection<OutboundDocument> OutboundDocuments { get; set; }
        [InverseProperty("Customer")]
        public ICollection<Project> Projects { get; set; }
    }
}
