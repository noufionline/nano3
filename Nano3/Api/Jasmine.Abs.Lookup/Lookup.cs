using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Lookup
{

    [Serializable]
    public class Lookup : IEquatable<Lookup>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Lookup);
        }

        public bool Equals(Lookup other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = -1919740922;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public static bool operator ==(Lookup left, Lookup right)
        {
            return EqualityComparer<Lookup>.Default.Equals(left, right);
        }

        public static bool operator !=(Lookup left, Lookup right)
        {
            return !(left == right);
        }
    }


}
