
using System;
using System.Collections.Generic;

namespace aaaaaaaa;

static class ECS {
    public static Dictionary<ObjectId, Dictionary<Type, object>> entities = new();
    public static void add(this Entity entity, object component)
    {
        if (!entities.ContainsKey(entity.entityUid))
            entities[entity.entityUid] = new();
        entities[entity.entityUid].Add(component.GetType(), component);
    }
}
public class ObjectId { }
public interface Entity {
    public static ObjectId RegisterObjectId() {
        var id = new ObjectId(); // ObjectId.GenerateNewId();
        id.RegisterEventBus();
        return id;
    }
    public ObjectId entityUid { get; set; }
    // private readonly Dictionary<Type, object> components = new();
}
abstract class System {
    public abstract void update();
}
class Family<T> : System {
    public HashSet<T> entities = new();
    public void onAddEntity(Entity t) {
        if(t is T)
            entities.Add((T) (object) t);
    }
    public override void update()
    {
    }
}
class Engine {
	public readonly Dictionary<Type, System> systems = new();
    public void add(Entity entity) {}
    public void add(System system) {}
}
