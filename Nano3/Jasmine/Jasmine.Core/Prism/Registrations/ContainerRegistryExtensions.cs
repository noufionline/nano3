using Prism.Ioc;

namespace Jasmine.Core.Prism.Registrations
{
    public static class ContainerRegistryExtensions
    {
        public static ICanRegisterCollectionView Register(this IContainerRegistry registry)
        {
            return new FluentContainerRegistry(registry);
        }
    }
}