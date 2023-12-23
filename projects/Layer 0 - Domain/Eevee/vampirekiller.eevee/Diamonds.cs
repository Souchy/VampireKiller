

using System.Collections.Immutable;
using Util.entity;
using vampirekiller.eevee.statements;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.eevee;

public static class Diamonds
{

    public static ImmutableDictionary<Type, IStatementScript> statementScripts { get; set; }
    public static ImmutableDictionary<Type, ITriggerScript> triggerScripts { get; set; }
    public static ImmutableDictionary<Type, IConditionScript> conditionScripts { get; set; }

    public static Dictionary<ID, SpellModel> spells { get; } = new();

}

public static class DiamondExtensions
{

    public static IStatementScript getScript(this IStatementSchema schema)
    {
        return Diamonds.statementScripts[schema.GetType()];
    }
    public static ITriggerScript getScript(this ITriggerSchema schema)
    {
        return Diamonds.triggerScripts[schema.GetType()];
    }
    public static IConditionScript getScript(this IConditionSchema schema)
    {
        return Diamonds.conditionScripts[schema.GetType()];
    }

}
