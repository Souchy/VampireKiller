using Godot;
using Logia.vampirekiller.logia;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Util.json;
using vampierkiller.espeon;
using vampirekiller.glaceon.configs;
using vampirekiller.logia;
using vampirekiller.logia.configs;
using vampirekiller.umbreon;

namespace vampirekiller.glaceon.autoload;

/// <summary>
/// Auto-loaded by godot
/// </summary>
public partial class Main : Node
{
    public static ConfigDev configDev { get; set; }
    public static ConfigGeneral configGeneral { get; set; }
    public static ConfigUser configUser { get; set; }

    public override void _EnterTree()
    {
        // DOC: https://docs.godotengine.org/en/stable/tutorials/export/exporting_for_dedicated_servers.html#doc-exporting-for-dedicated-servers
        Universe.isServer = OS.GetCmdlineUserArgs().Contains("--server");
        // Sinon:
        // OS.HasFeature("dedicated_server")
        // DisplayServer.GetName() == "headless"

#if DEBUG
        Config.BaseDirectory = "configs/";
#else
        Config.BaseDirectory = "res://configs/";
#endif

        configDev = Config.load<ConfigDev>();
        configGeneral = Config.load<ConfigGeneral>();
        configUser = Config.load<ConfigUser>("profiles/" + configGeneral.lastProfileUsed + ".json");

        ResourceLoader.SetResPathReplacement("res://Assets", Paths.vampireAssetsSources);
        ResourceLoader.SetResPathReplacement("res://vampireassets", Paths.vampireAssets);
        GD.Print("Override paths: " + Paths.vampireAssetsSources);
        GD.Print("Override paths: " + Paths.vampireAssets);
        //ResourceLoader.SetResPathReplacement("res://addons", ConfigDev.Instance.vampireAssetsPath + "addons/"); // maybe

        Universe.root = GetTree().Root;
        LogiaDiamonds.loadTypes();
        //AssetCache.loadResources();

        if (Universe.isServer)
            Espeon.main();
        else
            Umbreon.main();
    }

}
