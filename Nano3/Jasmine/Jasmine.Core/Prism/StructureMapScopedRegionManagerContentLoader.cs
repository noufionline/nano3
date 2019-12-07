using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Prism.Regions;
using StructureMap;

namespace Jasmine.Core.Prism
{
    public class StructureMapScopedRegionManagerContentLoader:ScopedRegionManagerContentLoader
    {
        private readonly IContainer _container;

        public StructureMapScopedRegionManagerContentLoader(IServiceLocator serviceLocator,IContainer container) : base(serviceLocator)
        {
            _container = container;
        }

        protected override IEnumerable<object> GetCandidatesFromRegion(IRegion region, string candidateNavigationContract)
        {
            if (candidateNavigationContract == null || candidateNavigationContract.Equals(string.Empty))
                throw new ArgumentNullException(nameof(candidateNavigationContract));

            IEnumerable<object> contractCandidates = base.GetCandidatesFromRegion(region, candidateNavigationContract).ToArray();

            if (!contractCandidates.Any())
            {
                //First try friendly name registration. If not found, try type registration
                var matchingRegistration = _container.Model.AllInstances.FirstOrDefault(r => candidateNavigationContract.Equals(r.Name, StringComparison.Ordinal));
                if (matchingRegistration == null)
                {
                    matchingRegistration = _container.Model.AllInstances.FirstOrDefault(r => candidateNavigationContract.Equals(r.Instance.Name, StringComparison.Ordinal));
                }
                if (matchingRegistration == null) return new object[0];

                string typeCandidateName = matchingRegistration.ReturnedType.FullName;

                contractCandidates = base.GetCandidatesFromRegion(region, typeCandidateName);
            }

            return contractCandidates;
        }
    }
}