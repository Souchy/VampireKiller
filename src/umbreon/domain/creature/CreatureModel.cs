using System;
using System.Collections.Generic;
using Godot;

namespace VampireKiller;

public class CreatureModel
{
    public CreatureModelStatsDictionary stats { get; set; } = new();

    public CreatureInstance createInstance()
    {
        return null;
    }
}

public partial class CreatureInstance : Node3D
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
    // public Vector3 getPosition() {
    //     return new();
    // }
}
