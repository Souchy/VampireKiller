using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.zones;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using vampirekiller.eevee.conditions.schemas;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using souchy.celebi.eevee.enums;
using vampirekiller.eevee.util.json;
using VampireKiller.eevee.vampirekiller.eevee;
using vampirekiller.logia.stub;
using vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.triggers.schemas;
using vampirekiller.eevee.enums;
using vampirekiller.logia.extensions;
using vampirekiller.eevee.actions;

namespace vampirekiller.gems.spells;

public class Swipe : TestGemBaseSpellModel
{
    public override string getName()
    {
        return "spell_swipe";
    }

    public override SpellModel generateImpl(SpellModel model)
    {
        var swipeAoe = new Statement()
        {
            zone = new Zone()
            {
                zoneType = ZoneType.circleHalf,
                size = new ZoneSize(2),
            },
            targetFilter = new Condition()
            {
                schema = new TeamFilter()
                {
                    team = TeamRelationType.Enemy
                }
            },
            schema = new DamageSchema()
            {
                baseDamage = 5
            }
        };
        model.statements.add(swipeAoe);
        return model;
    }

    protected override void testAssert(TestGemFight fight)
    {
        // TODO
    }
}