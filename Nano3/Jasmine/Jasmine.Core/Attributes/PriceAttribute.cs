namespace Jasmine.Core.Attributes
{
    public class PriceAttribute : AttributeWithFormatBase
    {
        public override string Format => "_(* #,##0.00_);_(* (#,##0.00);_(* " + "?" + "?_);_(@_)";
    }
}