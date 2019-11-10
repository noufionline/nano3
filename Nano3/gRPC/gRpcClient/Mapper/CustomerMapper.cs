using AutoMapper;
using GrpcService;
using System;
using System.Collections.Generic;
using System.Text;

namespace gRpcClient.Mapper
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, CustomerList>()
            //    .ForMember(d=> d.Salary,opt=> opt.MapFrom(s=> new GrpcDecimal(s.Salary.Units,s.Salary.Nanos)))
              //  .ForMember(d => d.CreatedDate, opt => opt.MapFrom(s => s.CreatedDate.ToDateTimeOffset().LocalDateTime))
                .ForMember(d=> d.Projects,opt=> opt.MapFrom(s=> s.Projects));

            CreateMap<Project,ProjectList>();
        }

    }



    //public partial class GrpcDecimal
    //{
    //    private const decimal NanoFactor = 1_000_000_000;
    //    public GrpcDecimal(long units, int nanos)
    //    {
    //        Units = units;
    //        Nanos = nanos;
    //    }

    //    public long Units { get; }
    //    public int Nanos { get; }

    //    public static implicit operator decimal(Decimal grpcDecimal)
    //    {
    //        return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
    //    }

    //    public static implicit operator Decimal(decimal value)
    //    {
    //        var units = decimal.ToInt64(value);
    //        var nanos = decimal.ToInt32((value - units) * NanoFactor);
    //        return new Decimal(units, nanos);
    //    }


    //}


    public class CustomerList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? PartnerId { get; set; }

        public decimal Salary { get; set; }

        public List<ProjectList> Projects{get;set;}
    }

    public class ProjectList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    //TODO implement Decimal Conversion

    //public partial class GrpcDecimal
    //{
    //    private const decimal NanoFactor = 1_000_000_000;
    //    public GrpcDecimal(long units, int nanos)
    //    {
    //        Units = units;
    //        Nanos = nanos;
    //    }

    //    public long Units { get; }
    //    public int Nanos { get; }

    //    public static implicit operator decimal(CustomTypes.Decimal grpcDecimal)
    //    {
    //        return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
    //    }

    //    public static implicit operator CustomTypes.Decimal(decimal value)
    //    {
    //        var units = decimal.ToInt64(value);
    //        var nanos = decimal.ToInt32((value - units) * NanoFactor);
    //        return new CustomTypes.Decimal(units, nanos);
    //    }
    //}
}


namespace GrpcService
{
    public partial class DecimalValue
    {
        private const decimal NanoFactor = 1_000_000_000;

        public DecimalValue(long units, int nanos)
        {
            Units = units;
            Nanos = nanos;
        }

        public static implicit operator decimal(DecimalValue decimalValue) => decimalValue.ToDecimal();

        public static implicit operator DecimalValue(decimal value) => FromDecimal(value);

        public decimal ToDecimal()
        {
            return Units + Nanos / NanoFactor;
        }

        public static DecimalValue FromDecimal(decimal value)
        {
            var units = decimal.ToInt64(value);
            var nanos = decimal.ToInt32((value - units) * NanoFactor);
            return new DecimalValue(units, nanos);
        }
    }
}
