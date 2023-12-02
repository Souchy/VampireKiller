using System;
using System.Collections.Generic;

namespace VampireKiller;

/// <summary>
/// SpellInstance can also be a weapon
/// </summary>
public class SpellInstance
{
    public Spell spellModel;
    public Stats instanceStats { get; set; } = new();
}
