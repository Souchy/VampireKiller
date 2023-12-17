using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.creature;

namespace VampireKiller.eevee.vampirekiller.eevee;

public class Fight : IDisposable
{
    public const string EventSet = "set";

    public SmartSet<CreatureInstance> creatures { get; init; } = SmartSet<CreatureInstance>.Create();
    public SmartSet<ProjectileInstance> projectiles { get; init; } = SmartSet<ProjectileInstance>.Create();

    public void Dispose()
    {
        creatures.Dispose();
        projectiles.Dispose();
        EventBus.centralBus.publish(nameof(Dispose), this);
    }
}
