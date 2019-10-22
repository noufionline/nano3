using Jasmine.Abs.Entities;
using System;
using System.Collections.Generic;


namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class LcDocumentDto : TrackableEntityBase,IEntity
    {
        public LcDocumentDto()
        {
            CommercialInvoices = new List<CommercialInvoiceLineDto>();
            LcDocumentRevisions = new List<LcDocumentRevisionDto>();
        }

        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string SunAccountCode { get; set; }
        public string ClientBankName { get; set; }
        public string ClientLcNo { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime OpeningDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public ICollection<CommercialInvoiceLineDto> CommercialInvoices { get; set; }
        public ICollection<LcDocumentRevisionDto> LcDocumentRevisions { get;  set; }
    }
}