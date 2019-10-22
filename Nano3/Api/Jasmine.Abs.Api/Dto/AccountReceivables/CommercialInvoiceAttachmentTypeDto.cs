using Jasmine.Abs.Entities;
using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class CommercialInvoiceAttachmentTypeDto : TrackableEntityBase, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
