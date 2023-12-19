using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logia.vampirekiller.logia;
using Util.communication.commands;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.triggers.schemas;
using vampirekiller.logia.extensions;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.espeon.commands;

public record struct CommandProcessTick(double delta) : ICommand
{
    public byte[] serialize()
    {
        throw new NotImplementedException();
    }
}

public class ProcessHandler : ICommandHandler<CommandProcessTick>
{
    public void handle(CommandProcessTick t)
    {
        if(Universe.fight == null)
            return;
        var action = new ActionProcessTick() 
        {
            delta = t.delta
        };
        var trigger = new TriggerEventProcessTick(t.delta);

        Universe.fight.procTriggers(action, trigger);

        // var creatures = Universe.fight.creatures.values.ToList();
        // foreach (var crea in creatures)
        // {
        //     crea.applyRegen();
        //     crea.procTriggers(action, trigg);
        //     // crea.getTotalStat<CreatureLifeDegen>(); // ??
        // }
    }


}
