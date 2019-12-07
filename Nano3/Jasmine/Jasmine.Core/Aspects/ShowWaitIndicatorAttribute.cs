using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using DevExpress.Mvvm;
using Jasmine.Core.Dialogs;
using Jasmine.Core.Mvvm.LookupItems;
using Jasmine.Core.Repositories;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Jasmine.Core.Aspects
{
    /// <inheritdoc />
    [PSerializable]
    [System.AttributeUsageAttribute(System.AttributeTargets.All, AllowMultiple = false)]
    [SuppressMessage("ReSharper", "InheritdocConsiderUsage")]
    public sealed class ShowWaitIndicatorAttribute : OnMethodBoundaryAspect
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
                    service?.ServiceContainer?.GetService<ISplashScreenService>();
                splashScreenService?.ShowSplashScreen();
            }
            else
            {
                 DevExpress.Xpf.Core.DXSplashScreen.Show<JasmineWaitIndicator>();
            }
        }

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

            if (args.Instance is LookupItemViewModel viewModel && args.Exception is ApiException exception)
            {
                foreach (var error in exception.Errors)
                {
                    foreach (var err in error.Value)
                    {
                        viewModel.Entity.SetError(error.Key,err);
                        viewModel.SelectedEntity = null;

                        if (exception.StatusCode == HttpStatusCode.Conflict)
                        {
                            viewModel.ErrorMessage = err;
                        }
                    }
                }
            }

            //if (args.Instance is IMessageBoxService messageBoxService)
            //{
            //    messageBoxService.Show(string.Format(args.Exception.Message), "Error Occured", MessageBoxButton.OK, MessageBoxImage.Error,
            //        MessageBoxResult.OK);
            //}
            //else
            //{
            //    DXMessageBox.Show(string.Format(args.Exception.Message), "Error Occured", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            //}
        }

        ///// <inheritdoc />
        ///// <summary>
        /////   Method executed <b>after</b> the body of methods to which this aspect is applied,
        /////   even when the method exists with an exception (this method is invoked from
        /////   the <c>finally</c> block).
        ///// </summary>
        ///// <param name="args">Event arguments specifying which method
        ///// is being executed and which are its arguments.</param>
        public override void OnExit(MethodExecutionArgs args)
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
        }


    }


    /// <summary>
    /// Class BusinessException.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class BusinessException : Exception
    {
        public BusinessException(string message,Exception innerException) : base(message,innerException)
        {
        }
    }



}
