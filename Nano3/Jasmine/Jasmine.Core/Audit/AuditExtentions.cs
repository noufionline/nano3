using Newtonsoft.Json;

namespace Jasmine.Core.Audit
{
    public static class AuditExtentions
    {
        public static T Clone<T>(this T source)
        {

            if (ReferenceEquals(source, null))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}