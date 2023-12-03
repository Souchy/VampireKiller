using System;
using System.Collections.Generic;

namespace VampireKiller;

public class ItemModel
{
    // public Stats bonusStats { get; set; } = new();
    public List<Statement> effects { get; set; } = new();
    public SpellModel attachedSpell;
}

public class ItemInstance
{
    public ItemModel model { get; set; }
    public SpellInstance spell;
}

// public class Weapon : Item
// {
//     public Stats weaponStats { get; set; } = new();
// }
