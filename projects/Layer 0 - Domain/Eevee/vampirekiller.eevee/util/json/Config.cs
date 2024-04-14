using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.eevee.util.json;

public class Config
{
    [JsonIgnore]
    private string path = "";

    public static T load<T>(string name = "") where T : Config, new()
    {
        if(name == "")
            name = typeof(T).Name + ".json";
        string path;
        #if DEBUG
            path = "configs/" + name;
        #else
            path = "res://configs/" + name;
        #endif
        if(!File.Exists(path))
        {
            var t = new T();
            t.path = path;
            t.save();
            return t;
        }
        string json = File.ReadAllText(path);
        T config = Json.deserialize<T>(json);
        config.path = path;
        return config;
    }

    public void save()
    {
        string json = Json.serialize(this);
        File.WriteAllText(path, json);
    }

}
