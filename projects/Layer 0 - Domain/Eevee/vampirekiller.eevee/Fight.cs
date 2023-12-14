using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.creature;

namespace VampireKiller.eevee.vampirekiller.eevee;
public class Fight
{
    
    public SmartSet<CreatureInstance> creatures { get; init; } = SmartSet<CreatureInstance>.Create();
    public SmartSet<Projectile> projectiles { get; init; } = SmartSet<Projectile>.Create();


}
