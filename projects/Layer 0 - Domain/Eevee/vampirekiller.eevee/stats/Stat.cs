﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Util.communication.events;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.conditions;

namespace VampireKiller.eevee.vampirekiller.eevee.stats;

public interface IStat : Identifiable
{
    public const string EventSet = "stat.set";
    [JsonIgnore]
    public object genericValue { get; }
    public List<ICondition> conditions { get; set; }
    public IStat copy();
    public void add(StatsDic dic);
}


public class StatInt : IStat
{
    public ID entityUid { get; set; }
    public List<ICondition> conditions { get; set; } = new();
    public object genericValue => value;
    private int _value;
    public virtual int value
    {
        get
        {
            return _value;
        }
        set
        {
            var before = _value;
            _value = value;
            this.GetEntityBus()?.publish(IStat.EventSet, this, before);
            this.GetEntityBus()?.publish(IStat.EventSet, this);
        }
    }

    protected StatInt() { }
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatInt>(this.GetType());
        if(val != null) 
            this.value += val.value;
    }
    public IStat copy()
    {
        var copy = (StatInt) Activator.CreateInstance(this.GetType(), true)!;
        copy.value = this.value;
        foreach(var cond in conditions)
            copy.conditions.Add(cond.copy());
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }

}
public class StatType : IStat
{
    public ID entityUid { get; set; }
    public List<ICondition> conditions { get; set; } = new();
    public object genericValue => value;
    private Type _value;
    public virtual Type value
    {
        get
        {
            return _value;
        }
        set
        {
            var before = _value;
            _value = value;
            this.GetEntityBus()?.publish(IStat.EventSet, this, before);
            this.GetEntityBus()?.publish(IStat.EventSet, this);
        }
    }
    protected StatType() { }
    public virtual void add(StatsDic dic)
    {
        throw new NotImplementedException();
    }
    public IStat copy()
    {
        var copy = (StatType) Activator.CreateInstance(this.GetType(), true)!;
        copy.value = this.value;
        foreach (var cond in conditions)
            copy.conditions.Add(cond.copy());
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
public class StatDate : IStat
{
    public ID entityUid { get; set; }
    public List<ICondition> conditions { get; set; } = new();
    public object genericValue => value;
    private DateTime _value;
    public virtual DateTime value
    {
        get
        {
            return _value;
        }
        set
        {
            var before = _value;
            _value = value;
            this.GetEntityBus()?.publish(IStat.EventSet, this, before);
            this.GetEntityBus()?.publish(IStat.EventSet, this);
        }
    }
    protected StatDate() { }
    public virtual void add(StatsDic dic)
    {
        throw new NotImplementedException();
    }
    public IStat copy()
    {
        var copy = (StatDate) Activator.CreateInstance(this.GetType(), true)!;
        copy.value = this.value;
        foreach (var cond in conditions)
            copy.conditions.Add(cond.copy());
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
public class StatTimeSpan : IStat
{
    public ID entityUid { get; set; }
    public List<ICondition> conditions { get; set; } = new();
    public object genericValue => value;
    private TimeSpan _value;
    public virtual TimeSpan value
    {
        get
        {
            return _value;
        }
        set
        {
            var before = _value;
            _value = value;
            this.GetEntityBus()?.publish(IStat.EventSet, this, before);
            this.GetEntityBus()?.publish(IStat.EventSet, this);
        }
    }
    protected StatTimeSpan() { }
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatTimeSpan>(this.GetType());
        if (val != null)
            this.value += val.value;
    }
    public IStat copy()
    {
        var copy = (StatTimeSpan) Activator.CreateInstance(this.GetType(), true)!;
        copy.value = this.value;
        foreach (var cond in conditions)
            copy.conditions.Add(cond.copy());
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
public class StatDouble : IStat
{
    public ID entityUid { get; set; }
    public List<ICondition> conditions { get; set; } = new();
    public object genericValue => value;
    private double _value;
    public virtual double value
    {
        get
        {
            return _value;
        }
        set
        {
            var before = _value;
            _value = value;
            this.GetEntityBus()?.publish(IStat.EventSet, this, before);
            this.GetEntityBus()?.publish(IStat.EventSet, this);
        }
    }
    protected StatDouble() { }
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatDouble>(this.GetType());
        if (val != null)
            this.value += val.value;
    }
    public IStat copy()
    {
        var copy = (StatDouble) Activator.CreateInstance(this.GetType(), true)!;
        copy.value = this.value;
        foreach (var cond in conditions)
            copy.conditions.Add(cond.copy());
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
public class StatBool : IStat
{
    public ID entityUid { get; set; }
    public List<ICondition> conditions { get; set; } = new();
    public object genericValue => value;
    private bool _value;
    public virtual bool value
    {
        get {
            return _value;
        }
        set {
            var before = _value;
            _value = value;
            this.GetEntityBus()?.publish(IStat.EventSet, this, before);
            this.GetEntityBus()?.publish(IStat.EventSet, this);
        }
    }
    protected StatBool() { }
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatBool>(this.GetType());
        if (val != null)
            and(val);
    }
    public void and(StatBool stat)
    {
        this.value &= stat.value;
    }
    public void or(StatBool stat)
    {
        this.value |= stat.value;
    }
    public IStat copy()
    {
        var copy = (StatBool) Activator.CreateInstance(this.GetType(), true)!;
        copy.value = this.value;
        foreach (var cond in conditions)
            copy.conditions.Add(cond.copy());
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}


public class StatIntTotal<B, I> : StatInt where B : StatInt where I : StatInt {
    protected int totalBase = 0;
    protected int totalIncrease = 0;
    public override int value
    {
        get
        {
            return (int)(totalBase * ((100.0 + totalIncrease) / 100.0));
        }
        set { }
    }
    public override void add(StatsDic dic)
    {
        var flat = dic.get<B>();
        var inc = dic.get<I>();
        if (flat != null) totalBase += flat.value;
        if (inc != null) totalIncrease += inc.value;
    }
}
/// <summary>
/// used for things like CreatureTotalLife, CreatureTotalLifeMax, which include fight damage, heals, erosion, buffs...
/// </summary>
public class StatIntTotalFight<B, I, A> : StatIntTotal<B, I> where B : StatInt where I : StatInt where A : StatInt
{
    protected int totalAdded = 0;
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
        base.add(dic);
        var added = dic.get<A>();
        if (added != null) totalAdded += added.value;
    }
}

public class StatDoubleTotal<B, I> : StatDouble where B : StatDouble where I : StatDouble {
    protected double totalBase = 0;
    protected double totalIncrease = 0;
    public override double value
    {
        get
        {
            return totalBase * ((100.0 + totalIncrease) / 100.0);
        }
        set { }
    }
    public override void add(StatsDic dic)
    {
        var flat = dic.get<B>();
        var inc = dic.get<I>();
        if (flat != null) totalBase += flat.value;
        if (inc != null) totalIncrease += inc.value;
    }
}
