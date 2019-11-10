using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService
{
    public class SalesAndServicesOthersData
    {

        public string Type { get; set; } 
        public int DONo { get; set; }
        public DateTime Date { get; set; }
        public string MovementType { get; set; }
        public string ClientName { get; set; }
        public string ProjectName { get; set; }
        public string OrderRef { get; set; }
        public string DeliveryTypeName { get; set; }
        public string ProductName { get; set; }
        public string  ItemCode { get; set; }
        public int SalesOrderId { get; set; }
        public string InvoicedByName { get; set; }
        public string Description { get; set; }
        public decimal Qty { get; set; }
        public string  Unit { get; set; }
    }


    public class SalesAndServicesOthersDataCriteria
    {


        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        public int? CustomerId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProductGroupId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductId { get; set; }

        public int? InvoicedById { get; set; }
        public int? Delivered { get; set; }
        public string WeightType { get; set; } = " C";



    }



    public class SalesAndServicesOthersDataMap : EntityMap<SalesAndServicesOthersData>
    {
        public SalesAndServicesOthersDataMap()

        {

            Map(u => u.ClientName).ToColumn("CustomerName");










        }

    }
}