using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Humanizer;
using Jasmine.Core.Audit;
using Jasmine.Core.Contracts;
using PostSharp.Patterns.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Jasmine.Core.Tracking;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Jasmine.Core.Mvvm
{
  //  [Obsolete("Use FluentValidatorEntityBase instead",true)]
    [NotifyPropertyChanged]
    public abstract class EntityBaseCore : EntityBase, IValidatable,
        INotifyDataErrorInfo, ICustomValidator, ISupportValidation
    {
        
        protected EntityBaseCore()
        {
            ErrorsContainer = new JasmineErrorContainer<ValidationFailure>(RaiseErrorsChanged);
            ValidateSelf();
        }




        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc />
        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or <see langword="null" /> or <see cref="F:System.String.Empty" />, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable GetErrors(string propertyName) => ErrorsContainer.GetErrors(propertyName);

        [IgnoreTracking] 
        public List<ValidationFailure> ValidationSummary => GetValidationSummary();

        [Pure]
        private List<ValidationFailure> GetValidationSummary()
        {
            return ErrorsContainer.GetAllErrors()
                .Select(x => new ValidationFailure(x.PropertyName.Humanize(LetterCasing.Title), x.ErrorMessage))
                .ToList();
        }


        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        [IgnoreTracking]
        public bool HasErrors => this.ErrorsContainer.HasErrors;

        /// <summary>
        /// Gets the errors container.
        /// </summary>
        /// <value>The errors container.</value>
        [IgnoreTracking]
        public JasmineErrorContainer<ValidationFailure> ErrorsContainer { get; }

        /// <summary>
        /// Raises the errors changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaiseErrorsChanged(string propertyName)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;



        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName != nameof(IsDirty))
            {
                ValidateSelf(propertyName);
            }
        }


        [IgnoreTracking]
        protected virtual IValidator FluentValidator => null;


        public IValidator GetValidator() => FluentValidator;

        /// <summary>
        /// Gets the validator.
        /// </summary>
        /// <value>The validator.</value>
        public void ValidateSelf(string propertyName = null)
        {

            if (propertyName != null)
            {
                ErrorsContainer.ClearErrors(propertyName);

                ValidateUsingDataAnnotations(propertyName);
                ValidateUsingFluentValidator(propertyName);
            }
            else
            {
                ErrorsContainer.ClearErrors();

                ValidateUsingDataAnnotations();
                ValidateUsingFluentValidator();
            }
        }

        private void ValidateUsingFluentValidator(string propertyName = null)
        {

            if (FluentValidator != null)
            {
                List<ValidationFailure> results = new List<ValidationFailure>();
                ValidationResult result;
                if (propertyName != null)
                {
                    string[] properties = new[] { propertyName };
                    FluentValidation.ValidationContext context = new FluentValidation.ValidationContext(this, new PropertyChain(),
                        new MemberNameValidatorSelector(properties));

                    result = FluentValidator.Validate(context);
                }
                else
                {
                    result = FluentValidator.Validate(this);
                }


                if (!result.IsValid)
                {
                    results.AddRange(result.Errors);
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
                ValidationContext context = new ValidationContext(this) { MemberName = propertyName };
                object value = this.GetType().GetProperty(propertyName)?.GetValue(this, null);

                Validator.TryValidateProperty(value, context, results);
            }
            else
            {
                ValidationContext context = new ValidationContext(this);

                Validator.TryValidateObject(this, context, results);
            }


            foreach (System.ComponentModel.DataAnnotations.ValidationResult failure in results)
            {
                string memberName = failure.MemberNames.First();
                ErrorsContainer.SetErrors(memberName, new[] { new ValidationFailure(memberName, failure.ErrorMessage) });
            }
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

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
            List<ValidationFailure> existingErrors = ErrorsContainer.GetErrors(propertyName).ToList();
            List<string> newErros = new List<string>();

            foreach (string error in errors)
            {
                if (!existingErrors.Any(x => x.PropertyName == propertyName && x.ErrorMessage == error))
                {
                    newErros.Add(error);
                }
            }

            existingErrors.AddRange(newErros.Select(error => new ValidationFailure(propertyName, error)));
            ErrorsContainer.SetErrors(propertyName, existingErrors);
          //  ((IDirty)this).IsDirty = true;
        }

        public void SetError<T>(Expression<Func<T, string>> propertyExpression, string error)
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
            foreach (var failure in errors.GroupBy(x => x.PropertyName)
                .Select(x => new { PropertyName = x.Key, Errors = x.ToList() }))
            {
                ErrorsContainer.SetErrors(failure.PropertyName, failure.Errors);
            }
        }

       
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
      

        [IgnoreTracking]
        public byte[] RowVersion { get; set; }

    }
}