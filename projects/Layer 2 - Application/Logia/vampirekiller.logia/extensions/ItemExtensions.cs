using VampireKiller.eevee.vampirekiller.eevee.equipment;

namespace vampirekiller.logia.extensions;

public static class ItemExtensions {
    
    public static void addStat<T>(this Item item, T t)
    {
        foreach(var s in item.statements.values)
        {
            // 1. check triggers
            // 2. check conditions
            // 3. check stats
            // 4. check children
        }
    }
}
