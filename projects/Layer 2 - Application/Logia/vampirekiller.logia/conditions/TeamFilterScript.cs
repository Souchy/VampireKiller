using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.ecs;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.conditions.schemas;
using vampirekiller.eevee.enums;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.conditions.schemas;
using VampireKiller.eevee.vampirekiller.eevee.enums;

namespace Logia.vampirekiller.logia.conditions;

public class TeamFilterScript : IConditionScript
{
    public Type schemaType => typeof(TeamFilter);

    public bool checkCondition(IAction action, ICondition condition)
    {
        // todo clean this up, hacking shortcuts
        var schema = (TeamFilter)condition.schema;
        Entity source = action.getSourceEntity();
        Entity? target = getTarget(action);
        if (target == null) {
            return false;
        }
        if (schema.team == TeamRelationType.Enemy)
        {
            return source.get<Team>() != target.get<Team>();
        }
        if (schema.team == TeamRelationType.Ally)
        {
            return source.get<Team>() == target.get<Team>();
        }
        if (schema.team == TeamRelationType.Self)
        {
            return source == target;
        }
        return false;
    }

    private Entity? getTarget(IAction action)
    {
        Entity? target = action.getRaycastEntity();
        if (action is ActionCollision actionCollision)
        {
            target = action.getRaycastEntity();
        }
        else
        if (action is ActionStatementTarget actionStatementTarget)
        {
            target = actionStatementTarget.currentTargetEntity;
        }
        else
        if (action is IActionTrigger actionTrigger)
        {
            target = actionTrigger.getContextCreature();
        }
        return target;
    }
}
