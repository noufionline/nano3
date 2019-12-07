using FluentValidation;
using Jasmine.Core.Mvvm;

namespace Jasmine.Core.Security
{


    public class UserCredential : EntityBaseCore
    {
        private string _password;
        public string UserName { get; set; }
        public int EmployeeId { get; set; }

        public string Password
        {
            get => _password;
            set
            {
                _password = value; 
                OnPropertyChanged(nameof(ValidationSummary));
            }
        }

        public int DivisionId { get; set; }

        protected override IValidator FluentValidator => new UserCredentialValidator();
    }



}