using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService
{
    public class SalesAndServicesThreadingData
    {
        public string ProductName { get; set; }
        public string Type { get; set; }
        public int DONo { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string SunAccountCode { get; set; }
        public string VatRegNo { get; set; }
        public string ProjectName { get; set; }
        public string AccountCode { get; set; }
        public string BBSRef { get; set; }
        public string Code { get; set; }
        public string InvoicedByName { get; set; }
        public string SuppliedBy { get; set; }
        public string MovementType { get; set; }
        public decimal S12 { get; set; }
        public decimal S16 { get; set; }
        public decimal S20 { get; set; }
        public decimal S25 { get; set; }
        public decimal S32 { get; set; }
        public decimal S40 { get; set; }
        public decimal E12 { get; set; }
        public decimal E16 { get; set; }
        public decimal E20 { get; set; }
        public decimal E25 { get; set; }
        public decimal E32 { get; set; }
        public decimal E40 { get; set; }

    }


    public class SalesAndServicesThreadingCriteria
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
    

        public int? Delivered { get; set; }
    }





    public class SalesAndServicesThreadingDataMap : EntityMap<SalesAndServicesThreadingData>
    {
        public SalesAndServicesThreadingDataMap()

        {

            Map(u => u.S12).ToColumn("THRD012");
            Map(u => u.S16).ToColumn("THRD016");
            Map(u => u.S20).ToColumn("THRD020");
            Map(u => u.S25).ToColumn("THRD025");
            Map(u => u.S32).ToColumn("THRD032");
            Map(u => u.S40).ToColumn("THRD040");
            Map(u => u.E12).ToColumn("THRDE012");
            Map(u => u.E16).ToColumn("THRDE016");
            Map(u => u.E20).ToColumn("THRDE020");
            Map(u => u.E25).ToColumn("THRDE025");
            Map(u => u.E32).ToColumn("THRDE032");
            Map(u => u.E40).ToColumn("THRDE040");






        }







    }


}






















