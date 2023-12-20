using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers.schemas;

namespace vampirekiller.logia.extensions;

public static class ActionExtensions
{
    public static void applyActionCast(this ActionCastActive action) {
        var item = action.getItem();
        var trigger = new TriggerEventOnCastActive(action);
        item.procTriggers(action, trigger);
    }

    /// <summary>
    /// Apply statement in new action for each target in the zone
    /// </summary>
    public static void applyStatementZone(this ActionStatementZone action)
    {
        if (action.targets?.Any() == true)
        {
            foreach (var target in action.targets)
            {
                var sub = new ActionStatementTarget(action)
                {
                    currentTarget = target
                };
                applyStatementTarget(sub);
            }
        }
        else
        {
            var sub = new ActionStatementTarget(action)
            {
                currentTarget = action.getRaycastCreature()
            };
            applyStatementTarget(sub);
        }
    }

    /// <summary>
    /// Apply statement
    /// </summary>
    public static void applyStatementTarget(this ActionStatementTarget action)
    {
        action.getParentStatement().apply(action);
    }

}
