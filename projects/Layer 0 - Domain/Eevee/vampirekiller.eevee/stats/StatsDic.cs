using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using Util.structures;

namespace VampireKiller.eevee.vampirekiller.eevee.stats;

public class StatsDic : SmartDictionary<Type, Stat>
{
    public const string EventUpdate = nameof(StatsDic) + ".changed";

    public override void initialize()
    {
        this.GetEntityBus().subscribe(this);
    }

    public T? get<T>(Type t) where T : Stat
    {
        return (T?)get(t);
    }

    public T? get<T>() where T : Stat
    {
        return (T?) get(typeof(T));
    }

    public void set(Stat s)
    {
        this.set(s.GetType(), s);
    }

    /// <summary>
    /// When the value of a stat changes
    /// </summary>
    /// <param name="stat"></param>
    [Subscribe(Stat.EventSet)]
    private void onChangedStat(Stat stat)
    {
       this.GetEntityBus().publish(EventUpdate, dic);
    }
    /// <summary>
    /// When a stat is added/set in the dictionary
    /// </summary>
    [Subscribe(StatsDic.EventSet)]
    private void onSetStat(StatsDic dic, Type t, Stat s)
    {
        s.GetEntityBus().subscribe(this);
        this.GetEntityBus().publish(EventUpdate, dic);
    }
    /// <summary>
    /// When a stat is removed/replaced in the dictionary
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="t"></param>
    /// <param name="s"></param>
    [Subscribe(StatsDic.EventRemove)]
    public void onRemoveStat(StatsDic dic, Type t, Stat s)
    {
        s.GetEntityBus().unsubscribe(this);
        this.GetEntityBus().publish(EventUpdate, dic);
    }

}
