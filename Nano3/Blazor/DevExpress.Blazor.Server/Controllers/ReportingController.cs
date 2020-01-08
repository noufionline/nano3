using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Compatibility.System.Web;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;

namespace DevExpress.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    public class ReportingController : Controller
    {
        public ReportingController() { }
        [Route("[action]", Name = "getReportDesignerModel")]
        public object GetReportDesignerModel(string reportUrl)
        {
            string modelJsonScript = new ReportDesignerClientSideModelGenerator(HttpContext.RequestServices)
                .GetJsonModelScript(reportUrl, null, "/DXXRD", "/DXXRDV", "/DXQB");
            return new JavaScriptSerializer().Deserialize<object>(modelJsonScript);
        }
    }



    public class CustomReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        public override bool CanSetData(string url)
        {
            // Check if the URL is available in the report storage.
            var item = ReportFactory.Reports.FirstOrDefault(x => x.Name == url);
            return item != null;
        }


        public override byte[] GetData(string url)
        {
            // Get the report data from the storage.
            var item = ReportFactory.Reports.FirstOrDefault(x => x.Name == url);
            using (MemoryStream ms = new MemoryStream())
            {
                item.Report.SaveLayoutToXml(ms);
                return ms.ToArray();
            }
        }


        public override Dictionary<string, string> GetUrls()
        {
            // Get URLs and display names for all reports available in the storage.
            var dictionary = new Dictionary<string, string>();
            foreach (var item in ReportFactory.Reports)
            {
                dictionary.Add(item.Name, item.DisplayName);
            }
            return dictionary;
        }


        public override bool IsValidUrl(string url)
        {
            // Check if the specified URL is valid for the current report storage.
            var item = ReportFactory.Reports.FirstOrDefault(x => x.Name == url);
            return item != null;
        }


        public override void SetData(XtraReport report, string url)
        {
            // Write a report to the storage under the specified URL.
            var item = ReportFactory.Reports.FirstOrDefault(x => x.Name == url);
            if (item != null)
            {
                item.Report = report;
            }
        }


        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            // Write a report to the storage under a new URL.
            var item = ReportFactory.Reports.FirstOrDefault(x => x.DisplayName == defaultUrl);
            if (item != null)
            {
                item.Report = report;
                return item.Name;
            }
            else
            {
                var url = ReportFactory.Reports.Count.ToString();
                ReportFactory.Reports.Add(new ReportInfo { DisplayName = defaultUrl, Name = url, Report = report });
                return url;
            }
        }
    }


    public class ReportInfo
    {
        public XtraReport Report { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
    public class ReportFactory
    {
        public readonly static List<ReportInfo> Reports = new List<ReportInfo>();

        public static XtraReport CreateReport()
        {
            var report = new XtraReport();
            var band = new DetailBand();
            band.HeightF = 200f;
            band.Controls.Add(new XRLabel()
            {
                Text = "Hello World!",
                SizeF = new System.Drawing.SizeF(410, 90),
                LocationFloat = new DevExpress.Utils.PointFloat(119, 5),
                Font = new System.Drawing.Font("Times New Roman", 48F, System.Drawing.FontStyle.Bold),
                Padding = new PaddingInfo(2, 2, 0, 96, System.Drawing.GraphicsUnit.Pixel)
            });
            report.Bands.Add(band);
            return report;
        }

        static ReportFactory()
        {
            Reports.Add(new ReportInfo() { DisplayName = "My Report", Name = "MyReport", Report = CreateReport() });
        }
    }
}