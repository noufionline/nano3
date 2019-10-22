using AutoMapper;
using Jasmine.Abs.Api.Dto.AccountReceivables;
using Jasmine.Abs.Api.Helper;
using Jasmine.Abs.Entities.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.Mapper
{
     public class LcDocumentProfile : Profile
    {
        public LcDocumentProfile()
        {
              CreateMap<LcDocument,LcDocumentDto>()
                .ForMember(d=> d.CommercialInvoices,opt=> opt.MapFrom(s=> s.CommercialInvoices.Where(x=> x.LcDocumentId==s.Id)))
                .ForMember(d=> d.LcDocumentRevisions,opt=> opt.MapFrom(s=> s.LcDocumentRevisions.Where(x=> x.DocumentId==s.Id)));

            CreateMap<LcDocument,LcDocumentForUpdateDto>()
                .IgnoreCollectionProperties();

            
                
            
            CreateMap<LcDocumentDto,LcDocument>().ForMember(d=> d.CommercialInvoices,opt=> opt.MapFrom(s=> s.CommercialInvoices));               
            
            CreateMap<LcDocument,LcDocumentListDto>()
                .ForMember(d=> d.CustomerName,opt=> opt.MapFrom(s=> s.Partner.Name))
                .ForMember(d=> d.BankName,opt=> opt.MapFrom(s=> s.Company.BankName))
                .ForMember(d=> d.TotalLcValue, opt=> opt.MapFrom(s=> s.LcDocumentRevisions.Where(x=> x.DocumentId==s.Id).Sum(x=> x.Amount)))
                .ForMember(d=> d.AmountUtilized, opt=> opt.MapFrom(s=> s.CommercialInvoices.Where(x=> x.LcDocumentId==s.Id).Sum(x=> x.Amount)));

            CreateMap<LcDocumentRevision,LcDocumentRevisionDto>().ReverseMap();           

            CreateMap<LcDocument, LcDocumentDetailDto>()
                .ForMember(d => d.OurLcNo, opt=> opt.MapFrom(s => s.Name))
                .ForMember(d => d.OurBankName, opt=> opt.MapFrom(s => s.Company.BankName))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Partner.Name))
                .ForMember(d => d.TotalLcValue, opt => opt.MapFrom(s => s.LcDocumentRevisions.Sum(i => i.Amount)))
                .ForMember(d => d.AmountUtilized, opt => opt.MapFrom(s => s.CommercialInvoices.Sum(i => i.Amount)));



            #region Commercial Invoice

            CreateMap<CommercialInvoice,CommercialInvoiceLineDto>()
                .IgnoreForeignKeyProperties()
                .IgnoreInverseProperties();

            CreateMap<CommercialInvoiceLineDto,CommercialInvoice>();

            CreateMap<CommercialInvoice, CommercialInvoiceDto>()
               .ForMember(d => d.CommercialInvoiceTransactionHistories, opt => opt.MapFrom(s => s.CommercialInvoiceTransactionHistories.Where(x => x.InvoiceId == s.Id)))
               .ForMember(d => d.CommercialInvoiceAttachments, opt => opt.MapFrom(s => s.CommercialInvoiceAttachments.Where(x => x.InvoiceId == s.Id && !x.Expired)));


            CreateMap<CommercialInvoiceDto, CommercialInvoice>();
                //.ForMember(d => d.CommercialInvoiceAttachments, opt => opt.Ignore());

            CreateMap<CommercialInvoiceAttachmentDto, CommercialInvoiceAttachment>()
                .IgnoreForeignKeyProperties();
            CreateMap<CommercialInvoiceAttachment, CommercialInvoiceAttachmentDto>()
                .ForMember(d => d.AttachmentTypeName, opt => opt.MapFrom(s => s.AttachmentType.Name)); 

            CreateMap<CommercialInvoiceAttachmentTypeDto, CommercialInvoiceAttachmentType>().ReverseMap();

            CreateMap<CommercialInvoice, CommercialInvoiceListDto>()
                .ForMember(d => d.LcDocumentNo, opt => opt.MapFrom(s => s.LcDocument.Name))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.LcDocument.Partner.Name))
                .ForMember(d => d.SunAccountCode, opt => opt.MapFrom(s => s.LcDocument.SunAccountCode))
                .ForMember(d => d.OpeningDate, opt => opt.MapFrom(s => s.LcDocument.OpeningDate))
                .ForMember(d => d.Bank, opt => opt.MapFrom(s => s.LcDocument.ClientBankName));

            CreateMap<CommercialInvoiceTransactionHistory, CommercialInvoiceTransactionHistoryDto>().ReverseMap();

            CreateMap<CommercialInvoice, CommercialInvoiceForUpdateDto>().IgnoreCollectionProperties();


            CreateMap<LcDocument,LcDocumentForPrintDto>()
                .ForMember(dest=> dest.OurLcNo,opt=> opt.MapFrom(src=> src.Name))
                .ForMember(dest=> dest.OurBankName,opt=> opt.MapFrom(src=> src.Company.BankName))
                .ForMember(dest=> dest.CustomerName,opt=> opt.MapFrom(src=> src.Partner.Name))
                .ForMember(dest=> dest.AccountCode,opt=> opt.MapFrom(src=> src.SunAccountCode))
                .ForMember(dest=> dest.LcDocumentRevisions,opt=> opt.MapFrom(src=> src.LcDocumentRevisions.Where(x=> x.DocumentId==src.Id)))
                .ForMember(dest=> dest.CommercialInvoices,opt=> opt.Ignore());

            CreateMap<LcDocumentRevision,LcDocumentRevisionForPrintDto>();
            CreateMap<CommercialInvoice,CommercialInvoiceLineForPrintDto>();
                
            #endregion

        }
    }
}
