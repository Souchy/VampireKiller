

using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.logia.extensions;

public static class TriggerExtensions {
    
    /// <summary>
    /// Check triggers on the whole fight
    /// </summary>
    public static void procTriggers(this Fight fight, IAction action, TriggerEvent trigger) {
        foreach(var creature in fight.creatures.values) {
            creature.procTriggers(action, trigger);
        }
        foreach(var projectile in fight.projectiles.values) {
            projectile.procTriggers(action, trigger);
        }
    }

    /// <summary>
    /// Check triggers on a creature
    /// </summary>
    public static void procTriggers(this CreatureInstance crea, IAction action, TriggerEvent trigger) {
        foreach(var item in crea.inventory.items.values) {
            item.procTriggers(action, trigger);
        }
        foreach(var status in crea.statuses.values) {
            status.procTriggers(action, trigger);
        }
    }

    /// <summary>
    /// Check triggers on a StatementContainer (status, item, projectile..)
    /// </summary>
    public static void procTriggers(this IStatementContainer container, IAction action, TriggerEvent trigger) {
        foreach(var statement in container.statements.values) {
            statement.procTrigger(action, trigger);
        }
    }    

    /// <summary>
    /// 
    /// </summary>
    public static bool checkTrigger(this TriggerListener listener, IAction action, TriggerEvent trigger)
    {
        // listener.holderCondition..check(action, liste);
        var checkHolder = listener.holderCondition?.checkCondition(action);
        if (checkHolder == false)
            return false;
        var checkTriggerer = listener.triggererCondition?.checkCondition(action);
        if (checkTriggerer == false)
            return false;
        // todo check zone
        // check orderType si on garde cette mécanique
        var script = listener.getScript();
        var checkScript = script.checkTrigger(action, trigger, listener.schema); // (action, listener); //...schema? statement? listener?
        return checkScript;
    }

    public static void procTrigger(this IStatement statement, IAction action, TriggerEvent trigger) {
        foreach(var listener in statement.triggers.values) {
            if (listener.checkTrigger(action, trigger))
                continue;
            
            var sub = new ActionStatementTarget(action);
            statement.apply(sub);
        }
    }

    // public bool checkTrigger(IAction action, TriggerEvent triggerEvent)
    //     var caster = action.fight.creatures.Get(action.caster);
    //     var targetCell = action.fight.GetBoardEntity(action.targetCell);

    //     if (HolderCondition != null && !HolderCondition.check(action, triggerEvent, caster, targetCell))
    //         return false;
    //     if (triggererFilter != null && !triggererFilter.check(action, triggerEvent, caster, targetCell))
    //         return false;

    //     if (triggerZone != null)
    //     {
    //         var area = triggerZone.getArea(action.fight, targetCell.position);
    //         var isCasterInArea = area.Cells.Any(c => c.position == caster.position);
    //         if (!isCasterInArea)
    //             return false;
    //     }

    //     var isRightType = this.schema.triggerType == triggerEvent.type && this.triggerOrderType == triggerEvent.orderType;
    //     if (!isRightType)
    //         return false;
    //     return this.schema.checkTrigger(action, triggerEvent);

}
