using Jasmine.Core.Tracking;


namespace Jasmine.Core.Audit
{
    public class AuditLogLine
    {
        public int Id { get; set; }
        public int PrimaryKey { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public object Before { get; set; }
        public object After { get; set; }
        public TrackingState TrackingState { get; set; }
    }
}