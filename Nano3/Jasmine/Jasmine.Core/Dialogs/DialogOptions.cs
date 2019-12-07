using System.Windows;

namespace Jasmine.Core.Dialogs
{
    public class DialogOptions
    {
        SizeToContent sizeToContent;
        public SizeToContent SizeToContent
        {
            get => WindowState != WindowState.Normal ? SizeToContent.Manual : sizeToContent;
            set => sizeToContent = value;
        }
        public ResizeMode ResizeMode { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public WindowState WindowState { get; set; }
        public WindowStartupLocation WindowStartupLocation { get; set; }
        public WindowStyle WindowStyle { get; set; } = WindowStyle.SingleBorderWindow;

        public DialogOptions(SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
            ResizeMode resizeMode = ResizeMode.CanResize, double width = 0, double height = 0,
            WindowState windowState = WindowState.Normal, WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterOwner)
        {
            SizeToContent = sizeToContent;
            ResizeMode = resizeMode;
            Width = width;
            Height = height;
            WindowState = windowState;
            WindowStartupLocation = windowStartupLocation;
        }
    }
}