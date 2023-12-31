﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.ecs;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.spells;

namespace VampireKiller.eevee.vampirekiller.eevee;

public class Fight : IDisposable
{
    public const string EventSet = "set";

    public EntityFamily entities {get; init;} = Register.Create<EntityFamily>();
    public SmartSet<CreatureInstance> creatures { get; init; } = SmartSet<CreatureInstance>.Create();
    public SmartSet<ProjectileInstance> projectiles { get; init; } = SmartSet<ProjectileInstance>.Create();
    public SmartSet<Item> items { get; init; } = SmartSet<Item>.Create();
    public SmartSet<SpellInstance> spells { get; init; } = SmartSet<SpellInstance>.Create();

    public Fight() {
        EventBus.centralBus.subscribe(entities);
    }

    public void Dispose()
    {
        creatures.Dispose();
        projectiles.Dispose();
        EventBus.centralBus.publish(nameof(Dispose), this);
    }
}
