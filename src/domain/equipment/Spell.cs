using System;
using System.Collections.Generic;

namespace VampireKiller;

/// <summary>
/// A spell can also be a weapon. Ak it's any kind of active ability/skill
/// Spells are given by items
/// </summary>
public class Spell
{
    public Stats spellStats { get; set; } = new();
    public List<Effect> effects { get; set; } = new();
}
