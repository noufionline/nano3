using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace Jasmine.Blazor.Server.Pages
{
    public class LcDocumentsBase :ComponentBase
    {

        [Parameter]
        public IEnumerable<LcDocumentList> Documents { get; set; }

        [Parameter]
        public string AccessToken{get;set;}

    [Inject]
    public ILcDocumentService Service{get;set;}

        protected override async Task OnInitializedAsync()
        {
            //Documents = await Service.GetDocumentsAsync();
            AccessToken = await Service.GetAccessTokenAsync();
            await base.OnInitializedAsync();
        }
    }


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
