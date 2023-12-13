using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace VampireKiller.eevee.creature;

public class CreatureInstance : Identifiable // : Node3D
{
    public ID entityUid { get; set; }
    public CreatureModel model { get; set; }
    public Vector3 position { get => positionHook(); }
    public Func<Vector3> positionHook { get; set; }

    public CreatureFightStats fightStats = new();
    // public CreatureInstanceStats resources { get; set; } = new();
    // public Inventory inventory { get; set; } = new();
    // public List<StatusInstance> statuses { get; set; } = new();

    public CreatureInstance() { }

    public void initialize()
    {
        fightStats.GetEntityBus().subscribe(this);

    }
    public void Dispose()
    {
        this.GetEntityBus().publish(nameof(Dispose), this);
        fightStats.Dispose();
    }

    public Stat getTotalStat<T>() where T : Stat, new()
    {
        var t = new T();
        t.add(this.model.baseStats);
        t.add(this.fightStats.dic);
        // TODO getTotalStat: items & statuses
        return t;
    }

    /// <summary>
    /// Bubble up les events de stats jusqu'au UI
    /// </summary>
    [Subscribe("stats.changed")]
    private void onStatsChanged(StatsDic dic)
    {
        this.GetEntityBus().publish("stats.changed", this);
    }

}
