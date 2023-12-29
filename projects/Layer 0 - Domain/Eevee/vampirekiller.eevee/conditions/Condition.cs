using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using static System.Collections.Specialized.BitVector32;

namespace VampireKiller.eevee.vampirekiller.eevee.conditions;

public interface ICondition
{
    public ActorType actorType { get; set; }
    public ConditionComparatorType comparator { get; set; }
    public IConditionSchema schema { get; set; }
    public ICondition copy();
}
public class Condition : ICondition
{
    public ActorType actorType { get; set; } = ActorType.Target;
    public ConditionComparatorType comparator { get; set; } = ConditionComparatorType.EQ;
    public IConditionSchema schema { get; set; }

    public IConditionScript getScript() => schema.getScript();
    public ICondition copy()
    {
        var cond = new Condition(); //Register.Create<Condition>(); // maybe?
        cond.schema = schema.copy();
        return cond;
    }

}
public interface IConditionSchema
{
    public IConditionSchema copy();
}
public interface IConditionScript
{
    public Type schemaType { get; }
    public bool checkCondition(IAction action, ICondition condition);
    //public abstract bool check(IAction action, TriggerEvent trigger, ICreature boardSource, IBoardEntity boardTarget);
    //public bool check(ICondition s, Vector3 position, CreatureInstance caster, CreatureInstance currentTarget);
}

public enum ConditionComparatorType
{
    EQ, // equal
    NE, // not equal
    GT, // greater than
    GE, // greater or equal
    LT, // lesser than
    LE // lesser or equal
}

public static class ConditionComparatorTypeExtentions
{
    public static bool check(this ConditionComparatorType comparator, object fetchedValue, object wantedValue)
    {
        if (fetchedValue == null) return false;
        // can compare EQ, NE between IID, strings, numbers...
        if (comparator == ConditionComparatorType.EQ)
        {
            return fetchedValue == wantedValue;
        }
        if (comparator == ConditionComparatorType.NE)
        {
            return fetchedValue != wantedValue;
        }
        // only numbers can be used with GT, GE, LT, LE
        double fetchedInt = (double) fetchedValue;
        double wantedInt = (double) wantedValue;
        switch (comparator)
        {
            case ConditionComparatorType.GT:
                return fetchedInt > wantedInt;
            case ConditionComparatorType.GE:
                return fetchedInt >= wantedInt;
            case ConditionComparatorType.LT:
                return fetchedInt < wantedInt;
            case ConditionComparatorType.LE:
                return fetchedInt <= wantedInt;
        }
        return false;
    }
}
