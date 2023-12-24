using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;

namespace vampirekiller.eevee.statements.schemas;

public class AddStatusSchema : IStatementSchema
{
    public ID statusModelUid { get; set; }
    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
