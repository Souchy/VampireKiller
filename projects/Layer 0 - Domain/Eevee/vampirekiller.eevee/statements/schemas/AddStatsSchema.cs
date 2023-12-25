using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.statements.schemas;

public class AddStatsSchema : IStatementSchema
{
    public StatsDic stats = Register.Create<StatsDic>();

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
