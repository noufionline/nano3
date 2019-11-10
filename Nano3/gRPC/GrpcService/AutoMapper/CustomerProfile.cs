using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using GrpcService.Dto;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Linq;
using System.Threading.Tasks;
using EF = Jasmine.Abs.Entities.Models.Abs;
namespace GrpcService.AutoMapper
{
    public partial class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            //CreateMap<EF.Project, ProjectDto>()
            //  .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ProjectId))
            //  .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ProjectName));

            CreateMap<EF.Customer, CustomerDto>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CustomerId))
              .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CustomerName))
              .ForMember(d => d.Projects, opt => opt.Ignore()); //opt.MapFrom(s => s.Projects));

            CreateMap<CustomerDto, Customer>()
                .ForMember(d => d.Salary, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore());


            CreateMap<SteelDeliveryNoteDetailReportCriteriaRequest, SteelDeliveryNoteDetailReportCriteriaDto>(MemberList.Destination);
            CreateMap<SteelDeliveryNoteDetailReportData, SteelDeliveryNoteDetailReportDataResponse>().ReverseMap();
                //.ForMember(d=> d.DeliveredDate,opt=> opt.AllowNull());
                

            //.ForMember(d => d.Projects, opt => opt.MapFrom(s => s.Projects));

            //CreateMap<ProjectDto, Project>()
            //    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            //    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

        }
    }


    public class NullStringConverter : ITypeConverter<string, string>
    {
        public string Convert(string source)
        {
            return source ?? string.Empty;
        }

        public string Convert(string source, string destination, ResolutionContext context)
        {
            if (source == null)
            {
                return string.Empty;
            }

            return source;
        }
    }
}
