namespace Jasmine.Core.Attributes
{
    public interface IColumnWithFormat
    {
        string Format { get; }
        //string Column { get; set; }
        bool Summary { get; set; }
        string Heading { get; set; }
    }
}