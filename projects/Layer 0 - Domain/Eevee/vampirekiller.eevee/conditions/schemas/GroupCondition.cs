using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.structures;

namespace VampireKiller.eevee.vampirekiller.eevee.conditions.schemas;

public enum ConditionGroupType
{
    AND,
    OR
}

public class GroupCondition : IConditionSchema
{
    public ConditionGroupType groupType { get; set; } = ConditionGroupType.AND;
    public SmartList<ICondition> children { get; set; } = SmartList<ICondition>.Create();

    public IConditionSchema copy()
    {
        throw new NotImplementedException();
    }
}
