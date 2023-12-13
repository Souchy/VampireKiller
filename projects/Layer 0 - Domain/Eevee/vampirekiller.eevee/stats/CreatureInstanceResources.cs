using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using Util.structures;

namespace VampireKiller.eevee.vampirekiller.eevee.stats;
public class CreatureFightStats : IDisposable
{
    public StatsDic dic { get; init; } = Register.Create<StatsDic>();

    public CreatureFlatLife life
    {
        get => dic.get<CreatureFlatLife>();
        set => dic.set(value);
    }
    public CreatureFlatLifeMax lifeMax
    {
        get => dic.get<CreatureFlatLifeMax>();
        set => dic.set(value);
    }
    public CreatureFightStats()
    {
        dic.set(Register.Create<CreatureFlatLife>());
        dic.set(Register.Create<CreatureFlatLifeMax>());
        dic.set(Register.Create<CreatureIncreaseLife>());
        dic.set(Register.Create<CreatureIncreaseLifeMax>());
    }
    public IEventBus GetEntityBus() => this.dic.GetEntityBus();

    public void Dispose() => dic.Dispose();
}


public class CreatureFlatLife : StatInt { }
public class CreatureFlatLifeMax : StatInt { }
public class CreatureIncreaseLife : StatInt { }
public class CreatureIncreaseLifeMax : StatInt { }

public class CreatureTotalLife : StatInt
{
    private int totalFlat = 0;
    private int totalIncrease = 0;
    public override int value
    {
        get
        {
            return (int) (totalFlat * ((100.0 + totalIncrease) / 100.0));
        }
        set { }
    }
    public override void add(StatsDic dic)
    {
        var flat = dic.get<CreatureFlatLife>();
        var inc = dic.get<CreatureIncreaseLife>();
        if (flat != null) totalFlat += flat.value;
        if (inc != null) totalIncrease += inc.value;
    }
}
public class CreatureTotalLifeMax : StatInt
{
    private int totalFlat = 0;
    private int totalIncrease = 0;
    public override int value
    {
        get
        {
            return (int) (totalFlat * ((100.0 + totalIncrease) / 100.0));
        }
        set { }
    }
    public override void add(StatsDic dic)
    {
        var flat = dic.get<CreatureFlatLifeMax>();
        var inc = dic.get<CreatureIncreaseLifeMax>();
        if (flat != null) totalFlat += flat.value;
        if (inc != null) totalIncrease += inc.value;
    }
}