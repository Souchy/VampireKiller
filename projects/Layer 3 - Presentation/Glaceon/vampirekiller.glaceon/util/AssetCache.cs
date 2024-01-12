using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.logia;

namespace vampirekiller.glaceon.util;

/// <summary>
/// TODO
/// malheureusement on peut pas faire d'extension sur GD comme GD.LoadCache :(
/// </summary>
public static class AssetCache
{
    public static Dictionary<string, Resource> resources = new();

    public static Dictionary<string, List<string>> filesByExtension = new();

    public static List<string> models = new();
    public static List<string> skills = new();
    public static List<string> maps = new();
    public static List<string> animations = new();

    public static void loadResources() {
        recurse(Paths.creatures, models);
        recurse(Paths.spells, skills);
        recurse(Paths.maps, maps);
        recurse(Paths.animations, animations);
    }

    private static void recurse(string dirPath, List<string> list)
    {
        var dir = DirAccess.Open(dirPath);
        if(dir == null) {
            GD.PrintErr("AssetCache could not find directory: " + dirPath);
            return;
        }
        foreach(var file in dir.GetFiles())
        {
            var filePath = dirPath + file;
            var ext = file.Split(".").Last();
            if(ext.Contains("import"))
                continue;
            if(!filesByExtension.ContainsKey(ext))
                filesByExtension.Add(ext, new());
            filesByExtension[ext].Add(filePath);

            // list for spawners
            list.Add(filePath);

            // Preload?
            // Load<Resource>(filePath);
        }
        foreach(var sub in dir.GetDirectories())
        {
            recurse(dirPath + sub + "/", list);
        }
    }

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
            throw new Exception($"Couldn't find resource with path [{path}] and extensions [{string.Join(", ", extensions)}]");
        if (extensions.Length == 0) 
            return Load<T>(path);
        foreach (var extension in extensions)
        {
            if(path.Contains(extension)) 
                return Load<T>(path);
        }
        foreach (var extension in extensions)
        {
            var filepath = path + extension;
            // GD.Print("Look for resource: " + filepath);
            if (resources.ContainsKey(filepath))
                return (T) resources[filepath];
            if (!FileAccess.FileExists(filepath)) 
                continue;
            var scene = GD.Load<T>(filepath);
            resources[filepath] = scene;
            return (T) resources[filepath];
        }
        throw new Exception($"Couldn't find resource with path [{path}] and extensions [{string.Join(", ", extensions)}]");
        //return (T) resources[path];
    }
}

