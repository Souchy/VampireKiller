using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;

namespace Util.ecs;

public class Family<T> : System
{

    public HashSet<T> entities = new();
    public void onAddEntity(Identifiable t)
    {
        if (t is T)
            entities.Add((T) (object) t);
    }
    public override void update()
    {
    }
    
}
