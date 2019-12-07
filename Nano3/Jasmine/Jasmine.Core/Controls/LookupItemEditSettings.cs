using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Grid.LookUp;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Jasmine.Core.Controls
{
    public class LookupItemEditSettings : LookUpEditSettings
    {
        public LookupItemEditSettings() => RegisterCustomEdit();

        public static void RegisterCustomEdit() => EditorSettingsProvider.Default.RegisterUserEditor(typeof(LookupItemEditSett),
                typeof(LookupItemEditSettings),
                () => new LookupItemEditSett(),
                () => new LookupItemEditSettings());
        protected override void AssignToEditCore(IBaseEdit edit)
        {
            LookupItemEditSett editor = edit as LookupItemEditSett;
            if (editor == null)
                return;
            base.AssignToEditCore(edit);
            editor.CanManage = CanManage;
            editor.Route = Route;
        }


        public string Route
        {
            get => (string)GetValue(RouteProperty);
            set => SetValue(RouteProperty, value);
        }

        public static readonly DependencyProperty RouteProperty =
            DependencyProperty.Register("Route1", typeof(string), typeof(LookupItemEdit), new PropertyMetadata(string.Empty));

        public bool CanManage
        {
            get => (bool)GetValue(CanManageProperty);
            set => SetValue(CanManageProperty, value);
        }

        public static readonly DependencyProperty CanManageProperty =
            DependencyProperty.Register("CanManage1", typeof(bool), typeof(LookupItemEdit), new PropertyMetadata(false));
    }

    public class LookupItemEditSett : LookUpEdit
    {
        public LookupItemEditSett()
        {
            DisplayMember = "Name";
            ValueMember = "Id";
            AutoPopulateColumns = false;
            AutoComplete = true;
            IncrementalFiltering = true;
            ImmediatePopup = true;
            IsPopupAutoWidth = true;
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
            AllowNullInput = true;
            FilterCondition = DevExpress.Data.Filtering.FilterCondition.StartsWith;
            FindButtonPlacement = EditorPlacement.Popup;
            FindMode = DevExpress.Xpf.Editors.FindMode.Always;
            ShowSizeGrip = true;
            ValidateOnTextInput = true;
        }



        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            PopupContentTemplate = BuildPopupContentTemplate();
            PopupBottomAreaTemplate = BuildPopupBottomAreaTemplate();
        }

        ControlTemplate BuildPopupContentTemplate()
        {
            const string xamlCode = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
              "xmlns:dxg=\"http://schemas.devexpress.com/winfx/2008/xaml/grid\"> " +
              "<dxg:GridControl Name=\"PART_GridControl\">" +
                 "<dxg:GridControl.Columns>" +
                      "<dxg:GridColumn FieldName=\"Name\"/>" +
                   "</dxg:GridControl.Columns>" +
                    "<dxg:GridControl.View>" +
                        " <dxg:TableView AutoWidth=\"True\"/>" +
                      "</dxg:GridControl.View>" +
                   "</dxg:GridControl>" +
                "</ControlTemplate> ";
            ControlTemplate template = (ControlTemplate)System.Windows.Markup.XamlReader.Parse(xamlCode);

            return template;
        }
        ControlTemplate BuildPopupBottomAreaTemplate()
        {
            const string xamlCode = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
                "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" " +
                "xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\" " +
                "xmlns:dx=\"http://schemas.devexpress.com/winfx/2008/xaml/core\">" +
                     "<StackPanel Orientation=\"Horizontal\" Margin=\"5,5,5,5\" HorizontalAlignment=\"Right\">" +
                         "<StackPanel.Resources>" +
                             "<dx:BooleanToVisibilityConverter x:Key=\"BooleanToVisibilityConverter\"/>" +
                          "</StackPanel.Resources>" +
                          "<dx:SimpleButton Glyph=\"{dx:DXImage Image=Add_16x16.png}\" Visibility=\"{Binding (dxe:BaseEdit.OwnerEdit).CanManage, RelativeSource=" +
                      "{RelativeSource TemplatedParent}, Converter={StaticResource BooleanToVisibilityConverter}}\" " +
                                 "Padding=\"5,5,5,5\" Content=\"Manage\" " +
                                 "Command=\"{Binding Path=DataContext.LookupItemCreationCommand, RelativeSource={RelativeSource TemplatedParent}}\" " +
                                "CommandParameter=\"{Binding (dxe:BaseEdit.OwnerEdit).Route, RelativeSource={RelativeSource TemplatedParent}}\"/> " +
                "<dx:SimpleButton Glyph=\"{dx:DXImage Image=Refresh_16x16.png}\" " +
                    "Content=\"Refresh\" Margin=\"5,0,0,0\" Padding=\"5,5,5,5\" " +
                                 "Command=\"{Binding Path=DataContext.LookupItemRefreshCommand, RelativeSource={RelativeSource TemplatedParent}}\" " +
                                "CommandParameter=\"{Binding (dxe:BaseEdit.OwnerEdit).Route, RelativeSource={RelativeSource TemplatedParent}}\"/> " +
                    "</StackPanel>" +
                 "</ControlTemplate> ";
            ControlTemplate template = (ControlTemplate)System.Windows.Markup.XamlReader.Parse(xamlCode);

            return template;
        }

        public string Route
        {
            get => (string)GetValue(RouteProperty);
            set => SetValue(RouteProperty, value);
        }

        public static readonly DependencyProperty RouteProperty =
            DependencyProperty.Register("Route2", typeof(string), typeof(LookupItemEdit), new PropertyMetadata(string.Empty));

        public bool CanManage
        {
            get => (bool)GetValue(CanManageProperty);
            set => SetValue(CanManageProperty, value);
        }

        public static readonly DependencyProperty CanManageProperty =
            DependencyProperty.Register("CanManage2", typeof(bool), typeof(LookupItemEdit), new PropertyMetadata(false));

    }
}
