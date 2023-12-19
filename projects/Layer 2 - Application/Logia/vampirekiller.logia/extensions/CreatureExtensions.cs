

using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.extensions;

public static class CreatureExtensions {
    
    public static T getTotalStat<T>(this CreatureInstance crea) where T : IStat, new()
    {
        var t = new T();
        t.add(crea.model.baseStats);
        t.add(crea.fightStats.dic);
        foreach(var item in crea.inventory.items.values)
            item.addStat<T>(t);
        // TODO getTotalStat: items & statuses
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
    
}
