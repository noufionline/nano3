namespace Jasmine.Core.Contracts
{
    public interface IValidatable
    {
        void ValidateSelf(string propertyName = null);
    }
}