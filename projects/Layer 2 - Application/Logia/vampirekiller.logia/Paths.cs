using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.logia.configs;

namespace vampirekiller.logia;

public static class Paths
{

#if DEBUG
    public static string vampireAssetsSources { get; } = ConfigDev.Instance.vampireAssetsPath + "Assets/";
    public static string vampireAssets { get; } = ConfigDev.Instance.vampireAssetsPath + "vampireassets/";
#else
    public static string vampireAssetsSources { get; } = "res://Assets/";
    public static string vampireAssets { get; } = "res://vampireassets/";
#endif

    public static string animations { get; } = vampireAssets + "animations/";
    public static string creatures { get; } = vampireAssets + "creatures/";
    public static string spells { get; } = vampireAssets + "spells/";
    public static string maps { get; } = vampireAssets + "maps/";
    //public static string tilesets { get; } = vampireAssets + "tilesets/"; // dont really need it if we just load maps?
    public static string materials { get; } = vampireAssets + "materials/";
    public static string textures { get; } = vampireAssets + "textures/";
    /// <summary>
    /// Examples: Lapis background, events (rest scene, merchant npc scene...)
    /// </summary>
    public static string scenes { get; } = vampireAssets + "scenes/";

    public static string entities { get; } = "res://scenes/sapphire/entities/";
}
