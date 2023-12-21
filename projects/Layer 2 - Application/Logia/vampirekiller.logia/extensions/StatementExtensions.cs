using Logia.vampirekiller.logia;
using Namespace;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;

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
        // Va chercher toutes les zones avant d'appliquer les effets
        var statementActions = container.statements.values.Select(statement =>
        {
            // todo: calcul zone, get toutes les fight.entity.position qui sont dedans (creatures, projectiles, glyphs..)
            // disons qu'on prend juste les creatures pour l'instant
            // pourra être utile d'avoir des CreatureSystem, ProjectileSystem, etc qui regroupent toutes les entités
            //      peuvent être populés automatiquement via Register.Create<>();
            IEnumerable<CreatureInstance> targets = null!;
            var sub = new ActionStatementZone(action)
            {
                statement = statement,
                targets = targets
            };
            return sub;
        });
        // Applique à toute la zone
        foreach (var sub in statementActions)
        {
            sub.applyStatementZone();
        }
    }
    
    /// <summary>
    /// Apply un seul statement à seul target (peut être untargeted, ex: ground-target summon, glyph, projectile...)
    /// </summary>
    public static void apply(this IStatement statement, ActionStatementTarget action)
    {
        var script = statement.getScript();
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
