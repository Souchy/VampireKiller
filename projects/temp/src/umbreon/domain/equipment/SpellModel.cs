using System;
using System.Collections.Generic;

namespace VampireKiller;

/// <summary>
/// Spells are given by items
/// </summary>
public class SpellModel
{
    public SpellModelStatsDictionary modelStats { get; set; } = new();
    public List<Statement> statements { get; set; } = new();
}


/// <summary>
/// SpellInstance
/// </summary>
public class SpellInstance
{
    public SpellModel spellModel;
    public SpellInstanceStatsDictionary instanceStats { get; set; } = new();
}
