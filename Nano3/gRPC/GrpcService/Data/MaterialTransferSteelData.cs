using Dapper.FluentMap.Mapping;
using System;

namespace GrpcService
{
    public class MaterialTransferSteelData
    {
        public int DONo { get; set; }
        public DateTime DeliveredDate { get; set; }
        public string MovementType { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public string DeliveryTypeName { get; set; }
        public string DivisionName { get; set; }
        public decimal _08MM { get; set; }
        public decimal _10MM { get; set; }
        public decimal _12MM { get; set; }
        public decimal _14MM { get; set; }

        public string Origin { get; set; }
        public decimal _16MM { get; set; }
        public decimal _18MM { get; set; }
        public decimal _20MM { get; set; }
        public decimal _22MM { get; set; }
        public decimal _25MM { get; set; }
        public decimal _28MM { get; set; }
        public decimal _32MM { get; set; }
        public decimal _40MM { get; set; }

    }

    public class MaterialTransferSteelDataMap : EntityMap<MaterialTransferSteelData>
    {
        public MaterialTransferSteelDataMap()
        {
            Map(u => u.Origin).ToColumn("OriginName");
            Map(u => u._08MM).ToColumn("08MM");
            Map(u => u._10MM).ToColumn("10MM");
            Map(u => u._12MM).ToColumn("12MM");
            Map(u => u._14MM).ToColumn("14MM");
            Map(u => u._16MM).ToColumn("16MM");
            Map(u => u._18MM).ToColumn("18MM");
            Map(u => u._20MM).ToColumn("20MM");
            Map(u => u._22MM).ToColumn("22MM");
            Map(u => u._25MM).ToColumn("25MM");
            Map(u => u._28MM).ToColumn("28MM");
            Map(u => u._32MM).ToColumn("32MM");
            Map(u => u._40MM).ToColumn("40MM");
        }
    }



    public class MaterialTransferSteelDataCriteria
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? DivisionId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductGroupId { get; set; }
        public int? OriginId { get; set; }
        public string WeightType { get; set; } = " C";
    }







}
