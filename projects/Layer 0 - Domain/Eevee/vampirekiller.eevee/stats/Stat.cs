using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.conditions;

namespace VampireKiller.eevee.vampirekiller.eevee.stats;

public interface IStat : Identifiable
{
    public const string EventSet = "stat.set";
    public List<ICondition> conditions { get; set; }
    public IStat copy();
    public void add(StatsDic dic);
}


public class StatInt : IStat
{
    public ID entityUid { get; set; }
    public List<ICondition> conditions { get; set; } = new();
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
            this.GetEntityBus().publish(IStat.EventSet, this, before);
            this.GetEntityBus().publish(IStat.EventSet, this);
        }
    }

    public StatInt() { }
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatInt>(this.GetType());
        if(val != null) 
            this.value += val.value;
    }
    public IStat copy()
    {
        var copy = new StatInt();
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
            this.GetEntityBus().publish(IStat.EventSet, this, before);
            this.GetEntityBus().publish(IStat.EventSet, this);
        }
    }
    public StatType() { }
    public virtual void add(StatsDic dic)
    {
        throw new NotImplementedException();
    }
    public IStat copy()
    {
        var copy = new StatType();
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
            this.GetEntityBus().publish(IStat.EventSet, this, before);
            this.GetEntityBus().publish(IStat.EventSet, this);
        }
    }
    public StatDate() { }
    public virtual void add(StatsDic dic)
    {
        throw new NotImplementedException();
    }
    public IStat copy()
    {
        var copy = new StatDate();
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
            this.GetEntityBus().publish(IStat.EventSet, this, before);
            this.GetEntityBus().publish(IStat.EventSet, this);
        }
    }
    public StatTimeSpan() { }
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatTimeSpan>(this.GetType());
        if (val != null)
            this.value += val.value;
    }
    public IStat copy()
    {
        var copy = new StatTimeSpan();
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
            this.GetEntityBus().publish(IStat.EventSet, this, before);
            this.GetEntityBus().publish(IStat.EventSet, this);
        }
    }
    public StatDouble() { }
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatDouble>(this.GetType());
        if (val != null)
            this.value += val.value;
    }
    public IStat copy()
    {
        var copy = new StatDouble();
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
    private bool _value;
    public virtual bool value
    {
        get {
            return _value;
        }
        set {
            var before = _value;
            _value = value;
            this.GetEntityBus().publish(IStat.EventSet, this, before);
            this.GetEntityBus().publish(IStat.EventSet, this);
        }
    }
    public StatBool() { }
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
        var copy = new StatBool();
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
