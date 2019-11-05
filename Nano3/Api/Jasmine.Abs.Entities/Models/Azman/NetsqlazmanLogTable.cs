using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class NetsqlazmanLogTable : TrackableEntityBase
    {
        public int LogId { get; set; }
        public DateTime LogDateTime { get; set; }
        public string WindowsIdentity { get; set; }
        public string SqlIdentity { get; set; }
        public string MachineName { get; set; }
        public Guid InstanceGuid { get; set; }
        public Guid? TransactionGuid { get; set; }
        public int OperationCounter { get; set; }
        public string Enstype { get; set; }
        public string Ensdescription { get; set; }
        public string LogType { get; set; }
    }
}
