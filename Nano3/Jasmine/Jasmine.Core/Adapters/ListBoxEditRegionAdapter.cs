using DevExpress.Xpf.Editors;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Jasmine.Core.Adapters
{
    public class ListBoxEditRegionAdapter : RegionAdapterBase<ListBoxEdit>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ListBoxEditRegionAdapter`1"/>.
        /// </summary>
        /// <param name="regionBehaviorFactory">The factory used to create the region behaviors to attach to the created regions.</param>
        public ListBoxEditRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {

        }

        protected override void Adapt(IRegion region, ListBoxEdit regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement element in e.NewItems)
                    {
                        regionTarget.Items.Add(element);
                    }
                }

                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (FrameworkElement element in e.OldItems)
                    {
                        regionTarget.Items.Remove(element);
                    }
                }
            };
        }
        protected override IRegion CreateRegion() => new AllActiveRegion();
    }
}

