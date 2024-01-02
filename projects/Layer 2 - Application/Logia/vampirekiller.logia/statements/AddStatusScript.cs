using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.actions;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee;
using Util.entity;

namespace vampirekiller.logia.statements;
internal class AddStatusScript : IStatementScript
{
    public Type schemaType => typeof(AddStatusSchema);

    public void apply(ActionStatementTarget action)
    {
        AddStatusSchema schema = action.statement.GetProperties<AddStatusSchema>();
        Status model = Diamonds.statusModels[schema.statusModelUid];

        Status status = Register.Create<Status>();
        status.modelUid = model.entityUid;
        // .....


    }
}
