using System;
using System.Collections.Generic;
using Godot;

namespace VampireKiller;

// public class ItemModel
// {
//     // public Stats bonusStats { get; set; } = new();
//     public List<Statement> effects { get; set; } = new();
//     public SpellModel attachedSpell;
// }

// public class ItemInstance
// {
//     public ItemModel model { get; set; }
//     public SpellInstance spell;
// }

// public class Weapon : Item
// {
//     public Stats weaponStats { get; set; } = new();
// }

// ------------------------------------------------------------------------------------------------------ Code
public class ItemModel
{

    public string icon { get; init; }
    public string equipementScenePath { get; init; }
    public StatsDictionary stats { get; set; } = new();
    // public List<Statement> statements;
}
public class WeaponModel : ItemModel
{
    public int ammoMax { get; set; }
    public int rechargeRate { get; set; }
    public int attackTime { get; set; }
}
public class SpellScrollModel : ItemModel
{
    public SpellModel spellModel { get; init; }
}
// --------------------------------------------------------------------------------
public class ItemInstance
{
    public ItemModel itemModel { get; init; }
    public int quantity = 0;
}

public interface Active
{
    public void cast(CreatureInstance caster, Vector3 cursor);
}

public class Weapon : ItemInstance //, Active
{
    public int currentAmmo { get; set; }
    public WeaponModel weaponModel => (WeaponModel)this.itemModel;
    // public void cast(CreatureInstance caster, Vector3 cursor)
    // {
    //     // spawn Projectile Effects
    // }
}

public class SpellScroll : ItemInstance //, Active
{
    public SpellScrollModel spellScrollModel => (SpellScrollModel)this.itemModel;
    public int cooldown;
    public int charges;

    // public void cast(CreatureInstance caster, Vector3 cursor)
    // {
    //     // spawn Aoe Effect
    // }
}
