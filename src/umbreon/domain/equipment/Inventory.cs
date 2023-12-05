using System;
using System.Collections.Generic;
using aaaaaaaa;

namespace VampireKiller;


public class Inventory
{

    /// <summary>
    /// Active Slots (max 6)
    /// </summary>
    public List<ItemInstance> equipedActives { get; set; } = new();

    /// <summary>
    /// Passive Slots (max 20)
    /// </summary>
    public List<ItemInstance> equipedPassives { get; set; } = new();

    /// <summary>
    /// Everything else
    /// </summary>
    public List<ItemInstance> stash { get; set; } = new();

}
