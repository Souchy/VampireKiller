namespace VampireKiller;

public class CreatureStats : Stats
{
    public new CreatureStatsDictionary flat => (CreatureStatsDictionary)base.baseFlat;
    public new CreatureStatsDictionary increase => (CreatureStatsDictionary)base.increase;
}

public class CreatureStatsDictionary : StatsDictionary
{
    public CreatureResourceLife GetCreatureLife() => this.get<CreatureResourceLife>();    
    public CreatureResourceLifeMax GetCreatureLifeMax() => this.get<CreatureResourceLifeMax>();
    public CreatureShield GetCreatureShield() => this.get<CreatureShield>();
    public CreatureAttack GetCreatureAttack() => this.get<CreatureAttack>();
    public CreatureMovementSpeed GetCreatureMovementSpeed() => this.get<CreatureMovementSpeed>();
}

public interface CreatureStat { }

public record CreatureResourceLife(int value) : StatInt(value), CreatureStat { }
public record CreatureResourceLifeMax(int value) : StatInt(value), CreatureStat { }
public record CreatureShield(int value) : StatInt(value), CreatureStat { }
public record CreatureAttack(int value) : StatInt(value), CreatureStat { }
public record CreatureMovementSpeed(double value) : StatDouble(value), CreatureStat { }
// Mana ? 
// Ammo/Bullets ? // per each item? / distinction between weapons and spells?


public record CreatureResourceMana(int value) : StatInt(value), CreatureStat { }
// Ammo are rather a stat on the weapon instance. We use SpellInstance.SpellBuiltinResource 
// public record CreatureResourceAmmo(int value) : StatInt(value), CreatureStat { }
