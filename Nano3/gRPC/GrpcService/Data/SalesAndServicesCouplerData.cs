using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService
{

    public class SalesAndServicesCouplerData
    {
        public string ProductName { get; set; }
        public int DONo { get; set; }
        public string Date { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string InvoicedByName { get; set; }
        public string SuppliedBy { get; set; }
        public string MovementType { get; set; }
        public string OrderRef { get; set; }
        public string Diameter { get; set; }
        public decimal Qty { get; set; }

    }

    public class CouplerDeliveryNoteDetailReportCriteria
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
      
        public int? Delivered { get; set; }




    }















}