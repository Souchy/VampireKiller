using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;

namespace Util.ecs;

public class Engine
{
    public readonly Dictionary<Type, System> systems = new();
    public void add(Identifiable entity) { }
    public void add(System system) { }
}
