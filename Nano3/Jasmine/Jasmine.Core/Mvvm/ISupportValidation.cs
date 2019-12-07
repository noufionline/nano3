using CommonServiceLocator;
using DevExpress.XtraEditors.DXErrorProvider;
using FluentValidation.Internal;
using FluentValidation.Results;
using Humanizer;
using Jasmine.Core.Audit;
using Jasmine.Core.Contracts;
using Jasmine.Core.Tracking;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ValidationContext = FluentValidation.ValidationContext;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Jasmine.Core.Mvvm
{
    public interface ISupportValidation
    {
        List<ValidationFailure> ValidationSummary { get; }

    }


    //// [Obsolete("Use FluentValidatorEntityBase<T> instead", true)]
   // [NotifyPropertyChanged]
   // public abstract class EntityBase<TValidator> : EntityBase, IValidatable,
   //     INotifyDataErrorInfo,
   //     IDXDataErrorInfo,
   //     ICustomValidator, ISupportValidation where TValidator : class, IValidator, new()
   // {

   //     private readonly LogSource _logger = LogSource.Get();


   //     protected EntityBase()
   //     {
   //         ErrorsContainer = new JasmineErrorContainer<ValidationFailure>(RaiseErrorsChanged);
   //         Validator = ServiceLocator.Current.GetInstance<TValidator>();
   //         ValidateSelf();
   //     }
   //     //[IgnoreTracking]
   //     //public bool IsValid => !HasErrors && Id > 0 && ((IDirty)this).IsDirty == false;


   //     public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
   //     public event EventHandler ChildCollectionChanged;

   //     /// <inheritdoc />
   //     /// <summary>
   //     /// Gets the validation errors for a specified property or for the entire entity.
   //     /// </summary>
   //     /// <param name="propertyName">The name of the property to retrieve validation errors for; or <see langword="null" /> or <see cref="F:System.String.Empty" />, to retrieve entity-level errors.</param>
   //     /// <returns>The validation errors for the property or entity.</returns>
   //     public IEnumerable GetErrors(string propertyName) => ErrorsContainer.GetErrors(propertyName);

   //     [IgnoreTracking]
   //     public List<ValidationFailure> ValidationSummary => GetValidationErrorSummary();

   //     [Pure]
   //     private List<ValidationFailure> GetValidationErrorSummary()
   //     {
   //         return ErrorsContainer.GetAllErrors()
   //             .Select(x => new ValidationFailure(x.PropertyName.Humanize(LetterCasing.Title), x.ErrorMessage))
   //             .ToList();
   //     }

   //     /// <summary>
   //     /// Gets a value that indicates whether the entity has validation errors.
   //     /// </summary>
   //     /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
   //     [IgnoreTracking]
   //     public bool HasErrors => ErrorsContainer.HasErrors;

   //     /// <summary>
   //     /// Gets the errors container.
   //     /// </summary>
   //     /// <value>The errors container.</value>
   //     [IgnoreTracking]
   //     public JasmineErrorContainer<ValidationFailure> ErrorsContainer { get; }

   //     /// <summary>
   //     /// Raises the errors changed.
   //     /// </summary>
   //     /// <param name="propertyName">Name of the property.</param>
   //     protected void RaiseErrorsChanged(string propertyName)
   //     {
   //         OnPropertyChanged(nameof(HasErrors));
   //         OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
   //     }

   //     public IValidator GetValidator() => Validator;


   //     protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
   //     {
   //         base.OnPropertyChanged(propertyName);

   //         if (propertyName != "IsDirty")
   //         {
   //             ValidateSelf(propertyName);
   //         }
   //     }

   //     /// <summary>
   //     /// Gets the validator.
   //     /// </summary>
   //     /// <value>The validator.</value>
   //     [IgnoreTracking]
   //     private IValidator Validator { get; }

   //     public void ValidateSelf(string propertyName = null)
   //     {
   //         List<ValidationFailure> results = new List<ValidationFailure>();

   //         ValidationResult result;

   //         if (propertyName != null)
   //         {
   //             ErrorsContainer.ClearErrors(propertyName);

   //             string[] properties = new[] { propertyName };

   //             var context = new ValidationContext(this, new PropertyChain(), new MemberNameValidatorSelector(properties));

   //             result = Validator.Validate(context);

   //         }
   //         else
   //         {
   //             ErrorsContainer.ClearErrors();
   //             result = Validator.Validate(this);
   //         }

   //         if (!result.IsValid)
   //         {
   //             results.AddRange(result.Errors);
   //         }
   //         foreach (var failure in results.GroupBy(x => x.PropertyName).Select(x => new { PropertyName = x.Key, Errors = x.ToList() }))
   //         {
   //             ErrorsContainer.SetErrors(failure.PropertyName, failure.Errors);
   //         }
   //     }

   //     protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
   //     {
   //         ErrorsChanged?.Invoke(this, e);
   //         OnPropertyChanged(nameof(HasErrors));
   //     }

   //     /// <summary>
   //     /// Sets the error.
   //     /// </summary>
   //     /// <param name="propertyName">Name of the property.</param>
   //     /// <param name="errorMessage">The error message.</param>
   //     public void SetError(string propertyName, string errorMessage) => SetError(propertyName, new[] { errorMessage });

   //     /// <summary>
   //     /// Sets the error.
   //     /// </summary>
   //     /// <param name="propertyName">Name of the property.</param>
   //     /// <param name="errors">The errors.</param>
   //     public void SetError(string propertyName, string[] errors)
   //     {
   //         List<ValidationFailure> existingErrors = ErrorsContainer
   //             .GetErrors(propertyName).ToList();
   //         List<string> newErros = new List<string>();

   //         foreach (string error in errors)
   //         {
   //             if (!existingErrors
   //                 .Any(x => x.PropertyName == propertyName && x.ErrorMessage == error))
   //             {
   //                 newErros.Add(error);
   //             }
   //         }

   //         existingErrors.AddRange(newErros.Select(error => new ValidationFailure(propertyName, error)));
   //         ErrorsContainer.SetErrors(propertyName, existingErrors);
   //         //  MakeDirty();
   //     }

   //     public void SetError<T>(Expression<Func<T, string>> propertyExpression, string error)
   //     {
   //         MemberExpression expression = (MemberExpression)propertyExpression.Body;
   //         string name = expression.Member.Name;

   //         SetError(name, error);
   //     }

   //     /// <summary>
   //     /// Sets the errors.
   //     /// </summary>
   //     /// <param name="errors">The errors.</param>
   //     public void SetErrors(IList<ValidationFailure> errors)
   //     {
   //         foreach (var failure in errors
   //             .GroupBy(x => x.PropertyName)
   //             .Select(x => new
   //             {
   //                 PropertyName = x.Key,
   //                 Errors = x.ToList()
   //             }))
   //         {
   //             ErrorsContainer.SetErrors(failure.PropertyName, failure.Errors);
   //         }
   //     }


   //     [IgnoreTracking]
   //     public byte[] RowVersion { get; set; }
   //     public virtual void OnChildCollectionChanged() => ChildCollectionChanged?.Invoke(this, EventArgs.Empty);

   //     public void GetPropertyError(string propertyName, ErrorInfo info)
   //     {
   //         IEnumerable<ValidationFailure> errors = ErrorsContainer.GetErrors(propertyName);
   //         ValidationFailure[] validationFailures = errors as ValidationFailure[] ?? errors.ToArray();
   //         if (validationFailures.Any())
   //         {
   //             info.ErrorText = string.Join(Environment.NewLine, validationFailures.Select(x => x.ErrorMessage).ToArray());
   //             info.ErrorType = ErrorType.Critical;
   //         }
   //     }

   //     public void GetError(ErrorInfo info)
   //     {

   //     }


   // }
}