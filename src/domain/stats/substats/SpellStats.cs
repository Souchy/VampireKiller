namespace VampireKiller;

public class SpellStats : Stats
{
    public new SpellStatsDictionary flat => (SpellStatsDictionary)base.baseFlat;
    public new SpellStatsDictionary increase => (SpellStatsDictionary)base.increase;
}

public class SpellStatsDictionary : StatsDictionary
{
    public SpellRepeat GetSpellRepeat() => this.get<SpellRepeat>();
    public SpellResourceCostType GetSpellResourceCostType() => this.get<SpellResourceCostType>();
    public SpellResourceCost GetSpellResourceCost() => this.get<SpellResourceCost>();
    public SpellCastTime GetSpellCastTime() => this.get<SpellCastTime>();
    public SpellBaseCooldown GetSpellBaseCooldown() => this.get<SpellBaseCooldown>();
    public SpellMaxCharges GetSpellMaxCharges() => this.get<SpellMaxCharges>();

}

public interface SpellStat { }

public record SpellRepeat(double value) : StatDouble(value), SpellStat { }

public record SpellResourceCostType(int value) : StatInt(value), SpellStat { }
public record SpellResourceCost(int value) : StatInt(value), SpellStat { }
public record SpellCastTime(double value) : StatDouble(value), SpellStat { }
public record SpellBaseCooldown(double value) : StatDouble(value), SpellStat { }
public record SpellMaxCharges(int value) : StatInt(value), SpellStat { }

