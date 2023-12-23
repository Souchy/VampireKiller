

using System.Collections.Immutable;
using Util.entity;
using vampirekiller.eevee.statements;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.util.json;
using VampireKiller.eevee.creature;
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
    public static Dictionary<ID, CreatureModel> creatures { get; } = new();

    static Diamonds()
    {
        Diamonds.load();
    }

    public static void load()
    {
        var pathDb = "C:/Robyn/godot/VampireKiller/projects/Layer 1 - Data/DB/";
        var pathSpells = pathDb + "spells/";
        var pathCreatures = pathDb + "creatures/";
        Diamonds.spells.Clear();
        Diamonds.creatures.Clear();

        string[] spells = Directory.GetFiles(pathSpells);
        foreach (var path in spells)
        {
            var json = File.ReadAllText(path);
            var spell = Json.deserialize<SpellModel>(json);
            Diamonds.spells.Add(spell.entityUid, spell);
        }

        string[] creatures = Directory.GetFiles(pathCreatures);
        foreach (var path in creatures)
        {
            var json = File.ReadAllText(path);
            var crea = Json.deserialize<CreatureModel>(json);
            Diamonds.creatures.Add(crea.entityUid, crea);
        }
    }

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
