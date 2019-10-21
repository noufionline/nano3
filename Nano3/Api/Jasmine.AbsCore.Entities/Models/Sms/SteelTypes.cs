using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class SteelTypes
    {
        public SteelTypes()
        {
            ReportRepository = new HashSet<ReportRepository>();
            SteelProducts = new HashSet<SteelProducts>();
        }

        public int SteelTypeId { get; set; }
        public string SteelType { get; set; }

        public virtual ICollection<ReportRepository> ReportRepository { get; set; }
        public virtual ICollection<SteelProducts> SteelProducts { get; set; }
    }
}
