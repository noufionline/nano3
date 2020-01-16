using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace is4aspid.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public int DivisionId { get; set; }
        public byte[] Photo { get; set; }
    }
}
