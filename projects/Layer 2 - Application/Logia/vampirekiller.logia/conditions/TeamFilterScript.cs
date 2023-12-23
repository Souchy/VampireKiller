using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.conditions.schemas;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.conditions.schemas;
using VampireKiller.eevee.vampirekiller.eevee.enums;

namespace Logia.vampirekiller.logia.conditions;

public class TeamFilterScript : IConditionScript
{
    public Type schemaType => typeof(TeamFilter);

    // public bool check(ICondition s, Vector3 position, CreatureInstance caster, CreatureInstance currentTarget)
    // {
    //     throw new NotImplementedException();
    // }

    public bool checkCondition(IAction action, ICondition condition)
    {
        // todo clean this up, hacking shortcuts
        var schema = (TeamFilter)condition.schema;
        var source = (CreatureInstance) action.getSourceEntity();
        if (action is ActionStatementTarget actionStatementTarget)
        {
            var currentTarget = (CreatureInstance) actionStatementTarget.currentTarget;
            if (schema.team == TeamRelationType.Enemy)
            {
                return source.creatureGroup != currentTarget.creatureGroup;
            }
            if (schema.team == TeamRelationType.Ally)
            {
                return source.creatureGroup == currentTarget.creatureGroup;
            }
            if (schema.team == TeamRelationType.Ally)
            {
                return source == currentTarget;
            }
        }
        return false;
    }
}
