using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.glaceon.util;

/// <summary>
/// TODO
/// malheureusement on peut pas faire d'extension sur GD comme GD.LoadCache :(
/// </summary>
public class AssetCache
{
    public static Dictionary<string, Resource> resources = new();
    public static T Load<T>(string path) where T : Resource
    {
        if (!resources.ContainsKey(path))
        {
            var scene = GD.Load<T>(path);
            resources[path] = scene;
        }
        return (T)resources[path];
    }
}

