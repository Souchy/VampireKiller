﻿using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.communication.events;
using vampierkiller.logia.commands;
using vampirekiller.logia;
using vampirekiller.logia.stub;
using VampireKiller.eevee.vampirekiller.eevee;

namespace vampirekiller.umbreon.commands;

internal class HandlerOnPlay : ICommandHandler<CommandPlayGame>
{
    public void handle(CommandPlayGame t)
    {
        if (Universe.isOnline)
            return;
        Universe.fight = new StubFight();
        EventBus.centralBus.publish(Events.EventChangeScene, Events.SceneFight);
    }
}
