using System;

namespace VampireKiller;

public interface CreatureStat { }

/// <summary>
/// Model
/// </summary>
// public class CreatureModelStats : Stats
// {
//     public new CreatureModelStatsDictionary flat => (CreatureModelStatsDictionary)base.flat;
//     public new CreatureModelStatsDictionary increase => (CreatureModelStatsDictionary)base.increase;
// }

public class CreatureModelStatsDictionary : StatsDictionary
{
    public CreatureLifeMax GetCreatureLifeMax() => this.get<CreatureLifeMax>();
    public CreatureManaMax GetCreatureManaMax() => this.get<CreatureManaMax>();
    public CreatureShield GetCreatureShield() => this.get<CreatureShield>();

    public CreaturelAttack GetCreatureAttack() => this.get<CreaturelAttack>();
    public CreaturelMagic GetCreatureMagic() => this.get<CreaturelMagic>();
    public CreaturelDefense GetCreatureDefense() => this.get<CreaturelDefense>();
    public CreatureMovementSpeed GetCreatureMovementSpeed() => this.get<CreatureMovementSpeed>();
}

public record CreatureLifeMax(int value) : StatInt(value), CreatureStat { }
public record CreatureManaMax(int value) : StatInt(value), CreatureStat { }
public record CreaturelMagic(int value) : StatInt(value), CreatureStat { }
public record CreaturelAttack(int value) : StatInt(value), CreatureStat { }
public record CreaturelDefense(int value) : StatInt(value), CreatureStat { }
public record CreatureMovementSpeed(double value) : StatDouble(value), CreatureStat { }
// Mana ? 
// Ammo/Bullets ? // per each item? / distinction between weapons and spells?


// Ammo are rather a stat on the weapon instance. We use SpellInstance.SpellBuiltinResource 
// public record CreatureResourceAmmo(int value) : StatInt(value), CreatureStat { }



/// <summary>
/// Instance
/// </summary>
public class CreatureInstanceStats : StatsDictionary
{
    public CreatureShield GetCreatureShield() => this.get<CreatureShield>();
    public CreatureResourceLife GetLife() => this.get<CreatureResourceLife>();
    public CreatureResourceMana GetMana() => this.get<CreatureResourceMana>();
}

public record CreatureResourceLife(int value) : StatInt(value), CreatureStat { }
public record CreatureResourceMana(int value) : StatInt(value), CreatureStat { }
public record CreatureShield(int value) : StatInt(value), CreatureStat { }


// public class CreatureTotalStats : StatsDictionary
// {
//     public CreatureResourceLife GetLife() => this.get<CreatureResourceLife>();
//     public CreatureResourceMana GetMana() => this.get<CreatureResourceMana>();
//     public CreatureShield GetCreatureShield() => this.get<CreatureShield>();
    
//     public CreatureLifeMax GetCreatureLifeMax() => this.get<CreatureLifeMax>();
//     public CreatureManaMax GetCreatureManaMax() => this.get<CreatureManaMax>();
//     public CreaturelAttack GetCreatureAttack() => this.get<CreaturelAttack>();
//     public CreaturelDefense GetCreatureDefense() => this.get<CreaturelDefense>();
//     public CreatureMovementSpeed GetCreatureMovementSpeed() => this.get<CreatureMovementSpeed>();
// }
