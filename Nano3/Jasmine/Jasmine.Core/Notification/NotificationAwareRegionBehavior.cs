using Prism.Regions;
using System;
using System.Collections.Specialized;
using System.Windows;

namespace Jasmine.Core.Notification
{
    public class NotificationAwareRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "NotificationAwareRegionBehavior";

        protected override void OnAttach()
        {
            if (Region.Name == KnownRegions.NotificationRegion)
            {
                Region.Views.CollectionChanged += Views_CollectionChanged;
            }
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is FrameworkElement view)
                    {
                        var viewModel = view.DataContext as INotificationAware;
                        if (viewModel == null)
                        {
                            throw new NullReferenceException("Notification's ViewModel must implement the INotificationAware interface");
                        }
                    }
                }
            }
        }
    }
}
