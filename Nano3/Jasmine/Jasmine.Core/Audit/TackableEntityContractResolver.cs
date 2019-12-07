using System.Reflection;
using Jasmine.Core.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jasmine.Core.Audit
{
    public class TackableEntityContractResolver:DefaultContractResolver
    {
        public ITrackable Trackable { get; }

        public TackableEntityContractResolver(ITrackable trackable)
        {
            Trackable = trackable;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property= base.CreateProperty(member, memberSerialization);
            if (property.PropertyName!= "Id" &&  !Trackable.ModifiedProperties.Contains(property.PropertyName))
            {
                property.Ignored = true;
            }

            return property;
        }
    }
}