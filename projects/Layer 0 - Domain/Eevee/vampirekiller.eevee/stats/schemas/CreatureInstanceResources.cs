using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using Util.structures;
using vampirekiller.eevee.stats.schemas.resources;

namespace VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

/// <summary>
/// Lorsqu'on prend du dommage ou du heal, on va généralement utiliser CreatureAddedLife pour l'ajotuer à la fin du calcul de vie total
/// </summary>
public class CreatureFightStats : IDisposable
{
    public StatsDic dic { get; init; } = Register.Create<StatsDic>();

    public CreatureAddedLife addedLife
    {
        get => dic.get<CreatureAddedLife>()!;
    }
    public CreatureAddedLifeMax addedLifeMax
    {
        get => dic.get<CreatureAddedLifeMax>()!;
    }
    public CreatureFightStats()
    {
        // Actual stats de combat
        dic.set(Register.Create<CreatureAddedLife>());
        dic.set(Register.Create<CreatureAddedLifeMax>());
        // Stats influencées par les levels? (growth)
        dic.set(Register.Create<CreatureBaseLife>());
        dic.set(Register.Create<CreatureBaseLifeMax>());
        dic.set(Register.Create<CreatureIncreasedLife>());
        dic.set(Register.Create<CreatureIncreasedLifeMax>());
        // Si on veut un arbre de passifs avec des choix,
        //      alors faudrait un objet dans creature: PassiveTree { list<Passive> } et Passive { StatsDic bonus; ou List<Statement> statements; }
        //      de la même manière que les items et status
    }
    public IEventBus GetEntityBus() => dic.GetEntityBus();

    public void Dispose() => dic.Dispose();
}


// Damage & Resistance
public class IncreasedDamage : StatInt { }
public class IncreasedDirectDamage : StatInt { }
public class IncreasedIndirectDamage : StatInt { }
public class PercentResistance : StatInt { }
public class AddedDamageReduction : StatInt { }

// Movement speed
public class CreatureBaseMovementSpeed : StatDouble { }
public class CreatureIncreasedMovementSpeed : StatDouble { }
public class CreatureTotalMovementSpeed : StatDoubleTotal<CreatureBaseMovementSpeed, CreatureIncreasedMovementSpeed> { }


