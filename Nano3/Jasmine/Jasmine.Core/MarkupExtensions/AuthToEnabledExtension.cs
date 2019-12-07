using CommonServiceLocator;
using Jasmine.Core.Contracts;
using System;
using System.Windows.Markup;

namespace Jasmine.Core.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(bool))]
    public class AuthToEnabledExtension : MarkupExtension
    {
        public string Operation { get; set; }

        public AuthToEnabledExtension() => Operation = String.Empty;

        public AuthToEnabledExtension(string operation) => Operation = operation;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Operation))
                return false;

            IAuthorizationCache cache = ServiceLocator.Current.GetInstance<IAuthorizationCache>();
            try
            {
                return cache.CheckAccess(Operation);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    [MarkupExtensionReturnType(typeof(bool))]
    public class AuthToReadOnlyExtension : MarkupExtension
    {
        public string Operation { get; set; }

        public AuthToReadOnlyExtension() => Operation = String.Empty;

        public AuthToReadOnlyExtension(string operation) => Operation = operation;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(Operation))
                return true;

            IAuthorizationCache cache = ServiceLocator.Current.GetInstance<IAuthorizationCache>();
            try
            {
                return !cache.CheckAccess(Operation);
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}