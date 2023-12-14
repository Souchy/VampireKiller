using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace VampireKiller.eevee.vampirekiller.eevee.equipment;

public class Item
{
    public int quantity { get; set; }
    public SmartList<IStatement> statements = SmartList<IStatement>.Create();

    public void addStat<T>(T t)
    {
        foreach(var s in statements.values)
        {
            // 1. check triggers
            // 2. check conditions
            // 3. check stats
            // 4. check children
        }
    }
}
