using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.structures;
using VampireKiller.eevee.creature;

namespace vampirekiller.eevee.creature;

public class CrowdInstance
{
    public SmartList<CreatureInstance> Instances { get; set; } = SmartList<CreatureInstance>.Create();
}
