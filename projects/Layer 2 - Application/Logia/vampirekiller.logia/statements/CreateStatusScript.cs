using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using vampirekiller.eevee.actions;

namespace vampirekiller.logia.statements;
internal class CreateStatusScript : IStatementScript
{
    public Type schemaType => typeof(CreateStatusSchema);

    public void apply(ActionStatementTarget action)
    {
        // TODO status creation script
    }
}
