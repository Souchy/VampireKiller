using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.ecs;
using Util.entity;
using Util.structures;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.creature;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.util;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace VampireKiller.eevee.creature;


public class CreatureInstance : Entity, Identifiable
{
    public const string EventUpdateStats = "creature.stats.changed";

    public long playerId { get; set; }
    public CreatureModel model { get; set; }
    public EntityGroupType creatureGroup { get => get<EntityGroupType>(); set => set<EntityGroupType>(value); }

    public Vector3 spawnPosition { get; set; }
    public Vector3 position
    {
        get
        {
            var getter = get<PositionGetter>();
            //if (getter == null) return null;
            return getter();
        }
        // set => setPositionHook(value); }
    } 

    /// <summary>
    /// Fight stats like added life (-damage + heal...),  max added life (erosion...), etc
    /// </summary>
    public CreatureFightStats fightStats { get; set; } = new();
    /// <summary>
    /// 
    /// </summary>
    public SmartList<Status> statuses { get; set; } = SmartList<Status>.Create();
    /// <summary>
    /// Items are only passive.
    /// Some items can teach new skills. The skills go into the allSkill list, then you can move those into activeSkills.
    /// </summary>
    public SmartList<Item> items { get; set; } = SmartList<Item>.Create();
    /// <summary>
    /// SpellInstances can be learned from items or naturally
    /// </summary>
    public SmartList<SpellInstance> allSkills { get; set; } = SmartList<SpellInstance>.Create();
    /// <summary>
    /// Maximum of 4 active skills at a time
    /// </summary>
    public SmartList<SpellInstance> activeSkills { get; set; } = SmartList<SpellInstance>.Create();
    /// <summary>
    /// Current skin can be chosen at the start of the game (MTX), or modified through the game (transformations)
    /// </summary>
    public CreatureSkin currentSkin { get; set; }

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
