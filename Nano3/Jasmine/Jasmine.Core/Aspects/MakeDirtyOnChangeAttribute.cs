//using System;
//using System.Reflection;
//using PostSharp.Aspects;
//using PostSharp.Extensibility;
//using PostSharp.Patterns.Model;
//using PostSharp.Serialization;

//namespace Jasmine.Mvvm.Aspects
//{
//    [PSerializable]
//    [MulticastAttributeUsage(Inheritance = MulticastInheritance.Multicast)]
//    public sealed class MakeDirtyOnChangeAttribute : OnMethodBoundaryAspect
//    {
//        /// <summary>
//        /// The _method name
//        /// </summary>
//        private string _methodName;
//        /// <summary>
//        /// The _class name
//        /// </summary>
//        private Type _className;


//        /// <summary>
//        /// Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked
//        /// before any other build-time method.
//        /// </summary>
//        /// <param name="method">Method to which the current aspect is applied</param>
//        /// <param name="aspectInfo">Reserved for future usage.</param>
//        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
//        {
//            _methodName = method.Name;
//            _className = method.DeclaringType;
//        }


//        /// <summary>
//        /// Method invoked at build time to ensure that the aspect has been applied to the right target.
//        /// </summary>
//        /// <param name="method">Method to which the aspect has been applied</param>
//        /// <returns><c>true</c> if the aspect was applied to an acceptable field, otherwise
//        /// <c>false</c>.</returns>
//        public override bool CompileTimeValidate(MethodBase method) => null != _className.GetProperty("IsDirty", typeof(bool));


//        /// <summary>
//        /// Method executed <b>before</b> the body of methods to which this aspect is applied.
//        /// </summary>
//        /// <param name="args">Event arguments specifying which method
//        /// is being executed, which are its arguments, and how should the execution continue
//        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
//        public override void OnEntry(MethodExecutionArgs args)
//        {
//            if (args?.Method.MemberType == MemberTypes.Method &&
//                _methodName.StartsWith("set_") &&
//                !_methodName.Equals("set_IsDirty") &&
//                args.Arguments.Count > 0)
//            {
//                object currentValue = _className.InvokeMember(_methodName.Substring(4), BindingFlags.GetProperty, null, args.Instance, null);
//                args.MethodExecutionTag = currentValue;
//            }
//        }


//        /// <summary>
//        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
//        /// but only when the method successfully returns (i.e. when no exception flies out
//        /// the method.).
//        /// </summary>
//        /// <param name="args">Event arguments specifying which method
//        /// is being executed and which are its arguments.</param>
//        public override void OnSuccess(MethodExecutionArgs args)
//        {
//            if (args == null)
//            {
//                return;
//            }
//            if (_methodName.StartsWith("set_") && !_methodName.Equals("set_IsDirty") && args.Arguments.Count > 0)
//            {
//                if (args.MethodExecutionTag != args.Arguments[0])
//                {
//                    PropertyInfo dirty = _className.GetProperty("IsDirty", typeof(bool));
//                    dirty?.SetValue(args.Instance, true, null);

//                    IAggregatable aggregatable = args.Instance as IAggregatable;
//                    IDirty p = aggregatable?.Parent as IDirty;
//                    if (p != null)
//                        p.IsDirty = true;
//                }
//            }
//        }
//    }
//}
