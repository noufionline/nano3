using System.Windows.Media;

namespace Jasmine.Core.Mvvm
{
    public abstract class CollectionViewModelBase
    {
        public virtual string Caption { get; set; }

        public virtual ImageSource CaptionImage { get; set; }

        public  virtual bool ShowBusyIndicator { get; set; }
    }
}