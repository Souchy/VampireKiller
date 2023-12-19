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
    public const string EventUpdateStats = "creature.stats.changed";

    // public ID entityUid { get; set; }
    public CreatureModel model { get; set; }
    public EntityGroupType creatureGroup { get; set; }

    public Vector3 spawnPosition { get; set; }
    public Vector3 position { get => getPositionHook();  set => setPositionHook(value); }
    public Func<Vector3> getPositionHook { get; set; }
    public Action<Vector3> setPositionHook { get; set; }

    public CreatureFightStats fightStats = new();
     public Inventory inventory { get; set; } = new();
    // public CreatureInstanceStats resources { get; set; } = new();
    // public List<StatusInstance> statuses { get; set; } = new();

    private CreatureInstance() { }

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
    /// Celui là c'est juste quand tu ajoute/enlève un objet de Stat au complet
    /// </summary>
    // [Subscribe(StatsDic.EventUpdate)]
    // private void onStatsChanged(StatsDic dic)
    // {
    //    this.GetEntityBus().publish(EventUpdateStats, this, dic);
    // }
    /// <summary>
    /// Bubble up les events de stats jusqu'au UI
    /// Celui là c'est quand la valeur d'une stat change
    /// </summary>
    [Subscribe(StatsDic.EventUpdate)]
    private void onStatChanged(IStat stat)
    {
    //    GD.Print("CreatureInstance: onStatChanged: " + stat);
       this.GetEntityBus().publish(EventUpdateStats, this, stat);
    }

    // [Subscribe(IStat.EventSet)]
    // private void onStatChanged(IStat stat)
    // {
    //     this.GetEntityBus().publish(IStat.EventSet, this, stat);
    // }

}
