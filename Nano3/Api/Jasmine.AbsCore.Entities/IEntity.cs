namespace Jasmine.AbsCore.Entities
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IConcurrency
    {
        byte[] RowVersion { get; set; }
    }

    public interface ILookupItem : IEntity
    {
        string Name { get; set; }
    }

    public class LookupItem : ILookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }



    public interface ILookupItemModel : ILookupItem, IConcurrency
    {

    }

    public class AbsApplicationInfo : LookupItem
    {
        public string ApplicationType { get; set; }
    }
}

