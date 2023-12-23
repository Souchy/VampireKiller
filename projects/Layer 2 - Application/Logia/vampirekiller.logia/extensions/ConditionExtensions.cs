
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using VampireKiller.eevee.vampirekiller.eevee.conditions;

namespace vampirekiller.logia.extensions;

public static class ConditionExtensions {

    public static bool checkCondition(this ICondition condition, IAction action) {
        // todo check base stuff?
        var script = condition?.schema?.getScript();
        if(script == null)
            return true;
        var result = script.checkCondition(action, condition);
        return result;
    }
    
}
