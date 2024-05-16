using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.ecs;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.creature;

namespace vampirekiller.eevee.creature;

public class CrowdInstance : Entity, Identifiable
{
    public SmartSet<CreatureInstance> Instances { get; set; } = SmartSet<CreatureInstance>.Create();
    private CrowdInstance() { }
}
