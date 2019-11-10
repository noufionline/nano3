using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService
{
    public class MaterialTransferOthersData
    {
        public string Type { get; set; }
        public int DONo { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public string MovementType { get; set; } = "CICON STEEL";
        public string CustomerName { get; set; }
    }

    public class MaterialTransferOthersDataMap : EntityMap<MaterialTransferOthersData>
    {
        public MaterialTransferOthersDataMap()
        {



            Map(u => u.CustomerName).ToColumn("DivisionName");
            Map(u => u.MovementType).ToColumn("DeliveryTypeName");









        }







    }

    public class MaterialTransferOthersDataCriteria
    {

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int? DivisionId { get; set; }
        public int? ProductGroupId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductId { get; set; }





    }















}
