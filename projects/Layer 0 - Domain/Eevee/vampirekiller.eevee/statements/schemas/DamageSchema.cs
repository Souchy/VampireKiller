using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VampireKiller.eevee.vampirekiller.eevee.statements.schemas;

public class DamageSchema : IStatementSchema
{
    //public ElementType element { get; set; } = ElementType.None;
    public int baseDamage { get; set; } = 0;
    public int percentPenetration { get; set; } = 0;
    public int percentVariance { get; set; } = 0;

    public IStatementSchema copy() => new DamageSchema()
    {
        //element = element,
        baseDamage = baseDamage,
        percentPenetration = percentPenetration,
        percentVariance = percentVariance
    };
}
