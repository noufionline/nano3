namespace Jasmine.Core.Prism.Registrations
{
    public interface ICanRegisterRepository
    {
        ICanRegisterRepository WithRepository<TFrom, TTo>() where TTo : TFrom;
    }
}