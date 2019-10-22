using System.ComponentModel.DataAnnotations;

namespace Jasmine.Abs.Entities.Models.Abs
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public int CustomerId { get; set; }
        public string ProjectName { get; set; }
        public string NameOnPartner { get; set; }
        public int? PartnerProjectId { get; set; }
        public bool Mapped { get; set; }
        //[ForeignKey("CustomerId")]
        //[InverseProperty("Projects")]
        public Customer Customer { get; set; }
    }
}