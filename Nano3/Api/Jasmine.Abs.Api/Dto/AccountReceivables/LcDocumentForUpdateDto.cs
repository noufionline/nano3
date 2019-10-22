using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class LcDocumentForUpdateDto : LcDocumentDto
    {

    }

    public class LcDocumentForPrintDto
    {
        public string CustomerName { get; set; }
        public string AccountCode { get; set; }
        public string ClientBankName { get; set; }
        public string ClientLcNo { get; set; }
        public string OurBankName { get; set; }
        public string OurLcNo { get; set; }

        public DateTime OpeningDate { get; set; }

        public List<LcDocumentRevisionForPrintDto> LcDocumentRevisions { get; set; }
        public List<CommercialInvoiceLineForPrintDto> CommercialInvoices { get; set; }

    }

}