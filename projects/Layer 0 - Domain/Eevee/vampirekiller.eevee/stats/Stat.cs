using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;

namespace VampireKiller.eevee.vampirekiller.eevee.stats;

public interface Stat : Identifiable
{
    public const string EventSet = "stat.set";
    public Stat copy();
    public void add(StatsDic dic);
}


public class StatInt : Stat
{
    public ID entityUid { get; set; }
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
            this.GetEntityBus().publish(Stat.EventSet, this, before);
            this.GetEntityBus().publish(Stat.EventSet, this);
        }
    }

    public StatInt(int value = 0) => this.value = value;
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatInt>(this.GetType());
        if(val != null) 
            this.value += val.value;
    }
    //public void add(StatInt statFlat)
    //{
    //    this.value += statFlat.value;
    //}
    //public void increase(StatInt statPercentage)
    //{
    //    this.value *= (statPercentage.value + 100) / 100;
    //}
    public Stat copy()
    {
        var copy = new StatInt();
        copy.value = this.value;
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }

}
public class StatType : Stat
{
    public ID entityUid { get; set; }
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
            this.GetEntityBus().publish(Stat.EventSet, this, before);
            this.GetEntityBus().publish(Stat.EventSet, this);
        }
    }
    public StatType() { }
    public StatType(Type value) => this.value = value;
    public virtual void add(StatsDic dic)
    {
        throw new NotImplementedException();
    }
    public Stat copy()
    {
        var copy = new StatType();
        copy.value = this.value;
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
public class StatDate : Stat
{
    public ID entityUid { get; set; }
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
            this.GetEntityBus().publish(Stat.EventSet, this, before);
            this.GetEntityBus().publish(Stat.EventSet, this);
        }
    }
    public StatDate() { }
    public StatDate(DateTime value) => this.value = value;
    public virtual void add(StatsDic dic)
    {
        throw new NotImplementedException();
        //var val = dic.get<StatDate>(this.GetType());
        //if (val != null)
        //    this.value += val.value;
    }
    //public void add(StatTimeSpan flat)
    //{
    //    this.value.Add(flat.value);
    //}
    //public void increase(StatInt statPercentage)
    //{
    //    // this.value *= (statPercentage.value + 100) / 100;
    //}
    public Stat copy()
    {
        var copy = new StatDate();
        copy.value = this.value;
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
public class StatTimeSpan : Stat
{
    public ID entityUid { get; set; }
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
            this.GetEntityBus().publish(Stat.EventSet, this, before);
            this.GetEntityBus().publish(Stat.EventSet, this);
        }
    }
    public StatTimeSpan() { }
    public StatTimeSpan(TimeSpan value) => this.value = value;
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatTimeSpan>(this.GetType());
        if (val != null)
            this.value += val.value;
    }
    //public void add(StatTimeSpan flat)
    //{
    //    this.value.Add(flat.value);
    //}
    //public void increase(StatInt statPercentage)
    //{
    //    // this.value *= (statPercentage.value + 100) / 100;
    //}
    public Stat copy()
    {
        var copy = new StatTimeSpan();
        copy.value = this.value;
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
public class StatDouble : Stat
{
    public ID entityUid { get; set; }
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
            this.GetEntityBus().publish(Stat.EventSet, this, before);
            this.GetEntityBus().publish(Stat.EventSet, this);
        }
    }
    public StatDouble(double value = 0) => this.value = value;
    public virtual void add(StatsDic dic)
    {
        var val = dic.get<StatDouble>(this.GetType());
        if (val != null)
            this.value += val.value;
    }
    //public void add(StatDouble statFlat)
    //{
    //    this.value += statFlat.value;
    //}
    //public void increase(StatDouble statPercentage)
    //{
    //    this.value *= (statPercentage.value + 100) / 100;
    //}
    public Stat copy()
    {
        var copy = new StatDouble();
        copy.value = this.value;
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
public class StatBool : Stat
{
    public ID entityUid { get; set; }
    private bool _value;
    public virtual bool value
    {
        get {
            return _value;
        }
        set {
            var before = _value;
            _value = value;
            this.GetEntityBus().publish(Stat.EventSet, this, before);
            this.GetEntityBus().publish(Stat.EventSet, this);
        }
    }
    public StatBool(bool value = false) => this.value = value;
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
    public Stat copy()
    {
        var copy = new StatBool();
        copy.value = this.value;
        return copy;
    }

    public void Dispose()
    {
        this.DisposeEventBus();
    }
}
