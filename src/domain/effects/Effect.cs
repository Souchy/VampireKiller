using System;
using System.Collections.Generic;
using Godot;

namespace VampireKiller;


public record Action(Creature caster, Vector3 positionTarget) { }
public record ActionSpell(Creature caster, Spell spell, Vector3 positionTarget) { }
public record ActionMove(Creature caster, Vector3 positionTarget) { }
public record ActionEffectZone(Creature caster, Vector3 positionTarget, Effect effect, List<Creature> possibleTargets) : Action(caster, positionTarget) { }
public record ActionEffectTarget(Creature caster, Vector3 positionTarget, Effect effect, Creature target) : Action(caster, positionTarget) { }


public abstract class Effect
{

    public List<Effect> children { get; set; } = new();
    public List<Trigger> triggers { get; set; } = new();

    public abstract void apply(ActionEffectTarget action);
}


public class Trigger
{

}


public class Damage
{
    public int damage { get; set; }
    public void apply(ActionEffectTarget action)
    {
        CreatureAttack attack = action.caster.stats.get<CreatureAttack>();
        int dam = damage * attack.value;

        CreatureResourceLife life = action.target.stats.moreFlat.get<CreatureResourceLife>();
        life.value -= dam;

        var totalLife = action.target.stats.get<CreatureResourceLife>();
        if(totalLife.value <= 0) {
            // dead
        }
    }
}
