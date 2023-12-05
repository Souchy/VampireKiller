using System;
using System.Collections.Generic;
using Godot;

namespace VampireKiller;


public record Action(CreatureInstance caster, Vector3 positionTarget) { }
public record ActionSpell(CreatureInstance caster, SpellModel spell, Vector3 positionTarget) { }
public record ActionMove(CreatureInstance caster, Vector3 positionTarget) { }
public record ActionEffectZone(CreatureInstance caster, Vector3 positionTarget, Statement effect, List<CreatureInstance> possibleTargets) : Action(caster, positionTarget) { }
public record ActionEffectTarget(CreatureInstance caster, Vector3 positionTarget, Statement statement, CreatureInstance target) : Action(caster, positionTarget) { }


public abstract class Statement
{
    // public Vector3 aoeSize = new();
    public List<Statement> children { get; set; } = new();
    public List<Trigger> triggers { get; set; } = new();

    public abstract void apply(ActionEffectTarget action);
}

public enum Trigger
{
    OnReady,
    OnProcess,
    OnHit,
    OnExpire,
    // OnDeath, ?
}

public class StatementAddStats : Statement
{
    public Stats bonusStats { get; set; }

    public override void apply(ActionEffectTarget action)
    {
        GD.Load<object>("");
    }
}

// public class Projectile : Effect
// {
//     public override void apply(ActionEffectTarget action)
//     {
//         throw new NotImplementedException();
//     }
// }

public class Damage : Statement
{
    public int damage { get; set; }
    public override void apply(ActionEffectTarget action)
    {
        var totalStats = action.caster.getTotalStats();
        // var totalLife = totalStats.get<CreatureResourceLife>();
        CreaturelAttack attack = totalStats.get<CreaturelAttack>();
        int dam = damage * attack.value;

        var life = action.target.resources.GetLife();
        life.value -= dam;
        // CreatureResourceLife life = action.target.stats.fight.get<CreatureResourceLife>();
        // life.value -= dam;

        if (life.value <= 0) { //totalLife.value - dam <= 0) {
            // dead
        }
    }
}

