using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace Jasmine.Core.Helpers
{
    public class ColorHelper : INotifyPropertyChanged
    {

        private Brush _foreground;
        private Brush _border;
        private Brush _background;
        private Brush _selectionBorder;
        private Brush _selectionForeground;
        private Brush _selectionContrastForeground;
        private Brush _selectionBackground;
        private Brush _hoverBorder;
        private Brush _hoverForeground;
        private Brush _hoverBackground;
        static ColorHelper _Instance;

        public ColorHelper()
        {
            ThemeManager.ThemeChanged += ColorHelper.ThemeChanged;
            GetColors(ThemeManager.ActualApplicationThemeName);
        }

        public static ColorHelper Instance { get { if (_Instance == null) _Instance = new ColorHelper(); return _Instance; } }
        public static void ThemeChanged(DependencyObject sender, ThemeChangedRoutedEventArgs e)
        {
            Instance.GetColors(e.ThemeName);
        }

        Brush Freeze(Brush brush)
        {
            brush.Freeze();
            return brush;
        }
        void GetColors(string name)
        {
            //ThemeManager.ThemeChanged -= ColorHelper.ThemeChanged;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                switch (name)
                {
                    case "VS2017Dark":
                        Background = Freeze((Brush)new BrushConverter().ConvertFrom("#FF2D2D30"));
                        Border = Freeze((Brush)new BrushConverter().ConvertFrom("#FF434346"));
                        Foreground = Freeze((Brush)new BrushConverter().ConvertFrom("#FFF1F1F1"));
                        HoverBackground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF3E3E40"));
                        HoverBorder = Freeze((Brush)new BrushConverter().ConvertFrom("#FF3E3E40"));
                        HoverForeground = Freeze((Brush)new BrushConverter().ConvertFrom("White"));
                        SelectionBackground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF007ACC"));
                        SelectionBorder = Freeze((Brush)new BrushConverter().ConvertFrom("#FF007ACC"));
                        SelectionForeground = Freeze((Brush)new BrushConverter().ConvertFrom("White"));
                        SelectionContrastForeground = Freeze((Brush)new BrushConverter().ConvertFrom("#80FF80"));
                        break;
                    case "VS2017Light":
                        Background = Freeze((Brush)new BrushConverter().ConvertFrom("#FFEEEEF2"));
                        Border = Freeze((Brush)new BrushConverter().ConvertFrom("#FFCCCEDB"));
                        Foreground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF1E1E1E"));
                        HoverBackground = Freeze((Brush)new BrushConverter().ConvertFrom("#FFC9DEF5"));
                        HoverBorder = Freeze((Brush)new BrushConverter().ConvertFrom("#FFC9DEF5"));
                        HoverForeground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF1E1E1E"));
                        SelectionBackground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF007ACC"));
                        SelectionBorder = Freeze((Brush)new BrushConverter().ConvertFrom("#FF007ACC"));
                        SelectionForeground = Freeze((Brush)new BrushConverter().ConvertFrom("White"));
                        SelectionContrastForeground = Freeze((Brush)new BrushConverter().ConvertFrom("#80FF80"));
                        break;
                    case "VS2017Blue":
                        Background = Freeze((Brush)new BrushConverter().ConvertFrom("#FFD6DBE9"));
                        Border = Freeze((Brush)new BrushConverter().ConvertFrom("#FF8E9BBC"));
                        Foreground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF1E1E1E"));
                        HoverBackground = Freeze((Brush)new BrushConverter().ConvertFrom("#FFFDF4BF"));
                        HoverBorder = Freeze((Brush)new BrushConverter().ConvertFrom("#FFE5C365"));
                        HoverForeground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF1E1E1E"));
                        SelectionBackground = Freeze((Brush)new BrushConverter().ConvertFrom("#FFFFF29D"));
                        SelectionBorder = Freeze((Brush)new BrushConverter().ConvertFrom("#FFE5C365"));
                        SelectionForeground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF1E1E1E"));
                        SelectionContrastForeground = Freeze((Brush)new BrushConverter().ConvertFrom("#008000"));
                        break;
                    case "Office2016BlackSE":
                        Background = Freeze((Brush)new BrushConverter().ConvertFrom("#FF262626"));
                        Border = Freeze((Brush)new BrushConverter().ConvertFrom("#FF686868"));
                        Foreground = Freeze((Brush)new BrushConverter().ConvertFrom("#FFDADADA"));
                        HoverBackground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF505050"));
                        HoverBorder = Freeze((Brush)new BrushConverter().ConvertFrom("#FF505050"));
                        HoverForeground = Freeze((Brush)new BrushConverter().ConvertFrom("White"));
                        SelectionBackground = Freeze((Brush)new BrushConverter().ConvertFrom("#FF6A6A6A"));
                        SelectionBorder = Freeze((Brush)new BrushConverter().ConvertFrom("#FF6A6A6A"));
                        SelectionForeground = Freeze((Brush)new BrushConverter().ConvertFrom("White"));
                        SelectionContrastForeground = Freeze((Brush)new BrushConverter().ConvertFrom("#80FF80"));
                        break;
                }
            });
            //ThemeManager.ThemeChanged += ColorHelper.ThemeChanged;
        }


        public Brush Background
        {
            get { return _background; }
            set
            {
                if (_background == value)
                    return;
                _background = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Background"));
            }
        }
        public Brush Foreground
        {
            get { return _foreground; }
            set
            {
                if (_foreground == value)
                    return;
                _foreground = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Foreground"));
            }
        }
        public Brush Border
        {
            get { return _border; }
            set
            {
                if (_border == value)
                    return;
                _border = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Border"));
            }
        }
        public Brush SelectionBackground
        {
            get { return _selectionBackground; }
            set
            {
                if (_selectionBackground == value)
                    return;
                _selectionBackground = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectionBackground"));
            }
        }
        public Brush SelectionBorder
        {
            get { return _selectionBorder; }
            set
            {
                if (_selectionBorder == value)
                    return;
                _selectionBorder = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectionBorder"));
            }
        }
        public Brush SelectionForeground
        {
            get { return _selectionForeground; }
            set
            {
                if (_selectionForeground == value)
                    return;
                _selectionForeground = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectionForeground"));
            }
        }
        public Brush SelectionContrastForeground
        {
            get { return _selectionContrastForeground; }
            set
            {
                if (_selectionContrastForeground == value)
                    return;
                _selectionContrastForeground = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectionContrastForeground"));
            }
        }
        public Brush HoverBackground
        {
            get { return _hoverBackground; }
            set
            {
                if (_hoverBackground == value)
                    return;
                _hoverBackground = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HoverBackground"));
            }
        }
        public Brush HoverBorder
        {
            get { return _hoverBorder; }
            set
            {
                if (_hoverBorder == value)
                    return;
                _hoverBorder = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HoverBorder"));
            }
        }
        public Brush HoverForeground
        {
            get { return _hoverForeground; }
            set
            {
                if (_hoverForeground == value)
                    return;
                _hoverForeground = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HoverForeground"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


    public static class EnumExentions
    {
        public static string GetDescription(this Enum GenericEnum) //Hint: Change the method signature and input paramter to use the type parameter T
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((DisplayAttribute)_Attribs.ElementAt(0)).Name;
                }
            }
            return GenericEnum.ToString();
        }

    }
}
