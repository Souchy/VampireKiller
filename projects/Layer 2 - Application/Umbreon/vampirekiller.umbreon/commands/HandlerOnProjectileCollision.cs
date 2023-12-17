using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logia.vampirekiller.logia;
using Util.communication.commands;
using vampirekiller.logia.commands;

namespace vampirekiller.umbreon.commands;

public class HandlerOnProjectileCollision : ICommandHandler<CommandProjectileCollision>
{
    public void handle(CommandProjectileCollision t)
    {
        Universe.fight.projectiles.remove(t.projectileInstance);
        
        // Temp damage handling
        t.collider.fightStats.addedLife.value -= t.projectileInstance.dmg;
        if (t.collider.fightStats.addedLife.value <= 0)
        {
            Universe.fight.creatures.remove(t.collider);
        }
    }
}