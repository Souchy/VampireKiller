using vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.logia.extensions;

public static class ItemExtensions {
    
    public static void addStat<T>(this Item item, T t) where T : IStat, new()
    {
        foreach(var s in item.statements.values)
        {
            // 1. check triggers
            // 2. check conditions
            // 3. check stats
            // 4. check children

            var add = s as AddStatsSchema;
            if (add is null)
                return;

            t.add(add.stats);
        }
    }

    public static void addStat<T>(this Status status, T t) where T : IStat, new()
    {
        foreach (var s in status.statements.values)
        {
            // 1. check triggers
            // 2. check conditions
            // 3. check stats
            // 4. check children

            var add = s as AddStatsSchema;
            if (add is null)
                return;

            t.add(add.stats);
        }
    }

}
