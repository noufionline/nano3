using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Zeon
{
    public partial class AbsDivision : TrackableEntityBase
    {
        public AbsDivision()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? RegionId { get; set; }
        public string StoreName { get; set; }
        public string ApplicationName { get; set; }
        public string InitialCatelog { get; set; }
        public string ApplicationType { get; set; }

        public ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
