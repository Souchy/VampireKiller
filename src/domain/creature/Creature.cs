using System;
using System.Collections.Generic;

namespace VampireKiller;

public class Creature
{

    public Stats stats { get; set; } = new();

    public Inventory inventory { get; set; } = new();

}
