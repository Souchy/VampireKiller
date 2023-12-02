using System;
using System.Collections.Generic;

namespace VampireKiller;

public class Item
{
    public Stats bonusStats { get; set; } = new();
    public List<Effect> effects { get; set; } = new();
    public Spell attachedSpell;
}

public class ItemInstance
{
    public Item model { get; set; }
    public SpellInstance spell;
}

// public class Weapon : Item
// {
//     public Stats weaponStats { get; set; } = new();
// }
