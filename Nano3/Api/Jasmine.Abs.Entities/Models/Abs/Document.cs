namespace Jasmine.Abs.Entities.Models.Abs
{
    public class Document
    {
        public int DocumentId { get; set; }
        public int SalesOrderId { get; set; }
        public string ServerPath { get; set; }

        public string FileName { get; set; }
        public SalesOrder Order { get; set; }
    }
}