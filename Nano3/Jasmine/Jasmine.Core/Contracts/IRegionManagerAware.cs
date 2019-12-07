using Prism.Regions;

namespace Jasmine.Core.Contracts
{
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}