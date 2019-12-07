namespace Jasmine.Core.Prism.Registrations
{
    public interface ICanRegisterService
    {
        ICanRegisterServiceAndRepository WithService<TFrom, TTo>() where TTo : TFrom;
    }
}