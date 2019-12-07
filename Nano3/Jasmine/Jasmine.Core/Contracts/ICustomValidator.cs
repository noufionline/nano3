using System.Collections.Generic;
using FluentValidation.Results;

namespace Jasmine.Core.Contracts
{
    public interface ICustomValidator
    {
        void SetError(string propertyName, string[] errors);
        void SetErrors(IList<ValidationFailure> errors);
    }
}
