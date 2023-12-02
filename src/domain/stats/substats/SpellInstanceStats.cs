using System;

namespace VampireKiller;

/// <summary>
/// 
/// </summary>
public class SpellInstanceStats : Stats
{
    public new SpellInstanceStatsDictionary flat => (SpellInstanceStatsDictionary)base.baseFlat;
    public new SpellInstanceStatsDictionary increase => (SpellInstanceStatsDictionary)base.increase;
}

public class SpellInstanceStatsDictionary : StatsDictionary
{
    public SpellInstanceCooldownUntil GetCooldownUntil() => this.get<SpellInstanceCooldownUntil>();
    public SpellInstanceCurrentCharges GetCurrentCharges() => this.get<SpellInstanceCurrentCharges>();
    public SpellInstanceBuiltinResource GetBuiltinResource() => this.get<SpellInstanceBuiltinResource>();
}

public interface SpellInstanceStat { }

/// <summary>
/// The DateTime when the spell will come off cooldown. <br>
/// To refund cooldown, just move the date back in time
/// </summary>
public record SpellInstanceCooldownUntil(DateTime value) : StatDate(value), SpellInstanceStat { }
public record SpellInstanceCurrentCharges(int value) : StatInt(value), SpellInstanceStat { }

public record SpellInstanceBuiltinResource(int value) : StatInt(value), SpellInstanceStat { }
