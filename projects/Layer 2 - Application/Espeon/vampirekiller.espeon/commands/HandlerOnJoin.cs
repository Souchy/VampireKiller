using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.communication.events;
using vampirekiller.logia.commands;
using vampirekiller.logia;
using Logia.vampirekiller.logia;
using vampirekiller.logia.stub;
using static Godot.RenderingServer;

namespace vampirekiller.espeon.commands;

public class HandlerOnJoin : ICommandHandler<CommandJoin>
{
    public void handle(CommandJoin t)
    {
        //var player = StubFight.spawnStubPlayer();
        //Universe.fight.creatures.add(player);
    }
}
