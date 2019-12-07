using FluentValidation;
using Jasmine.Core.Mvvm;
using System;
using System.Collections.Generic;

namespace Jasmine.Core
{


    public interface ILookupItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; set; }

        string Name { get; set; }
        
      
    }

    public interface ILookupItemModel : ILookupItem
    {
        byte[] RowVersion { get; set; }
    }

    public class LookupItem : ILookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
     
    }

    public class ApplicationSetting
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }
    public class LookupItemModel:EntityBase<LookupItemModel>, ILookupItemModel, IEquatable<LookupItemModel>
    {
        public string Name { get; set; }
    

        public override bool Equals(object obj)
        {
            return Equals(obj as LookupItemModel);
        }

        public bool Equals(LookupItemModel other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   EqualityComparer<byte[]>.Default.Equals(RowVersion, other.RowVersion);
        }

        public override int GetHashCode()
        {
            int hashCode = 295984171;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(RowVersion);
            return hashCode;
        }

        public static bool operator ==(LookupItemModel item1, LookupItemModel item2)
        {
            return EqualityComparer<LookupItemModel>.Default.Equals(item1, item2);
        }

        public static bool operator !=(LookupItemModel item1, LookupItemModel item2)
        {
            return !(item1 == item2);
        }

        public override string ToString() => Name;

        
    }

    

    public class LookupItem<T> where T : class
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }

    public partial class LookupItemValidator : AbstractValidator<LookupItemModel>
    {
        public LookupItemValidator() => RuleFor(x => x.Name).NotEmpty().WithMessage("Enter Name");
    }

    public class AbsApplicationInfo : LookupItem
    {
        public string ApplicationType { get; set; }
    }
}