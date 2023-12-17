using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.creature;

namespace vampirekiller.eevee.ai;

public interface Ai
{
    public CreatureInstance findTarget();
    public void update(float delta);
}
