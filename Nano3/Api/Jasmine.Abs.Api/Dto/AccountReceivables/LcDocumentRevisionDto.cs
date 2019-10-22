using Jasmine.Abs.Entities;
using System;


namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public partial class LcDocumentRevisionDto : TrackableEntityBase
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime NegotiationDate { get; set; }
        public bool Active { get; set; }
        public string Remarks { get; set; }
        public byte[] RowVersion { get; set; }

    }

    public partial class LcDocumentRevisionForPrintDto
    {
        public DateTime DeliveryDate { get; set; }
        public DateTime NegotiationDate { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}