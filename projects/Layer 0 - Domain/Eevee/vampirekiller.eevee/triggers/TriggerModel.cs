using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.zones;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using Util.entity;

namespace VampireKiller.eevee.vampirekiller.eevee.triggers;

public interface ITriggerModel : Identifiable //: IEntity
{
    /// <summary>
    /// Holds the TriggerType and trigger properties specific to each schema type
    /// </summary>
    public ITriggerSchema schema { get; set; }

    /// <summary>
    /// { When to react to something }
    /// Wether to insert the triggered effects before or after the cause
    /// </summary>
    public TriggerOrderType triggerOrderType { get; set; }

    /// <summary>
    /// { Where is the thing we can react to }
    /// Trigger zone, only creatures in that zone can activate the trigger (may not need this if we use glyphs)
    /// </summary>
    public IZone triggerZone { get; set; }

    /// <summary>
    /// { Who we can react to }
    /// Additionaly Filter what kind of creature can proc this trigger (observation subject), ex: breed is a demon
    /// </summary>
    public ICondition triggererFilter { get; set; }

    /// <summary>
    /// { How the holder is }
    /// Additionally Conditions on the holder of the trigger buff, ex: hp higher than 50
    /// </summary>
    public ICondition HolderCondition { get; set; }

    //public bool checkTrigger(IAction action, TriggerEvent triggerEvent);

    public ITriggerModel copy();
}
public class TriggerModel : ITriggerModel
{
    public ID entityUid { get; set; }

    /// <summary>
    /// Trigger Data
    /// </summary>
    public ITriggerSchema schema { get; set; }
    public TriggerOrderType triggerOrderType { get; set; } = TriggerOrderType.After;
    public IZone triggerZone { get; set; }  // only targets in the zone can trigger the TriggerModel
    public ICondition triggererFilter { get; set; } // who can trigger the triggerModel
    public ICondition HolderCondition { get; set; } // if the holder can be triggered

    //private TriggerModel() { }
    //public static ITriggerModel Create() => new TriggerModel()
    //{
    //    entityUid = Eevee.RegisterIIDTemporary()
    //};

    public ITriggerModel copy()
    {
        var copy = Register.Create<TriggerModel>(); // TriggerModel.Create();
        copy.triggerOrderType = triggerOrderType;
        copy.triggerZone = triggerZone.copy();
        copy.triggererFilter = triggererFilter.copy();
        copy.HolderCondition = HolderCondition.copy();
        copy.schema = schema.copy();
        return copy;
    }

    public bool checkTrigger()
    {
        return false;
    }
    //public bool checkTrigger(IAction action, TriggerEvent triggerEvent)
    //{
    //    var caster = action.fight.creatures.Get(action.caster);
    //    var targetCell = action.fight.GetBoardEntity(action.targetCell);

    //    if (HolderCondition != null && !HolderCondition.check(action, triggerEvent, caster, targetCell))
    //        return false;
    //    if (triggererFilter != null && !triggererFilter.check(action, triggerEvent, caster, targetCell))
    //        return false;

    //    if (triggerZone != null)
    //    {
    //        var area = triggerZone.getArea(action.fight, targetCell.position);
    //        var isCasterInArea = area.Cells.Any(c => c.position == caster.position);
    //        if (!isCasterInArea)
    //            return false;
    //    }

    //    var isRightType = this.schema.triggerType == triggerEvent.type && this.triggerOrderType == triggerEvent.orderType;
    //    if (!isRightType)
    //        return false;


    //    return this.schema.checkTrigger(action, triggerEvent);
    //}

    public void Dispose()
    {
    }

}

//public record TriggerEvent(
//    TriggerType type, // what you react to (spellcast, passturn, ...)
//    TriggerOrderType orderType // when you react to it (before, after)
//                               //IAction action //IEntityModeled entity = null // could be Effect, Spell, ... (OnEffectX, OnCastX...)
//);


public interface ITriggerSchema
{
    public bool checkTrigger(); // public bool checkTrigger(IAction action, TriggerEvent triggerEvent);
    public ITriggerSchema copy();
}

public abstract class TriggerSchema : ITriggerSchema
{
    public TriggerSchema()
    {
    }

    public abstract bool checkTrigger();
    //public abstract bool checkTrigger(IAction action, TriggerEvent triggerEvent);

    public abstract ITriggerSchema copy();
}
