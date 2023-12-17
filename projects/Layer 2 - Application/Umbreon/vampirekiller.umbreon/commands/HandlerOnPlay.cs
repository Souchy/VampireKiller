using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.communication.events;
using vampierkiller.logia.commands;
using vampirekiller.logia.stub;
using VampireKiller.eevee.vampirekiller.eevee;

namespace vampirekiller.umbreon.commands;

internal class HandlerOnPlay : ICommandHandler<CommandPlayGame>
{
    public void handle(CommandPlayGame t)
    {
        Universe.fight = new StubFight();
    }
}
