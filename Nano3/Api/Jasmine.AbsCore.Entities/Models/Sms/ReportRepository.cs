namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class ReportRepository
    {
        public int RepositoryId { get; set; }
        public int SessionId { get; set; }
        public string TransactionType { get; set; }
        public int LocationId { get; set; }
        public int? SubLocationId { get; set; }
        public int SteelTypeId { get; set; }
        public string ProductName { get; set; }
        public string Origin { get; set; }
        public string OriginName { get; set; }
        public string Diameter { get; set; }
        public decimal Length { get; set; }
        public bool IsSpecialLength { get; set; }
        public decimal BarCountWeight { get; set; }
        public decimal TheoreticalWeight { get; set; }
        public int NoOfRolls { get; set; }

        public virtual StockLocations Location { get; set; }
        public virtual UserSessions Session { get; set; }
        public virtual SteelTypes SteelType { get; set; }
        public virtual StockSubLocations SubLocation { get; set; }
    }
}
