using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;

namespace VampireKiller.eevee.vampirekiller.eevee.conditions.schemas;

public class CreatureModelCondition : IConditionSchema
{
    public List<ID> acceptedModelIds { get; set; } = new();
    public List<ID> rejectedModelIds { get; set; } = new();

    public IConditionSchema copy()
    {
        var copy = new CreatureModelCondition();
        foreach(var item in acceptedModelIds)
            copy.acceptedModelIds.Add(item);
        foreach (var item in rejectedModelIds)
            copy.rejectedModelIds.Add(item);
        return copy;
    }
}
