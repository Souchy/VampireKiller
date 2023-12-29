using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.structures;

namespace VampireKiller.eevee.vampirekiller.eevee.equipment;

public class Inventory
{
    /// <summary>
    /// All items, include passive and active
    /// </summary>
    public SmartList<Item> items { get; set; } = SmartList<Item>.Create();
    // /// <summary>
    // /// Items placed in active slots
    // /// </summary>
    // public SmartList<Item> activeSlots { get; set; } = SmartList<Item>.Create();
}
