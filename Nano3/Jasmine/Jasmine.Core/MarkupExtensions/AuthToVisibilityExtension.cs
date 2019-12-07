using System;
using System.Windows;
using System.Windows.Markup;
using CommonServiceLocator;
using Jasmine.Core.Contracts;
using static System.String;

namespace Jasmine.Core.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    public class AuthToVisibilityExtension : MarkupExtension
    {
        public string Operation { get; set; }

        public AuthToVisibilityExtension() => Operation = Empty;

        public AuthToVisibilityExtension(string operation) => Operation = operation;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsNullOrEmpty(Operation))
                return Visibility.Collapsed;

            IAuthorizationCache cache = ServiceLocator.Current.GetInstance<IAuthorizationCache>();
            try
            {
                if(cache.CheckAccess(Operation))
                    return Visibility.Visible;
            }
            catch (Exception)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }
    }
}