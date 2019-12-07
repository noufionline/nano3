namespace Jasmine.Core.Attributes
{
    public abstract class AttributeWithFormatBase : AttributeBase, IColumnWithFormat
    {
        public abstract string Format { get; }
    }
}