using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace Jasmine.Core.Mvvm
{
    /// <inheritdoc />
    /// <summary>
    /// Class JasminErrorContainer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="!:Prism.Mvvm.ErrorsContainer{T}" />
    public class JasmineErrorContainer<T> : ErrorsContainer<T>
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Jasmine.Core.Mvvm.JasminErrorContainer`1" /> class.
        /// </summary>
        /// <param name="raiseErrorsChanged">The action that invoked if when errors are added for an object./&gt;
        /// event.</param>
        public JasmineErrorContainer(Action<string> raiseErrorsChanged) : base(raiseErrorsChanged)
        {
        }

        /// <summary>
        /// Gets all errors.
        /// </summary>
        /// <returns>List&lt;T&gt;.</returns>
        public List<T> GetAllErrors() => validationResults.SelectMany(x => x.Value)
            .ToList();


    }
}