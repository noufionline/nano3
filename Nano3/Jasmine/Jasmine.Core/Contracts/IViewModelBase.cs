using System.Windows.Media;

namespace Jasmine.Core.Contracts
{
    public interface IViewModelBase
    {
        string Caption { get; set; }
        ImageSource CaptionImage { get; set; }
        bool ShowBusyIndicator { get; set; }
      
    }
}