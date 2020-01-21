using System;

namespace AbsCore.Blazor.Server.Data
{
    public class LcDocumentList
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientBankName { get; set; }
        public string ClientLcNo { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }

        public DateTime OpeningDate { get; set; }
    }
}