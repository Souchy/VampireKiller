using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Util.communication.events;
using Util.entity;
using Util.structures;

namespace VampireKiller.eevee.vampirekiller.eevee.stats;

public class StatsDic : SmartDictionary<Type, IStat>
{
    public const string EventUpdate = nameof(StatsDic) + ".changed";

    protected StatsDic() {}

    public override void initialize()
    {
        this.GetEntityBus().subscribe(this);
    }

    public T? get<T>(Type t) where T : IStat
    {
        return (T?) get(t);
    }

    public T? get<T>() where T : IStat
    {
        return (T?) get(typeof(T));
    }

    public void set(IStat s)
    {
        this.set(s.GetType(), s);
    }

    /// <summary>
    /// When the value of a stat changes
    /// </summary>
    [Subscribe(IStat.EventSet)]
    private void onChangedStat(IStat stat)
    {
        // GD.Print("StatsDic: onChangedStat: " + stat);
        this.GetEntityBus().publish(EventUpdate, stat);
    }
    /// <summary>
    /// When a stat is added/set in the dictionary
    /// </summary>
    [Subscribe(StatsDic.EventSet)]
    private void onSetStat(StatsDic dic, Type t, IStat s)
    {
        s.GetEntityBus()?.subscribe(this);
        this.GetEntityBus().publish(EventUpdate, dic);
    }
    /// <summary>
    /// When a stat is removed/replaced in the dictionary
    /// </summary>
    [Subscribe(StatsDic.EventRemove)]
    public void onRemoveStat(StatsDic dic, Type t, IStat s)
    {
        s.GetEntityBus()?.unsubscribe(this);
        this.GetEntityBus().publish(EventUpdate, dic);
    }

}
