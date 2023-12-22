using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Logia.vampirekiller.logia;
using Util.communication.commands;
using vampirekiller.logia.commands;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.umbreon.commands;

public class HandlerOnProjectileCollision : ICommandHandler<CommandProjectileCollision>
{
    public void handle(CommandProjectileCollision t)
    {
        // GD.Print("Handle on collision command: " + t.collider);
        Universe.fight.projectiles.remove(t.projectileInstance);
        
        if (t.collider != null)
        {
            // Temp damage handling
            t.collider.fightStats.addedLife.value -= t.projectileInstance.dmg;
            if (t.collider.getTotalStat<CreatureTotalLife>().value <= 0)
            {
                Universe.fight.creatures.remove(t.collider);
            }
        }
    }
}
