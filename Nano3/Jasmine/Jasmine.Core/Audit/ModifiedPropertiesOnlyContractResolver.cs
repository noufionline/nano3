using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jasmine.Core.Audit
{

    public class ModifiedPropertiesOnlyContractResolver : DefaultContractResolver
    {
        private readonly string[] _modifiedProperties;

        public ModifiedPropertiesOnlyContractResolver(params string[] properties)
        {
            _modifiedProperties = properties;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (Attribute.IsDefined(member, typeof(IgnoreTrackingAttribute)))
            {
                property.Ignored = true;
                return property;
            }

            if (_modifiedProperties.Contains(property.PropertyName))
            {
                property.Ignored = true;
            }

            return property;
        }

        public override JsonContract ResolveContract(Type type)
        {
            var contract = base.ResolveContract(type);
            contract.IsReference = false;
            return contract;
        }
    }
}