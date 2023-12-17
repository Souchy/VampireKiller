using Godot;
using Logia.vampirekiller.logia;
using System;
using System.Linq;
using vampierkiller.espeon;
using vampirekiller.umbreon;

namespace vampirekiller.glaceon.autoload;

/// <summary>
/// Auto-loaded by godot
/// </summary>
public partial class Main : Node
{
    public override void _EnterTree()
    {
        // DOC: https://docs.godotengine.org/en/stable/tutorials/export/exporting_for_dedicated_servers.html#doc-exporting-for-dedicated-servers
        Universe.isServer = OS.GetCmdlineUserArgs().Contains("--server");
        // Sinon:
        // OS.HasFeature("dedicated_server")
        // DisplayServer.GetName() == "headless"
        if(Universe.isServer)
            Espeon.main();
        else
            Umbreon.main();
    }
}
