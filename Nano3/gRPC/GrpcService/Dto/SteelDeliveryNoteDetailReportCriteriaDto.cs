using System;
namespace GrpcService.Dto
{
    public class SteelDeliveryNoteDetailReportCriteriaDto
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
}
