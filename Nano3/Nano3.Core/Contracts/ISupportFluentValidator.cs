using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Nano3.Core.Contracts
{
    public interface ISupportFluentValidator<T> : ISupportValidation
    {
        IValidator<T> Validator{get;set;}
        void ValidateSelf(string propertyName = null);
        void SetError(string propertyName, string[] errors);
        void SetErrors(IList<ValidationFailure> errors);
    }
}