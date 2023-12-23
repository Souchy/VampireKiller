using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers;
using vampirekiller.logia.extensions;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.statements;

public class DamageScript : IStatementScript
{
    public Type schemaType => typeof(DamageSchema);
    
    /// <summary>
    /// Petit problème à méditer: les entity source/target peuvent être des projectile (ex proj attack crea ou crea attack proj)
    /// Donc, sachant que les proj ont des stats aussi,
    /// On peut mettre les stats en component de Entity (.set<StatsDic>(stats), .get<StatsDic>())
    /// Mais l'extension getTotalStat est actuellement seulement sur CreatureInstance.
    /// Pourrait rajouter une extension .getTotalStat pour les projectiles aussi qui prendrait le total des stats de proj + stats de son propriétaire
    /// </summary>
    public void apply(ActionStatementTarget action)
    {
        var currentTarget = action.currentTarget as CreatureInstance;
        // var caster = action.getSourceEntity(); // as CreatureInstance;
        if(currentTarget == null)
            return;
        var statement = action.getParentStatement();

        var totalCurrentLife = currentTarget.getTotalStat<CreatureTotalLife>();
        var totalMaxLife = currentTarget.getTotalStat<CreatureTotalLifeMax>();

        DamageSchema dmg = statement.GetProperties<DamageSchema>();
        int dam = -dmg.baseDamage;

        // apply variance
        int variance = new Random().Next(-dmg.percentVariance, dmg.percentVariance);
        dam = (int) (dam * (1 + variance / 100.0));

        // apply affinities
        // var casterStats = caster.get<StatsDic>();
        var casterAttack = 0; //caster.getTotalStat<Attack>();
        dam = (int) (dam * (1 + casterAttack / 100.0));

        // apply resistances
        var targetRes = 0; //currentTarget.getTotalStat<Resistance>();
        dam = (int) (dam * (1 - targetRes / 100.0));

        // update life, which will send an event
        var addLife = Math.Clamp(dam, -totalCurrentLife.value, totalMaxLife.value - totalCurrentLife.value);
        currentTarget.fightStats.addedLife.value += addLife; //+= dam;

        if(addLife == -totalCurrentLife.value) {
            // TODO actionTrigger apply
            // var actionTrigger = new ActionStatementTrigger(action, TriggerType.onDeath);
            // actionTrigger.apply(); 
            currentTarget.GetEntityBus().publish("death", currentTarget);
            action.fight.creatures.remove(currentTarget);
            action.fight.entities.remove(currentTarget);
            currentTarget.Dispose();
        }
    }

}
