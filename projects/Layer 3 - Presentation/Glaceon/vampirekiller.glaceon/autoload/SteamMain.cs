using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.json;
using vampierkiller.espeon;
using vampirekiller.glaceon.configs;
using vampirekiller.logia.configs;
using vampirekiller.logia;
using vampirekiller.umbreon;
using Steamworks;

namespace vampirekiller.glaceon.autoload;

public partial class SteamMain : Node
{
    public override void _EnterTree()
    {
        base._EnterTree();
        try
        {
            uint appId = 1234;
            SteamClient.Init(appId);
        }
        catch (Exception)
        {
            // Couldn't init for some reason (steam is closed etc)
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        SteamClient.Shutdown();
    }

}
