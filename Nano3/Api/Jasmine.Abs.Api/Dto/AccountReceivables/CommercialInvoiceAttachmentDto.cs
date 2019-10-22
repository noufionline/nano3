using Jasmine.Abs.Entities;
using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class CommercialInvoiceAttachmentDto : TrackableEntityBase, IEntity
    {
        public int Id { get; set; }
        public string AttachmentTypeName { get; set; }
        public Guid? AttachmentFileId { get; set; }
        public int AttachmentTypeId { get; set; }
        public string AttachmentVersion { get; set; }
        public int InvoiceId { get; set; }
        public bool Expired { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
