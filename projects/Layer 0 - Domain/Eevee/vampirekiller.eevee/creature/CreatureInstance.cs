using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.ecs;
using Util.entity;
using vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace VampireKiller.eevee.creature;


public class CreatureInstance : Entity, Identifiable
{
    public ID entityUid { get; set; }
    public CreatureModel model { get; set; }
    public CreatureGroupType creatureGroup { get; set; }

    public Vector3 spawnPosition { get; set; }
    public Vector3 position { get => getPositionHook();  set => setPositionHook(value); }
    public Func<Vector3> getPositionHook { get; set; }
    public Action<Vector3> setPositionHook { get; set; }

    public CreatureFightStats fightStats = new();
     public Inventory inventory { get; set; } = new();
    // public CreatureInstanceStats resources { get; set; } = new();
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

    public T getTotalStat<T>() where T : IStat, new()
    {
        var t = new T();
        t.add(this.model.baseStats);
        t.add(this.fightStats.dic);
        foreach(var item in inventory.items.values)
            item.addStat<T>(t);
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
