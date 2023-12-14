using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.conditions;

namespace Logia.vampirekiller.logia.conditions;

public class SpellFilterScript : IConditionScript
{
    public bool check(ICondition s, Vector3 position, CreatureInstance caster, CreatureInstance currentTarget)
    {
        throw new NotImplementedException();
    }
}
