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
        if(path == null)
            return default;
        if (!resources.ContainsKey(path))
        {
            var scene = GD.Load<T>(path);
            resources[path] = scene;
        }
        return (T)resources[path];
    }
    /// <summary>
    /// Load a file with one of the extensions with order of priority.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">Path to file without extension like "res://assets/fireball"</param>
    /// <param name="extensions">List of acceptable extensions like [".tscn", ".glb"], in order of priority.</param>
    /// <returns></returns>
    public static T Load<T>(string path, params string[] extensions) where T : Resource
    {
        if (path == null)
            return default;
        if (resources.ContainsKey(path))
            return (T) resources[path];
        if(extensions.Length == 0) 
            return Load<T>(path);
        foreach (var extension in extensions)
        {
            if (!FileAccess.FileExists(path + extension)) continue;
            var scene = GD.Load<T>(path);
            resources[path] = scene;
            break;
        }
        return (T) resources[path];
    }
}

