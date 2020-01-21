using AbsCore.Blazor.Server.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace AbsCore.Blazor.Server.Pages
{
    public class LcDocumentsBase : ComponentBase
    {

        [Parameter]
        public IEnumerable<LcDocumentList> Documents { get; set; }

        


        [Inject]
        public ILcDocumentService Service { get; set; }

        
        protected override async Task OnInitializedAsync()
        {
            Documents = await Service.GetDocumentsAsync();
        }
    }


   
}
