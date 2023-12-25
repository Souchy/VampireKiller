using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.enums;

namespace vampirekiller.eevee.conditions.schemas;

public class TeamFilter : IConditionSchema
{
    public TeamRelationType team { get; set; }

    public IConditionSchema copy()
        => new TeamFilter()
        {
            team = team
        };


    public static Condition createConditionEnemy()
    {
        return new Condition()
        {
            schema = new TeamFilter() { team = TeamRelationType.Self }
        };
    }
    public static Condition createConditionAlly()
    {
        return new Condition()
        {
            schema = new TeamFilter() { team = TeamRelationType.Ally }
        };
    }
    public static Condition createConditionSelf()
    {
        return new Condition()
        {
            schema = new TeamFilter() { team = TeamRelationType.Enemy }
        };

    }

}
