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

/// <summary>
/// Items are all permanent passives.
/// Some of them give new skills.
/// </summary>
public class Item : Identifiable, IStatementContainer
{
    public ID entityUid { get; set; }
    public SmartList<IStatement> statements { get; set; } = SmartList<IStatement>.Create();
    public int quantity { get; set; }

    private Item() {}

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
