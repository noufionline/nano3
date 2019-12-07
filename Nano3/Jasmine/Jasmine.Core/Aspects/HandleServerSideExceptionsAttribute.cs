using System;
using DevExpress.Logify.WPF;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Jasmine.Core.Aspects
{
    /// <summary>
    /// Class HandleServerSideExceptionsAttribute. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="PostSharp.Aspects.OnExceptionAspect" />
    [PSerializable]
    public sealed class HandleServerSideExceptionsAttribute : OnExceptionAspect
    {



        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// in case that the method resulted with an exception (i.e., in a <c>catch</c> block).
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            Guid guid = Guid.NewGuid();

            LogifyAlert client = LogifyAlert.Instance;

            client.ApiKey = "3E99A8B031D049F1969320A512A747A9";
            if (!client.CustomData.ContainsKey("Exception ID"))
            {
                client.CustomData.Add("Exception ID", guid.ToString());                
            }
            if (!client.CustomData.ContainsKey("Machine Name"))
            {
                client.CustomData.Add("Machine Name", Environment.MachineName);
            }
            if (!client.CustomData.ContainsKey("User Name"))
            {
                client.CustomData.Add("User Name", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            }
            
            client.Send(args.Exception);
            string customErrorMessage =
                $"The service failed unexpectedly. Please report the incident to the Software team with the id #{guid}.";


            args.FlowBehavior = FlowBehavior.ThrowException;
            args.Exception = new BusinessException(customErrorMessage,args.Exception);
        }
    }
}