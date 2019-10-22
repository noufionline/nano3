using Jasmine.Abs.Entities;
using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class CommercialInvoiceTransactionHistoryDto : TrackableEntityBase, IEntity
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public CommercialInvoiceStatusTypes CommercialInvoiceStatus { get; set; }
        public DateTime Date { get; set; }
        public string Remarks { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
