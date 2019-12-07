using DevExpress.XtraEditors.DXErrorProvider;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Jasmine.Core.Audit;
using Jasmine.Core.Contracts;
using Jasmine.Core.Tracking;
using Microsoft.AspNetCore.JsonPatch;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Model;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ValidationContext = FluentValidation.ValidationContext;

namespace Jasmine.Core.Mvvm
{
    [NotifyPropertyChanged]
    public abstract class EntityBase<T> : EntityBase,
        INotifyDataErrorInfo,ISupportPatchUpdate,
        IDXDataErrorInfo,
        ISupportFluentValidator<T> where T : EntityBase<T>
    {

        protected EntityBase()
        {
            ErrorsContainer = new JasmineErrorContainer<ValidationFailure>(RaiseErrorsChanged);
        }

        public JsonPatchDocument CreatePatchDocument()
        {
            var patchDoc = new JsonPatchDocument();
            var properties = this.ModifiedProperties.Distinct().ToList();

            foreach (var navProp in this.GetNavigationProperties())
            {
                foreach (EntityCollectionProperty<ITrackingCollection> colProp in navProp.AsCollectionProperty<ITrackingCollection>())
                {
                    if (colProp.EntityCollection.Count > 0 && !properties.Contains(colProp.Property.Name))
                    {
                        properties.Add(colProp.Property.Name);
                    }
                }
            }

            foreach (var property in properties)
            {
                if (TryGetPropValue(this, property, out var value))
                {
                    patchDoc.Replace(property, value);
                }
            }

            patchDoc.Replace(nameof(ModifiedProperties), properties);
            patchDoc.Replace(nameof(TrackingState), this.TrackingState);
            patchDoc.Replace(nameof(EntityIdentifier), this.EntityIdentifier);


            return patchDoc;


            bool TryGetPropValue(object src, string propName, out object value)
            {
                var propertyInfo = src.GetType().GetProperty(propName);
                if (propertyInfo.CanWrite)
                {
                    value = propertyInfo.GetValue(src, null);
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
        }


        //[IgnoreTracking]
        //public bool IsValid => !HasErrors && Id > 0 && ((IDirty)this).IsDirty == false;


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event EventHandler ChildCollectionChanged;

        /// <inheritdoc />
        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or <see langword="null" /> or <see cref="F:System.String.Empty" />, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable GetErrors(string propertyName) => ErrorsContainer.GetErrors(propertyName);

        [IgnoreTracking]
        public List<ValidationFailure> ValidationSummary => GetValidationErrorSummary();

        [Pure]
        private List<ValidationFailure> GetValidationErrorSummary()
        {
            //return ErrorsContainer.GetAllErrors()
            //    .Select(x => new ValidationFailure(StringHumanizeExtensions.Humanize(x.PropertyName, LetterCasing.Title), x.ErrorMessage))
            //    .ToList();

            return ErrorsContainer.GetErrors().SelectMany(x=> x.Value)
               .Select(x => new ValidationFailure(x.PropertyName, x.ErrorMessage))
               .ToList();
        }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        [IgnoreTracking]
        [SafeForDependencyAnalysis]
        public bool HasErrors => ErrorsContainer.HasErrors;

        /// <summary>
        /// Gets the errors container.
        /// </summary>
        /// <value>The errors container.</value>
        [IgnoreTracking]
         public ErrorsContainer<ValidationFailure> ErrorsContainer { get; }
        //public JasmineErrorContainer<ValidationFailure> ErrorsContainer { get; }

        /// <summary>
        /// Raises the errors changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaiseErrorsChanged(string propertyName)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        public void ClearErrors(string propertyName = null)
        {
            if (propertyName == null)
            {
                ErrorsContainer.ClearErrors();
            }
            else
            {
                ErrorsContainer.ClearErrors(propertyName);
            }
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName != "IsDirty")
            {
                ValidateSelf(propertyName);
            }
        }

        /// <summary>
        /// Gets the validator.
        /// </summary>
        /// <value>The validator.</value>
        [IgnoreTracking]
        private List<IValidator<T>> Validators { get; set; }

        public void SetValidators(List<IValidator<T>> validators)
        {
            Validators = validators;
        }


        private void ValidateUsingFluentValidator(string propertyName = null)
        {

            if (Validators?.Count > 0)
            {
                List<ValidationFailure> results = new List<ValidationFailure>();


                if (propertyName != null)
                {
                    ErrorsContainer.ClearErrors(propertyName);

                    string[] properties = new[] { propertyName };

                    var context = new ValidationContext(this, new PropertyChain(),
                        new MemberNameValidatorSelector(properties));

                    foreach (var validator in Validators)
                    {
                        var result = validator.Validate(context);
                        if (!result.IsValid)
                        {
                            results.AddRange(result.Errors);
                        }
                    }
                }
                else
                {
                    ErrorsContainer.ClearErrors();
                    foreach (var validator in Validators)
                    {
                        var result = validator.Validate(this);
                        if (!result.IsValid)
                        {
                            results.AddRange(result.Errors);
                        }
                    }
                }

                foreach (var failure in results.GroupBy(x => x.PropertyName)
                    .Select(x => new { PropertyName = x.Key, Errors = x.ToList() }))
                {
                    ErrorsContainer.SetErrors(failure.PropertyName, failure.Errors);
                }
            }
        }

        private void ValidateUsingDataAnnotations(string propertyName = null)
        {
            List<System.ComponentModel.DataAnnotations.ValidationResult> results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (propertyName != null)
            {
                var context = new System.ComponentModel.DataAnnotations.ValidationContext(this) { MemberName = propertyName };
                object value = this.GetType().GetProperty(propertyName)?.GetValue(this, null);

                Validator.TryValidateProperty(value, context, results);
            }
            else
            {
                var context = new System.ComponentModel.DataAnnotations.ValidationContext(this);

                Validator.TryValidateObject(this, context, results);
            }


            foreach (System.ComponentModel.DataAnnotations.ValidationResult failure in results)
            {
                string memberName = failure.MemberNames.First();
                ErrorsContainer.SetErrors(memberName, new[] { new ValidationFailure(memberName, failure.ErrorMessage) });
            }
        }

        public void ValidateSelf(string propertyName = null)
        {
            if (propertyName != null)
            {
                ErrorsContainer.ClearErrors(propertyName);
                ValidateUsingFluentValidator(propertyName);
                //   ValidateUsingDataAnnotations(propertyName);
            }
            else
            {
                ErrorsContainer.ClearErrors();
                ValidateUsingFluentValidator();
                //   ValidateUsingDataAnnotations();
            }
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(HasErrors));
        }

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorMessage">The error message.</param>
        public void SetError(string propertyName, string errorMessage) => SetError(propertyName, new[] { errorMessage });

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errors">The errors.</param>
        public void SetError(string propertyName, string[] errors)
        {
            List<ValidationFailure> existingErrors = ErrorsContainer
                .GetErrors(propertyName).ToList();
            List<string> newErros = new List<string>();

            foreach (string error in errors)
            {
                if (!existingErrors
                    .Any(x => x.PropertyName == propertyName && x.ErrorMessage == error))
                {
                    newErros.Add(error);
                }
            }

            existingErrors.AddRange(newErros.Select(error => new ValidationFailure(propertyName, error)));
            ErrorsContainer.SetErrors(propertyName, existingErrors);
            //  MakeDirty();
        }

        public void SetError<TEntity>(Expression<Func<TEntity, string>> propertyExpression, string error)
        {
            MemberExpression expression = (MemberExpression)propertyExpression.Body;
            string name = expression.Member.Name;

            SetError(name, error);
        }

        /// <summary>
        /// Sets the errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        public void SetErrors(IList<ValidationFailure> errors)
        {
            foreach (var failure in errors
                .GroupBy(x => x.PropertyName)
                .Select(x => new
                {
                    PropertyName = x.Key,
                    Errors = x.ToList()
                }))
            {
                ErrorsContainer.SetErrors(failure.PropertyName, failure.Errors);
            }
        }


        [IgnoreTracking]
        public byte[] RowVersion { get; set; }
        public virtual void OnChildCollectionChanged() => ChildCollectionChanged?.Invoke(this, EventArgs.Empty);

        public void GetPropertyError(string propertyName, ErrorInfo info)
        {
            IEnumerable<ValidationFailure> errors = ErrorsContainer.GetErrors(propertyName);
            ValidationFailure[] validationFailures = errors as ValidationFailure[] ?? errors.ToArray();
            if (validationFailures.Any())
            {
                info.ErrorText = string.Join(Environment.NewLine, validationFailures.Select(x => x.ErrorMessage).ToArray());
                info.ErrorType = ErrorType.Critical;
            }
        }

        public void GetError(ErrorInfo info)
        {

        }


    }
}