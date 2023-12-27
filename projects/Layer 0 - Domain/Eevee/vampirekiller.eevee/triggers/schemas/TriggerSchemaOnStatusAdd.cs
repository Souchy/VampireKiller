using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.eevee.triggers.schemas;

public class TriggerSchemaOnStatusAdd : ITriggerSchema
{
    public TriggerType triggerType => TriggerType.onStatusAdd;

    public ID spellModelIdFilter { get; set; }
    //public IStatement creatorStatement { get; set; }

    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}