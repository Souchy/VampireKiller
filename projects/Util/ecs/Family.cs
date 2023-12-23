using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using Util.structures;

namespace Util.ecs;

public class Family<T> : SmartSet<T> //System
{
    // public HashSet<T> entities = new();

    [Subscribe(nameof(Register.RegisterEventBus))]
    public void onAddEntity(Identifiable t)
    {
        if (t is T)
            list.Add((T) (object) t);
    }

    // public override void update()
    // {
    // }
    
}
