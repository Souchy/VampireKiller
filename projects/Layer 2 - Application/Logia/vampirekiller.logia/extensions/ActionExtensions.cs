using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers.schemas;

namespace vampirekiller.logia.extensions;

public static class ActionExtensions
{
    public static void applyActionCast(this ActionCastActive action) {
        var skill = action.getActive();

        // TODO: 
        // check spell costs against creature's resources
        // check spell cast conditions
        skill.getModel().castCondition.checkCondition(action);

        // TODO: update les ressources du player, update le nombre de charges dans le spellinstance, update le cooldown, 
        // l'action devrait déjà contenir le raycast mousetarget depuis la root action
        // apply
        skill.applyStatementContainer(action);
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
        // Si on n'a pas de zone
        else
        {
            var sub = new ActionStatementTarget(action)
            {
                currentTarget = action.getTargetEntity() //.getRaycastCreature()
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
