

using System.Collections.Immutable;
using Util.entity;
using Util.json;
using Util.structures;
using vampirekiller.eevee.campaign.map;
using vampirekiller.eevee.statements;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.util.json;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.eevee;

public static class Diamonds
{

    public static ImmutableDictionary<Type, IStatementScript> statementScripts { get; set; }
    public static ImmutableDictionary<Type, ITriggerScript> triggerScripts { get; set; }
    public static ImmutableDictionary<Type, IConditionScript> conditionScripts { get; set; }

    public static Dictionary<ID, SpellModel> spellModels { get; } = new();
    public static Dictionary<ID, CreatureModel> creatureModels { get; } = new();
    public static Dictionary<ID, Status> statusModels { get; } = new();
    public static Dictionary<ID, Biome> biomes { get; } = new();
    public static Dictionary<ID, Item> items { get; } = new();
    //public static Dictionary<ID, SpellInstance> spells { get; } = new();

    static Diamonds()
    {
        Diamonds.load();
    }

    public static void load()
    {
        const string pathDb = "../../Layer 1 - Data/DB/";
        const string pathSpells = pathDb + "spells/";
        const string pathCreatures = pathDb + "creatures/";
        const string pathStatuses = pathDb + "status/";
        const string pathBiomes = pathDb + "biomes/";
        Diamonds.spellModels.Clear();
        Diamonds.creatureModels.Clear();

        string[] spells = Directory.GetFiles(pathSpells);
        foreach (var path in spells)
        {
            var json = File.ReadAllText(path);
            var spell = Json.deserialize<SpellModel>(json);
            Diamonds.spellModels.Add(spell.entityUid, spell);
        }

        string[] creatures = Directory.GetFiles(pathCreatures);
        foreach (var path in creatures)
        {
            var json = File.ReadAllText(path);
            var crea = Json.deserialize<CreatureModel>(json);
            Diamonds.creatureModels.Add(crea.entityUid, crea);
        }

        string[] statuses = Directory.GetFiles(pathStatuses);
        foreach (var path in statuses)
        {
            var json = File.ReadAllText(path);
            var status = Json.deserialize<Status>(json);
            Diamonds.statusModels.Add(status.entityUid, status);
        }

        string[] biomes = Directory.GetFiles(pathBiomes);
        foreach (var path in biomes)
        {
            var json = File.ReadAllText(path);
            var biome = Json.deserialize<Biome>(json);
            Diamonds.biomes.Add(biome.entityUid, biome);
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
