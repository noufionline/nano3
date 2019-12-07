using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace Jasmine.Core.Mvvm
{
    public interface ISupportFluentValidator<T>:ISupportValidation
    {
        void SetValidators(List<IValidator<T>> validators);
        void ValidateSelf(string propertyName = null);
        void SetError(string propertyName, string[] errors);
        void SetErrors(IList<ValidationFailure> errors);
    }

    public interface ISupportPatchUpdate
    {
        JsonPatchDocument CreatePatchDocument();
    }
}