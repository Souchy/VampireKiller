using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.json;

public static class Json
{

    public static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
        NullValueHandling = NullValueHandling.Ignore,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        //Converters = new List<JsonConverter> { new SmartListStatementConverter() }
    };

    public static string serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj, settings);
    }

    public static T deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, settings);
    }

}
