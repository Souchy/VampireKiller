using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.creature;
using vampirekiller.eevee.enums;
using VampireKiller.eevee.creature;

namespace vampirekiller.eevee.ai;

public class AiMelee : Ai
{
    public CreatureInstance findTarget()
    {
        return Universe.fight.creatures.values.First(c => c.creatureGroup == CreatureGroupType.Players);
    }

    public void update(float delta)
    {

    }

}
