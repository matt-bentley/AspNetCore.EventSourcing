using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace AspNetCore.EventSourcing.Infrastructure.Serialization
{
    public sealed class PrivatePropertyResolver : DefaultContractResolver
    {
        public readonly static PrivatePropertyResolver Instance = new PrivatePropertyResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                var hasPrivateSetter = property?.GetSetMethod(true) != null;
                prop.Writable = hasPrivateSetter;
            }
            return prop;
        }
    }
}
