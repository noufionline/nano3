using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.RichEdit;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Windows;
using System.Windows.Controls;
using Microsoft.OData;
using RestSharp;

namespace Jasmine.Core.Common
{
    public static class Helper
    {
        public static bool GetIsReadOnly(DependencyObject obj) => (bool)obj.GetValue(IsReadOnlyProperty);

        public static void SetIsReadOnly(DependencyObject obj, bool value) => obj.SetValue(IsReadOnlyProperty, value);

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(Helper), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.Inherits, ReadOnlyPropertyChanged));
        public static int GetDivisionId(this ClaimsPrincipal principal)
        {
            return Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("DivisionId").Value);
        }
        private static void ReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            switch (d)
            {
                case ButtonEdit buttonEdit:
                    buttonEdit.AllowDefaultButton = !(bool)e.NewValue;
                    buttonEdit.IsReadOnly = (bool)e.NewValue;
                    break;
                case IBaseEdit baseEdit:
                    baseEdit.IsReadOnly = (bool)e.NewValue;
                    break;
                case Button button:
                    button.IsEnabled = !(bool)e.NewValue;
                    break;
                case ToolBarControl toolBarControl:
                    toolBarControl.IsEnabled = !(bool)e.NewValue;
                    break;
                case GridControl gridControl:
                    if (gridControl.View != null)
                        gridControl.View.AllowEditing = false;
                    break;
                case RichEditControl richEditControl:
                    richEditControl.ReadOnly = (bool)e.NewValue;
                    break;
            }
        }


        public static ObservableCollection<T> ToObservableList<T>(this IEnumerable<T> items)
        {
            return items==null ? new ObservableCollection<T>() : new ObservableCollection<T>(items);
        }
    }

    public static class Extentions
    {
        public static SubscriptionToken Subscribe<TPayload>(this PubSubEvent<TPayload> pubSubEvent, Action<TPayload> action, Predicate<TPayload> filter)
        {
            return pubSubEvent.Subscribe(action, ThreadOption.PublisherThread, false, filter);
        }

        public static void SetBearerToken(this RestRequest request, string token)
        {
            request.AddHeader("authorization", $"Bearer {token}");
        }

        public static void SetBearerToken(this IODataRequestMessage request, string token)
        {
            request.SetHeader("authorization", $"Bearer {token}");
        }
    }


    public class SecurableAttribute : Attribute
    {
        public string Name { get; }

        public SecurableAttribute(string name) => Name = name;
    }

    public class NavigationObject
    {
        public string Caption { get; set; }

        public string ViewName { get; set; }
        public NavigationParameters Parameters { get; set; }

    }
}