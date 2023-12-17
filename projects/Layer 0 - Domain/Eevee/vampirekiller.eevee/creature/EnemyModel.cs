using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.ai;
using VampireKiller.eevee.creature;

namespace vampirekiller.eevee.creature;

public class EnemyModel : CreatureModel
{
    public Ai ai { get; set; }
}
