using System;
using System.Collections.Generic;

namespace VampireKiller;

/// <summary>
/// A spell can also be a weapon. Ak it's any kind of active ability/skill
/// Spells are given by items
/// </summary>
public class SpellModel
{
    public SpellModelStatsDictionary spellStats { get; set; } = new();
    public List<Effect> effects { get; set; } = new();
}


/// <summary>
/// SpellInstance can also be a weapon
/// </summary>
public class SpellInstance
{
    public SpellModel spellModel;
    public SpellInstanceStatsDictionary instanceStats { get; set; } = new();
}
