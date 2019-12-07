using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;

namespace Jasmine.Core.Common
{
    public class PostDelayHelper
    {
        public static DependencyProperty DelayTimeProperty = DependencyProperty.RegisterAttached("DelayTime",
            typeof(int), typeof(PostDelayHelper),
            new PropertyMetadata(0, new PropertyChangedCallback(DelayTimeChanged)));

        public static void SetDelayTime(DependencyObject d, int value)
        {
            d.SetValue(DelayTimeProperty, value);
        }

        public static int GetDelayTime(DependencyObject d)
        {
            return (int)d.GetValue(DelayTimeProperty);
        }

        public static void DelayTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue >= 0 && d is GridColumn gridColumn)
            {
                if (gridColumn.View is GridViewBase gridViewBase)
                {
                    gridViewBase.ShownEditor += delegate(object sender, EditorEventArgs args)
                    {
                        if (args.RowHandle == DataControlBase.AutoFilterRowHandle &&
                            Equals(args.Column, d as GridColumn))
                        {
                            ((BaseEdit) args.Editor).EditValuePostDelay = GetDelayTime(d);
                            ((BaseEdit) args.Editor).EditValuePostMode = PostMode.Delayed;
                        }
                    };
                }
            }
        }
    }
}