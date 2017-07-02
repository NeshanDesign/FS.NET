using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FsNet.Common.Helpers
{
    public class JsonHelper
    {
        public static T ReadObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { });
        }

        public static List<T> RaedObjects<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json, new JsonSerializerSettings { });
        }

        public static string SerializeObject<T>(T obj, bool indentedFormat = true)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                Formatting = indentedFormat ? Formatting.Indented : Formatting.None,
                ContractResolver = new IgnoredContractResolver()
            });
        }
    }

    public class IgnoredContractResolver : DefaultContractResolver
    {
        public IgnoredContractResolver() { }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            properties = properties.Where(p => p.HasMemberAttribute).ToList();
            return properties;
        }
    }

    public class IgnoredFromConfig : Attribute
    {
        private bool _ignore;
        public IgnoredFromConfig(bool ignore)
        {
            this._ignore = ignore;
        }
    }
}