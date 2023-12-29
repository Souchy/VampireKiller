using Godot;
using Logia.vampirekiller.logia;
using Namespace;
using souchy.celebi.eevee.enums;
using System.Linq;
using Util.ecs;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.util;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using Action = vampirekiller.eevee.actions.Action;

namespace vampirekiller.logia.extensions;

/// <summary>
/// Somewhat equivalent to Mind.cs (?)
/// </summary>
public static class StatementExtensions
{
    /// <summary>
    /// Apply les statements d'un container (Spell, Statement..)
    /// </summary>
    public static void applyStatementContainer(this IStatementContainer container, IAction action) 
    {
        Action a = (Action) action;
        // Test ça en attendant, mais c'est pas exact. desfois on veut la position sur le caster, desfois la position sur le curseur, ou sur le proj..
        //      Ex: shockNova check par rapport au crea caster vs fireballExplosion check par rapport au crea target de la collision

        // Va chercher toutes les zones avant d'appliquer les effets
        List<ActionStatementZone> subs = new();
        foreach (var statement in container.statements.values)
        {
            // todo: calcul zone, get toutes les fight.entity.position qui sont dedans (creatures, projectiles, glyphs..)
            // disons qu'on prend juste les creatures pour l'instant
            // pourra être utile d'avoir des CreatureSystem, ProjectileSystem, etc qui regroupent toutes les entités
            //      peuvent être populés automatiquement via Register.Create<>();

            Vector3? spawnPos = statement.zone.getZoneOrigin(action);
            if (spawnPos == null)
            {
                // Cancel the action
                if (statement.isRequiredForContainer)
                    return;
            }
            else
            if (statement.zone.zoneType != ZoneType.point)
            {
                var radius = statement.zone.size.radius;
                IEnumerable<Entity> targets = action.fight.entities.values.ToList().Where(c =>
                {
                    var getter = c.get<PositionGetter>();
                    if(getter == null) return false;
                    var dist = getter().DistanceTo((Vector3) spawnPos);
                    return dist <= radius;
                });
                var sub = new ActionStatementZone(action)
                {
                    statement = statement,
                    targets = targets
                };
                subs.Add(sub);
            }
            else
            {
                var sub = new ActionStatementZone(action)
                {
                    statement = statement,
                    targets = null
                };
                subs.Add(sub);
            }
        }
        // Applique à toute la zone
        foreach (var sub in subs)
        {
            sub.applyStatementZone();
        }
    }
    
    /// <summary>
    /// Apply un seul statement à seul target (peut être untargeted, ex: ground-target summon, glyph, projectile...)
    /// </summary>
    public static void apply(this IStatement statement, ActionStatementTarget action)
    {
        if(statement.targetFilter != null && !statement.targetFilter.checkCondition(action)) 
            return;
        if(statement.sourceCondition != null && !statement.sourceCondition.checkCondition(action)) 
            return;
        var script = statement.schema.getScript();
        script.apply(action);
        // trigger
        // var trigger = new TriggerEventOnStatement();
        // Universe.fight.procTriggers(action, trigger);
        var triggerAction = new ActionStatementTrigger(action, TriggerType.onStatement);
        Universe.fight.procTriggers(triggerAction);
        // apply children
        statement.applyStatementContainer(action);
    }

    /// <summary>
    /// Ex: ground targeted effect -> createGlyphe, createTrap, throw projectiles, untargeted spells..
    /// </summary>
    /// <param name="action"></param>
    // private static void applyEffectNoZone(ActionEffect action) 
    // {
    // }

}
