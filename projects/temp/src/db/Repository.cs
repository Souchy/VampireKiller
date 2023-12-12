using System.Collections.Generic;
using System;

namespace VampireKiller;

public class Repository
{

    public Dictionary<int, Enemy> enemies = new();
    public Dictionary<int, SpellModel> spells = new();
    public Dictionary<int, ItemModel> items = new();


}
