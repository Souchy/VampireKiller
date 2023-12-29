
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using VampireKiller.eevee.vampirekiller.eevee.conditions;

namespace vampirekiller.logia.extensions;

public static class ConditionExtensions {

    /// <summary>
    /// <b>ActionCastActive</b> checks the spell's <b>castCondition</b> <br></br>
    /// <b>IActionTrigger</b> checks the <b>TriggerListener</b> conditions <br></br>
    /// <b>ActionStatementTarget</b> checks the <b>statement</b> conditions <para></para>
    /// 
    /// Makes for some weird cases where you dont know who to apply the condition to: <br></br>
    ///     [sourceEntity, raycastEntity, contextCreature, statement's currentTargetEntity] <br></br>
    /// Check TeamFilterScript for how to handle it    
    /// 
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="action">Can be: ActionCastActive, IActionTrigger, ActionStatementTarget</param>
    /// <returns></returns>
    public static bool checkCondition(this ICondition condition, IAction action) {
        // todo check base stuff?
        var script = condition?.schema?.getScript();
        if(script == null)
            return true;
        var result = script.checkCondition(action, condition);
        return result;
    }
    
}
