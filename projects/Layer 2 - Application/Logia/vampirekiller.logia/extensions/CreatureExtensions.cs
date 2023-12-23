

using Util.entity;
using vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.extensions;

public static class CreatureExtensions
{
    /// <summary>
    /// Calculates the sum of the requested stat through all baseStats + fightStats + items + statuses
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="crea"></param>
    /// <returns></returns>
    public static T getTotalStat<T>(this CreatureInstance crea, StatsDic additional = null) where T : IStat, new()
    {
        var t = new T();
        t.add(crea.model.baseStats);
        t.add(crea.fightStats.dic);
        foreach (var item in crea.inventory.items.values)
            item.addStat<T>(t);
        // TODO getTotalStat: items & statuses
        // foreach (var status in crea.statuses.values)
        //     status.addStat<T>(t);
        if(additional != null) {
            t.add(additional);
        }
        return t;
    }

    /// <summary>
    /// TDOO utiliser un StatementHealScript qui proccerait les triggers etc
    /// </summary>
    /// <param name="crea"></param>
    public static void applyRegen(this CreatureInstance crea)
    {
        int regen = 0; //var regen = crea.getTotalStat<CreatureLifeRegen>();
        var life = crea.getTotalStat<CreatureTotalLife>();
        var lifeMax = crea.getTotalStat<CreatureTotalLifeMax>();
        life.value = Math.Clamp(life.value + regen, 0, lifeMax.value);
    }

    /// <summary>
    /// Add an item to a creature's inventory and teach him all the spells in it
    /// </summary>
    public static void equip(this CreatureInstance crea, Item item)
    {
        // equip item
        crea.inventory.items.add(item);
        // learn spells
        var spellBooks = item.statements.values
            .Where(s => s.schema is CastSpellSchema)
            .Select(s => s.schema as CastSpellSchema);
        foreach(var book in spellBooks) {
            var spell = Register.Create<SpellInstance>();
            spell.modelUid = book.spellModelId;
            crea.allSkills.add(spell);
        }
    }

    /// <summary>
    /// Remove an item from a creature's inventory and unteach him all the spells in it
    /// </summary>
    public static void unequip(this CreatureInstance crea, Item item)
    {
        // equip item
        crea.inventory.items.remove(item);
        crea.inventory.activeSlots.remove(item);
        // unlearn spells
        var spellBooks = item.statements.values
            .Where(s => s.schema is CastSpellSchema)
            .Select(s => s.schema as CastSpellSchema);
        
        foreach(var book in spellBooks) {
            var spell = crea.allSkills.values.FirstOrDefault(s => s.modelUid == book.spellModelId);
            crea.allSkills.remove(spell);
            crea.activeSkills.remove(spell);
        }
    }


}
