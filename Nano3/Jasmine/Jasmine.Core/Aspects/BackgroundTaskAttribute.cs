using DevExpress.Mvvm;
using Jasmine.Core.Dialogs;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Jasmine.Core.Aspects
{
    /// <inheritdoc />
    [PSerializable]
    [System.AttributeUsageAttribute(System.AttributeTargets.All, AllowMultiple = false)]
    [SuppressMessage("ReSharper", "InheritdocConsiderUsage")]
    public sealed class BackgroundTaskAttribute : OnMethodBoundaryAspect
    {

        /// <summary>
        /// Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed, which are its arguments, and how should the execution continue
        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (args.Instance is ISupportServices service)
            {
                ISplashScreenService splashScreenService =
                    service.ServiceContainer?.GetService<ISplashScreenService>();
                splashScreenService?.ShowSplashScreen();
            }
            else
            {
                DevExpress.Xpf.Core.DXSplashScreen.Show<JasmineWaitIndicator>();
            }
        }


        ///// <summary>
        ///// Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///// in case that the method resulted with an exception.
        ///// </summary>
        ///// <param name="args">Event arguments specifying which method
        ///// is being executed and which are its arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Instance is ISupportServices service)
            {
                if (service.ServiceContainer != null)
                {
                    var splashScreenService = service.ServiceContainer.GetService<ISplashScreenService>();
                    if (splashScreenService == null)
                    {
                        if (DevExpress.Xpf.Core.DXSplashScreen.IsActive)
                        {
                            DevExpress.Xpf.Core.DXSplashScreen.Close();
                        }
                    }
                    else
                    {
                        splashScreenService?.HideSplashScreen();
                    }


                    //var messageBoxService = service.ServiceContainer.GetService<IMessageBoxService>();
                    //if (messageBoxService == null)
                    //{
                    //    DXMessageBox.Show(args.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //else
                    //{
                    //    messageBoxService?.Show(args.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}


                }
            }
            else
            {
                if (DevExpress.Xpf.Core.DXSplashScreen.IsActive)
                {
                    DevExpress.Xpf.Core.DXSplashScreen.Close();
                }

                //  DXMessageBox.Show(args.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            args.FlowBehavior = FlowBehavior.Default;
        }

        /// <inheritdoc />
        /// <summary>
        ///   Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///   even when the method exists with an exception (this method is invoked from
        ///   the <c>finally</c> block).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            if (args.Instance is ISupportServices service)
            {
                service.ServiceContainer?.GetService<ISplashScreenService>()?.HideSplashScreen();
            }
            else if (DevExpress.Xpf.Core.DXSplashScreen.IsActive)
            {
                DevExpress.Xpf.Core.DXSplashScreen.Close();
            }
        }


    }
}