using Godot;
using Util.ecs;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.triggers.schemas;

namespace vampirekiller.logia.extensions;

public static class ActionExtensions
{

    public static bool canApplyCast(this ActionCastActive action)
    {
        var skill = action.getActive();

        // TODO: 
        // check spell costs against creature's resources
        // check spell cast conditions
        var result = skill.getModel().castCondition.checkCondition(action);

        return result;
    }

    public static void applyActionCast(this ActionCastActive action) {
        var skill = action.getActive();

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
        // If you have targets in the zone
        if (action.targets?.Count() >= action.statement.zone.minSampleCount) //.Any() == true)
        {
            foreach (var target in action.targets)
            {
                var sub = new ActionStatementTarget(action)
                {
                    currentTargetEntity = target,
                    currentTargetPos = null
                };
                applyStatementTarget(sub);
            }
        }
        else if (action.statement.zone.minSampleCount > 0)
        {
            // do nothing if we require targets but dont have them
            // Ex: ShockNova still casts the FX, but not the damage zone because we dont have an actual target to apply to
        }
        // If your zone has no targets or you have no zone, you still want to cast it.... unless we have a flag that says "requireTargets"
        else
        {
            Entity? targetEntity = action.statement.zone.getTargetActor(action);
            Vector3? targetPos = action.statement.zone.getZoneOrigin(action);
            var sub = new ActionStatementTarget(action)
            {
                // if your zone is based on a raycast, then entity could be null or misleading if you dont have targets in your zone
                currentTargetEntity = targetEntity,
                currentTargetPos = targetPos
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
