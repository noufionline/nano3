using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.AutoMapper
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
              CreateMap<Jasmine.Abs.Entities.Models.Abs.Customer,Customer>()
                .ForMember(d=> d.Id,opt=> opt.MapFrom(s=> s.CustomerId))
                .ForMember(d=> d.Name,opt=> opt.MapFrom(s=> s.CustomerName))
                .ForMember(d=> d.PartnerId,opt=> opt.MapFrom(s=> s.PartnerId));
        }
    }
}
