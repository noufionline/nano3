using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Accordion;
using DevExpress.Xpf.Editors;

namespace Jasmine.Core.Converters
{
    public class SelectedItemChangedEventArgsConverter:EventArgsConverterBase<AccordionSelectedItemChangedEventArgs>
    {
        protected override object Convert(object sender, AccordionSelectedItemChangedEventArgs args)
        {
            if(args.NewItem is AccordionItem item)
                return item.Tag;
            return null;
        }
    }


    public class EditValueChangedEventArgsConverter : EventArgsConverterBase<EditValueChangedEventArgs>
    {
        protected override object Convert(object sender, EditValueChangedEventArgs args)
        {
            return args.NewValue;
        }
    }

}