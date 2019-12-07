using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Jasmine.Core.Aspects
{
    /// <summary>
    /// Class ShowException. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="PostSharp.Aspects.OnExceptionAspect" />
    [PSerializable]
    public sealed class ShowExceptionAttribute : OnExceptionAspect
    {
        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// in case that the method resulted with an exception (i.e., in a <c>catch</c> block).
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Instance is ISupportServices service)
            {
                service.ServiceContainer?.GetService<ISplashScreenService>()?.HideSplashScreen();
            }
            else
            {
                if (DevExpress.Xpf.Core.DXSplashScreen.IsActive)
                {
                    DevExpress.Xpf.Core.DXSplashScreen.Close();
                }
            }
            
            if (args.Instance is IMessageBoxService messageBoxService)
            {
                messageBoxService.Show(args.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DXMessageBox.Show(args.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            args.FlowBehavior = FlowBehavior.Return;
        }
    }



}
