using DevExpress.Blazor.Server.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExpress.Blazor.Server.Pages
{
    public partial class FetchData
    {
        public List<LcDocumentList> documents;

        [Inject]
        ILcDocumentService ForecastService{get;set; }

        protected override async Task OnInitializedAsync()
        {
            documents = await ForecastService.GetLcDocumentsAsync();
            //summaries = await ForecastService.GetSummariesAsync();
        }
        async void OnRowRemoving(LcDocumentList dataItem)
        {

            //  documents = await ForecastService.Remove(dataItem);
            await InvokeAsync(StateHasChanged);
        }
        async void OnRowUpdating(LcDocumentList dataItem, Dictionary<string, object> newValue)
        {
            //   documents = await ForecastService.Update(dataItem, newValue);
            await InvokeAsync(StateHasChanged);
        }
        async void OnRowInserting(Dictionary<string, object> newValue)
        {
            //  documents = await ForecastService.Insert(newValue);
            await InvokeAsync(StateHasChanged);
        }
    }
}
