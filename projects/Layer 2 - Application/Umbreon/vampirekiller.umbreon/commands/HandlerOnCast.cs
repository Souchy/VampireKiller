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

namespace vampirekiller.umbreon.commands;

public class HandlerOnCast : ICommandHandler<CommandCast>
{
    public void handle(CommandCast t)
    {
        // Temp
        ProjectileInstance projectile = Register.Create<ProjectileInstance>();
        projectile.init(
            t.originator,
            t.originatorFacing,
            9.69f,
            "res://scenes/db/projectiles/Fireball.tscn",
            1
        );
        Universe.fight.projectiles.add(projectile);
    }
}
