using System;

namespace Jasmine.Core.Attributes
{
    public abstract class AttributeBase : Attribute
    {
        //public string Column { get; set; }
        public bool Summary { get; set; }
        public string Heading { get; set; }
    }
}