using System;
using System.Collections.Generic;

namespace VampireKiller;

public class CreatureModel
{
    public CreatureModelStatsDictionary stats { get; set; } = new();

    public CreatureInstance createInstance()
    {
        return null;
    }
}

public class CreatureInstance
{
    public CreatureModel model;
    public CreatureInstanceStats resources { get; set; } = new();
    public Inventory inventory { get; set; } = new();
    public List<StatusInstance> statuses { get; set; } = new();
    /// <summary>
    /// Total = items + status + fight
    /// </summary>
    public StatsDictionary getTotalStats()
    {
        var result = new StatsDictionary();
        return result;
    }
}
