using FluentValidation.Results;
using System.Collections.Generic;

namespace Nano3.Core.Contracts
{
    public interface ISupportValidation
    {
        List<ValidationFailure> ValidationSummary { get; }

    }
}