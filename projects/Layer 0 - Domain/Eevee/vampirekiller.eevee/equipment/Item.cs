using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace VampireKiller.eevee.vampirekiller.eevee.equipment;

public class Item : Identifiable, IStatementContainer
{
    public ID entityUid { get; set; }
    public SmartList<IStatement> statements { get; set; } = SmartList<IStatement>.Create();
    public int quantity { get; set; }

    public void addStat<T>(T t)
    private Item() {}
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
