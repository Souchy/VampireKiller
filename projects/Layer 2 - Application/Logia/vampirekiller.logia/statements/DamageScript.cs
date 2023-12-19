using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.actions;
using vampirekiller.logia.extensions;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.statements;

public class DamageScript : IStatementScript
{
    public Type schemaType => typeof(DamageSchema);
    
    public void apply(ActionStatementTarget action)
    {
        var currentTarget = action.currentTarget; //.getRaycastCreature();
        var statement = action.getParentStatement();

        var totalLife = currentTarget.getTotalStat<CreatureTotalLife>();
        currentTarget.getTotalStat<CreatureTotalLifeMax>();

        DamageSchema dmg = statement.GetProperties<DamageSchema>();
        int dam = -dmg.baseDamage;

        // apply variance
        int variance = new Random().Next(-dmg.percentVariance, dmg.percentVariance);
        dam = (int) (dam * variance / 100.0);

        // apply affinities
        var casterAttack = 0; //caster.getTotalStat<Attack>();
        dam = (int) (dam * (1 + casterAttack / 100.0));

        // apply resistances
        var targetRes = 0; //currentTarget.getTotalStat<Resistance>();
        dam = (int) (dam * (1 - targetRes / 100.0));

        // update life, which will send an event
        currentTarget.fightStats.addedLife.value += dam;
    }

    //public IEffectReturnValue apply(ISubActionEffectTarget action, IBoardEntity currentTarget, IEnumerable<IBoardEntity> allTargetsInZone)
    // public void apply(IStatement s, Vector3 position, CreatureInstance caster, CreatureInstance currentTarget, IEnumerable<CreatureInstance> allTargetsInZone)
    // {
    //     var totalLife = currentTarget.getTotalStat<CreatureTotalLife>();
    //     currentTarget.getTotalStat<CreatureTotalLifeMax>();

    //     DamageSchema dmg = s.GetProperties<DamageSchema>();
    //     int dam = -dmg.baseDamage;

    //     // apply variance
    //     int variance = new Random().Next(-dmg.percentVariance, dmg.percentVariance);
    //     dam = (int) (dam * variance / 100.0);

    //     // apply affinities
    //     var casterAttack = 0; //caster.getTotalStat<Attack>();
    //     dam = (int) (dam * (1 + casterAttack / 100.0));

    //     // apply resistances
    //     var targetRes = 0; //currentTarget.getTotalStat<Resistance>();
    //     dam = (int) (dam * (1 - targetRes / 100.0));

    //     // update life, which will send an event
    //     currentTarget.fightStats.addedLife.value += dam;
    // }

}
