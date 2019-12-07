namespace Jasmine.Core.Attributes
{
    public class TonnageAttribute : AttributeWithFormatBase
    {
        public override string Format => "_(* #,##0.000_);_(* (#,##0.000);_(* \"-\"??_);_(@_)";
    }
}