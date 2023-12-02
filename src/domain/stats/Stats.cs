
using System;
using System.Collections.Generic;

public class CompoundStats {
    
}

public class Stats
{
    public StatsDictionary baseFlat = new();
    public StatsDictionary increase = new();
    /// <summary>
    /// 
    /// </summary>
    public StatsDictionary moreFlat = new();
    public StatsDictionary set = new();
    public T get<T>() where T : Stat
    {
        if (set.has<T>())
            return set.get<T>();
        T flat = baseFlat.get<T>();
        T inc = increase.get<T>();
        T more = moreFlat.get<T>();
        return default;
    }
}

// public class StatsFlat : StatsDictionary { }
// public class StatsIncrease : StatsDictionary { }

public class StatsDictionary
{
    private Dictionary<Type, Stat> stats { get; set; } = new();
    public bool has<T>()
    {
        return stats.ContainsKey(typeof(T));
    }
    public void set(Stat stat)
    {
        this.stats[typeof(Stat)] = stat;
    }
    public T get<T>() where T : Stat
    {
        if (!stats.ContainsKey(typeof(T)))
            return default;
        return (T)stats[typeof(T)];
    }
}

public interface Stat
{
    // public void add<T>(T t) where T : Stat;
    // public void increase<T>(T t) where T : Stat;
}
public record StatInt : Stat
{
    public int value { get; set; } = 0;
    public StatInt(int value = 0) => this.value = value;
    public void add(StatInt statFlat)
    {
        this.value += statFlat.value;
    }
    public void increase(StatInt statPercentage)
    {
        this.value *= (statPercentage.value + 100) / 100;
    }

    public void add<T>(T t) where T : Stat
    {
        throw new NotImplementedException();
    }

    public void increase<T>(T t) where T : Stat
    {
        throw new NotImplementedException();
    }
}
public record StatDate : Stat
{
    public DateTime value { get; set; }
    public StatDate(DateTime value) => this.value = value;
    public void add(StatTimeSpan flat)
    {
        this.value.Add(flat.value);
    }
    public void increase(StatInt statPercentage)
    {
        // this.value *= (statPercentage.value + 100) / 100;
    }
}
public record StatTimeSpan : Stat
{
    public TimeSpan value { get; set; }
    public StatTimeSpan(TimeSpan value) => this.value = value;
    public void add(StatTimeSpan flat)
    {
        this.value.Add(flat.value);
    }
    public void increase(StatInt statPercentage)
    {
        // this.value *= (statPercentage.value + 100) / 100;
    }
}
public record StatDouble : Stat
{
    public double value { get; set; }
    public StatDouble(double value = 0) => this.value = value;
    public void add(StatDouble statFlat)
    {
        this.value += statFlat.value;
    }
    public void increase(StatDouble statPercentage)
    {
        this.value *= (statPercentage.value + 100) / 100;
    }
}
public record StatBool : Stat
{
    public bool value { get; set; }
    public StatBool(bool value = false) => this.value = value;
    public void and(StatBool stat) {
        this.value &= stat.value;
    }
    public void or(StatBool stat) {
        this.value |= stat.value;
    }
}


namespace EffectStats
{
    public record EffectRadius(double value) : StatDouble(value) { }
}

namespace ProjectileStats
{
    public record ProjectileSpeed(double value) : StatDouble(value) { }
    public record ProjectileFork(int value) : StatInt(value) { }
    public record ProjectileChain(int value) : StatInt(value) { }
    public record ProjectilePierce(int value) : StatInt(value) { }
    public record ProjectileRadius(double value) : StatDouble(value) { }
}

