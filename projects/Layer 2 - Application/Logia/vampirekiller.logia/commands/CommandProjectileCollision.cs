using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using VampireKiller.eevee;
using VampireKiller.eevee.creature;

namespace vampirekiller.logia.commands;

// Probably not the best way of doing it, we can generalise this to a "CommandEffectCollision" to encapsulate different spell effects colliding (like auras, aoes, rays, etc..)
public record struct CommandProjectileCollision : ICommand
{
    public ProjectileInstance projectileInstance;
    public CreatureInstance collider;

    public CommandProjectileCollision(ProjectileInstance projectileInstance, CreatureInstance collider)
    {
        this.projectileInstance = projectileInstance;
        this.collider = collider;
    }

    public byte[] serialize()
    {
        throw new NotImplementedException();
    }
}
