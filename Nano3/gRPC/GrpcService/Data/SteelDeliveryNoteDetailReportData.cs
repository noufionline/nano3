using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService
{
    public class SteelDeliveryNoteDetailReportData
    {
        public string Type { get; set; }
        public int DONo { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string CustomerName { get; set; }
        public string SunAccountCode { get; set; }
        public string VatRegNo { get; set; }
        public string ProjectName { get; set; }
        public string AccountCode { get; set; }
        public string InvoicedByName { get; set; }
        public string SuppliedBy { get; set; }
        public string MovementType { get; set; }
        public string DeliveryTypeName { get; set; }
        public string ProductName { get; set; }
        public string Abbr { get; set; }
        public string Origin { get; set; }
        public string OrderRef { get; set; }
        public string SubOrderRef { get; set; }
        public string Code { get; set; }
        public string BBSRef { get; set; }
        public string OtherRef { get; set; }
        public decimal D08MM { get; set; }
        public decimal D10MM { get; set; }
        public decimal D12MM { get; set; }
        public decimal D14MM { get; set; }
        public decimal D16MM { get; set; }
        public decimal D18MM { get; set; }
        public decimal D20MM { get; set; }
        public decimal D22MM { get; set; }
        public decimal D25MM { get; set; }
        public decimal D28MM { get; set; }
        public decimal D32MM { get; set; }
        public decimal D40MM { get; set; }

    }

    public class SteelDeliveryNoteDetailReportCriteria
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        public int? CustomerId { get; set; }
        public int? ProjectId { get; set; }                  
        public int? ProductGroupId { get; set; }                  
        public int? ProductCategoryId { get; set; }
        public int? ProductId { get; set; }
        public int? MovementTypeId { get; set; }
        public int? InvoicedById { get; set; }
        public int? SalesOrderId { get; set; }
        public int? DeliveryTypeId { get; set; }
        public int? SubOrderId { get; set; }
        public int? Delivered { get; set; }
        public string WeightType { get; set; } = " C";



    }


    public class SteelDeliveryNoteDetailReportDataMap : EntityMap<SteelDeliveryNoteDetailReportData>
    {
        public SteelDeliveryNoteDetailReportDataMap()
        {
            Map(u => u.D08MM).ToColumn("08MM");
            Map(u => u.D10MM).ToColumn("10MM");
            Map(u => u.D12MM).ToColumn("12MM");
            Map(u => u.D14MM).ToColumn("14MM");
            Map(u => u.D16MM).ToColumn("16MM");
            Map(u => u.D18MM).ToColumn("18MM");
            Map(u => u.D20MM).ToColumn("20MM");
            Map(u => u.D22MM).ToColumn("22MM");
            Map(u => u.D25MM).ToColumn("25MM");
            Map(u => u.D28MM).ToColumn("28MM");
            Map(u => u.D32MM).ToColumn("32MM");
            Map(u => u.D40MM).ToColumn("40MM");
        }
    }
}
