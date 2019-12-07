using System;
using System.Windows.Markup;

namespace Jasmine.Core.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof((string route,string title)))]
    public class LookupItemMarkupExtension : MarkupExtension
    {
        public string Route { get; set; }
        public string Title { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return (Route, Title);
        }
    }
}