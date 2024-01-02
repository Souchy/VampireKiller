using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace VampireKiller.eevee.vampirekiller.eevee.statements.schemas;

public class CreateStatusSchema : IStatementSchema
{
    public int mergeStrategy { get; set; }
    public List<IStatement> statusStatements { get; set; } = new();
    /// <summary>
    /// Contains Status stats like: base duration, max duration, max stacks, unbewitchable...
    /// </summary>
    public StatsDic stats { get; set; } = Register.Create<StatsDic>();

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
