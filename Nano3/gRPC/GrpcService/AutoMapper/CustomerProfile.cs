using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF = Jasmine.Abs.Entities.Models.Abs;
namespace GrpcService.AutoMapper
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<EF.Project, ProjectDto>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ProjectId))
              .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ProjectName));

            CreateMap<EF.Customer, CustomerDto>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CustomerId))
              .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CustomerName))
              .ForMember(d => d.PartnerId, opt => opt.MapFrom(s => s.PartnerId))
              .ForMember(d => d.Projects, opt => opt.MapFrom(s => s.Projects));

            CreateMap<CustomerDto, Customer>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.PartnerId, opt => opt.MapFrom(s => s.PartnerId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Salary, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.Projects, opt => opt.MapFrom(s => s.Projects));

            CreateMap<ProjectDto, Project>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

        }

      
        public class CustomerDto
        {
            public int Id { get; set; }
            public int? PartnerId { get; set; }
            public string Name { get; set; }
            public decimal Salary { get; set; }
            public DateTime CreatedDate { get; set; }

            public List<ProjectDto> Projects { get; set; }
        }

        public class ProjectDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
