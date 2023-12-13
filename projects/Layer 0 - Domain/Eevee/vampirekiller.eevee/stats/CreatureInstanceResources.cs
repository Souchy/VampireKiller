using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using Util.structures;

namespace VampireKiller.eevee.vampirekiller.eevee.stats;

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
        dic.set(Register.Create<CreatureIncreaseLife>());
        dic.set(Register.Create<CreatureIncreaseLifeMax>());
        // Si on veut un arbre de passifs avec des choix,
        //      alors faudrait un objet dans creature: PassiveTree { list<Passive> } et Passive { StatsDic bonus; }
        //      de la même manière que les items et status
    }
    public IEventBus GetEntityBus() => this.dic.GetEntityBus();

    public void Dispose() => dic.Dispose();
}


public class CreatureBaseLife : StatInt { }
public class CreatureBaseLifeMax : StatInt { }
public class CreatureIncreaseLife : StatInt { }
public class CreatureIncreaseLifeMax : StatInt { }
public class CreatureAddedLife : StatInt { }
public class CreatureAddedLifeMax : StatInt { }

public class CreatureTotalLife : StatInt
{
    private int totalBase = 0;
    private int totalIncrease = 0;
    private int totalAdded = 0;
    public override int value
    {
        get
        {
            return (int) (totalBase * ((100.0 + totalIncrease) / 100.0)) + totalAdded;
        }
        set { }
    }
    public override void add(StatsDic dic)
    {
        var flat = dic.get<CreatureBaseLife>();
        var inc = dic.get<CreatureIncreaseLife>();
        var added = dic.get<CreatureAddedLife>();
        if (flat != null) totalBase += flat.value;
        if (inc != null) totalIncrease += inc.value;
        if (added != null) totalAdded += added.value;
    }
}
public class CreatureTotalLifeMax : StatInt
{
    private int totalFlat = 0;
    private int totalIncrease = 0;
    private int totalAdded = 0;
    public override int value
    {
        get
        {
            return (int) (totalFlat * ((100.0 + totalIncrease) / 100.0)) + totalAdded;
        }
        set { }
    }
    public override void add(StatsDic dic)
    {
        var flat = dic.get<CreatureBaseLifeMax>();
        var inc = dic.get<CreatureIncreaseLifeMax>();
        var added = dic.get<CreatureAddedLifeMax>();
        if (flat != null) totalFlat += flat.value;
        if (inc != null) totalIncrease += inc.value;
        if (added != null) totalAdded += added.value;
    }
}