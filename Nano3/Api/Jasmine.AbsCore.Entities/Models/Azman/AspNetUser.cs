using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class AspNetUser:TrackableEntityBase
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        public string Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int DivisionId { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }

        public AbsDivision Division { get; set; }
        public ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
