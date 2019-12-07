using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using CommonServiceLocator;
using Jasmine.Core.Contracts;
using Prism.Common;
using Prism.Regions;
using Unity;


namespace Jasmine.Core.Prism
{
    public class ScopedRegionManagerContentLoader : IRegionNavigationContentLoader
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationContentLoader"/> class with a service locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="container"></param>
        public ScopedRegionManagerContentLoader(IServiceLocator serviceLocator,IUnityContainer container)
        {
            _serviceLocator = serviceLocator;
            _container = container;
        }

        /// <summary>
        /// Gets the view to which the navigation request represented by <paramref name="navigationContext"/> applies.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="navigationContext">The context representing the navigation request.</param>
        /// <returns>
        /// The view to be the target of the navigation request.
        /// </returns>
        /// <remarks>
        /// If none of the views in the region can be the target of the navigation request, a new view
        /// is created and added to the region.
        /// </remarks>
        /// <exception cref="ArgumentException">when a new view cannot be created for the navigation request.</exception>
        public object LoadContent(IRegion region, NavigationContext navigationContext)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));
            if (navigationContext == null) throw new ArgumentNullException(nameof(navigationContext));

            string candidateTargetContract = GetContractFromNavigationContext(navigationContext);

            IEnumerable<object> candidates = GetCandidatesFromRegion(region, candidateTargetContract);

            IEnumerable<object> acceptingCandidates =
                candidates.Where(
                    v =>
                    {
                        INavigationAware navigationAware = v as INavigationAware;
                        if (navigationAware?.IsNavigationTarget(navigationContext) == false)
                        {
                            return false;
                        }

                        FrameworkElement frameworkElement = v as FrameworkElement;
                        if (frameworkElement == null)
                        {
                            return true;
                        }

                        navigationAware = frameworkElement.DataContext as INavigationAware;
                        return navigationAware == null || navigationAware.IsNavigationTarget(navigationContext);
                    });

            object view = acceptingCandidates.FirstOrDefault();

            if (view != null)
            {
                return view;
            }

            view = CreateNewRegionItem(candidateTargetContract);

            region.Add(view, null, CreateRegionManagerScope(view));

            return view;
        }

        private bool CreateRegionManagerScope(object view)
        {
            bool createRegionManagerScope = false;

            if (view is ICreateRegionManagerScope viewHasScopedRegions)
            {
                createRegionManagerScope = viewHasScopedRegions.CreateRegionManagerScope;
            }
            else
            {
                FrameworkElement frameworkElement = view as FrameworkElement;
                if (frameworkElement?.DataContext is ICreateRegionManagerScope viewModel)
                    createRegionManagerScope = viewModel.CreateRegionManagerScope;
            }

            return createRegionManagerScope;
        }

        /// <summary>
        /// Provides a new item for the region based on the supplied candidate target contract name.
        /// </summary>
        /// <param name="candidateTargetContract">The target contract to build.</param>
        /// <returns>An instance of an item to put into the <see cref="IRegion"/>.</returns>
        protected virtual object CreateNewRegionItem(string candidateTargetContract)
        {
            object newRegionItem;
            try
            {
                newRegionItem = _serviceLocator.GetInstance<object>(candidateTargetContract);
            }
            catch (ActivationException e)

            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Cannot create Navigation Target", candidateTargetContract),
                    e);
            }
            return newRegionItem;
        }

        /// <summary>
        /// Returns the candidate TargetContract based on the <see cref="NavigationContext"/>.
        /// </summary>
        /// <param name="navigationContext">The navigation contract.</param>
        /// <returns>The candidate contract to seek within the <see cref="IRegion"/> and to use, if not found, when resolving from the container.</returns>
        protected virtual string GetContractFromNavigationContext(NavigationContext navigationContext)
        {
            if (navigationContext == null) throw new ArgumentNullException(nameof(navigationContext));

            string candidateTargetContract = UriParsingHelper.GetAbsolutePath(navigationContext.Uri);
            candidateTargetContract = candidateTargetContract.TrimStart('/');
            return candidateTargetContract;
        }

        /// <summary>
        /// Returns the set of candidates that may satisfiy this navigation request.
        /// </summary>
        /// <param name="region">The region containing items that may satisfy the navigation request.</param>
        /// <param name="candidateNavigationContract">The candidate navigation target as determined by <see cref="GetContractFromNavigationContext"/></param>
        /// <returns>An enumerable of candidate objects from the <see cref="IRegion"/></returns>
        protected virtual IEnumerable<object> GetCandidatesFromRegion(IRegion region, string candidateNavigationContract)
        {
            if (candidateNavigationContract == null || candidateNavigationContract.Equals(string.Empty))
                throw new ArgumentNullException(nameof(candidateNavigationContract));

            IEnumerable<object> contractCandidates = GetCandidatesFromRegionCore(region, candidateNavigationContract).ToList();

            if (!contractCandidates.Any())
            {
                //First try friendly name registration. If not found, try type registration
                var matchingRegistration =_container.Registrations.FirstOrDefault(r => candidateNavigationContract.Equals(r.Name, StringComparison.Ordinal)) ??
                                          _container.Registrations.FirstOrDefault(r => candidateNavigationContract.Equals(r.RegisteredType.Name, StringComparison.Ordinal));
                if (matchingRegistration == null) return new object[0];

                string typeCandidateName = matchingRegistration.MappedToType.FullName;

                contractCandidates = GetCandidatesFromRegion(region, typeCandidateName);
            }

            return contractCandidates;
        }

        private static IEnumerable<object> GetCandidatesFromRegionCore(IRegion region, string candidateNavigationContract)
        {
            return region.Views.Where(v =>
                string.Equals(v.GetType().Name, candidateNavigationContract, StringComparison.Ordinal) ||
                string.Equals(v.GetType().FullName, candidateNavigationContract, StringComparison.Ordinal));
        }
    }

   



   
}