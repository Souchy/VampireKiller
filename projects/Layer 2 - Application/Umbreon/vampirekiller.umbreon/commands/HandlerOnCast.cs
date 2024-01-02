using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Logia.vampirekiller.logia;
using vampirekiller.logia.commands;
using VampireKiller.eevee;
using Util.entity;
using vampirekiller.eevee.actions;
using vampirekiller.logia.extensions;

namespace vampirekiller.umbreon.commands;

public class HandlerOnCast : ICommandHandler<CommandCast>
{
    public void handle(CommandCast command)
    {
        var playerCreature = Universe.fight.creatures.get(c => c.playerId == command.playerId);
        var action = new ActionCastActive() {
            sourceEntity = playerCreature.entityUid, //command.source.entityUid,
            raycastEntity = command.raycastEntity,
            raycastPosition = command.raycastMouse,
            fight = Universe.fight,
            slot = command.activeSlot,
        };
        if (action.canApplyCast())
            action.applyActionCast();
    }
}
