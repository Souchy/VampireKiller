using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.actions;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.conditions.schemas;

namespace Logia.vampirekiller.logia.conditions;

public class SpellConditionScript : IConditionScript
{
    public Type schemaType => typeof(SpellModelCondition);

    // public bool check(ICondition s, Vector3 position, CreatureInstance caster, CreatureInstance currentTarget)
    // {
    //     throw new NotImplementedException();
    // }

    public bool checkCondition(IAction action, ICondition condition)
    {
        throw new NotImplementedException();
    }
}
