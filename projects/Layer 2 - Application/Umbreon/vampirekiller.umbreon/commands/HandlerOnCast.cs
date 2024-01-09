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
using Util.communication.events;
using VampireKiller.eevee.vampirekiller.eevee.spells;

namespace vampirekiller.umbreon.commands;

public class HandlerOnCast : ICommandHandler<CommandCast>
{
    public const string EventAnimationCast = "animation.cast";
    public void handle(CommandCast command)
    {
        var action = new ActionCastActive() {
            sourceEntity = command.sourceCreature,
            raycastEntity = command.raycastEntity,
            raycastPosition = command.raycastMouse,
            skillInstanceId = command.skillInstanceId,
            fight = Universe.fight,
        };

        // canApplyCast assures that skill & caster are valid
        if (action.canApplyCast())
        {
            var skill = action.getActive()!;
            var caster = Universe.fight.creatures.get(c => c.entityUid == command.sourceCreature)!;
            var castTime = caster.getTotalStat<SpellTotalCastTime>(skill.stats);

            EventBus.centralBus.publish(EventAnimationCast, action, castTime.value);
        }
    }
}
