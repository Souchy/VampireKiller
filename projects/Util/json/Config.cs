using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.json;

public class Config
{
    [JsonIgnore]
    private string savePath = "";

    public static string BaseDirectory { get; set; }

    public static T load<T>(string name = "") where T : Config, new()
    {
        if (name == "")
            name = typeof(T).Name;
        if (!name.Contains('.'))
            name += ".json";

        string path = name;
        if(!string.IsNullOrEmpty(BaseDirectory))
            path = string.Join('/', BaseDirectory, name);

        if (!File.Exists(path))
        {
            var t = new T();
            t.savePath = path;
            t.save();
            return t;
        }
        string json = File.ReadAllText(path);
        T config = Json.deserialize<T>(json);
        config.savePath = path;
        return config;
    }

    public void save()
    {
        string json = Json.serialize(this);
        File.WriteAllText(savePath, json);
    }

}
