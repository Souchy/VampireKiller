using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;

namespace Util.communication.events;
public static class CentralBus
{
    private static Dictionary<ID, EventBus> eventBuses = new();
    public static readonly EventBus centralBus = new();

    /// <summary>
    /// Creates an EventBus for the Entity using its ID. <br></br>
    /// If the Entity doesn't have an ID yet, it is generated automatically.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool RegisterEventBus(this IEntity entity)
    {
        if(entity.entityUid == null)
            entity.entityUid = IDGenerator.Instance.Generate();
        var result = RegisterEventBus(entity.entityUid);
        centralBus.publish(nameof(RegisterEventBus), entity);
        return result;
    }
    public static bool DisposeEventBus(this IEntity entity)
    {
        var result = DisposeEventBus(entity.entityUid);
        centralBus.publish(nameof(DisposeEventBus), entity);
        return result;
    }
    public static EventBus GetEntityBus(this IEntity entity) => GetEntityBus(entity.entityUid);
    public static bool RegisterEventBus(this ID id)
    {
        lock (eventBuses)
        {
            if (eventBuses.ContainsKey(id))
                throw new ArgumentException("Id already exists");
            var eventBus = new EventBus();
            centralBus.publish(nameof(RegisterEventBus), id, eventBus);
            eventBuses.Add(id, eventBus);
        }
        return true;
    }
    public static bool DisposeEventBus(this ID id)
    {
        lock (eventBuses)
        {
            if (!eventBuses.ContainsKey(id))
                return false;
            var eventBus = eventBuses[id];
            eventBuses[id].Dispose();
            eventBuses.Remove(id);
            centralBus.publish(nameof(DisposeEventBus), id, eventBus);
        }
        return true;
    }
    public static EventBus GetEntityBus(this ID id)
    {
        lock (eventBuses)
        {
            if (eventBuses.ContainsKey(id))
                return eventBuses[id];
        }
        return null; // when NewtonsoftJson deserializes objects, it sets properties which calls the event bus before the entities' id are registered
                     //throw new Exception("You made a mistake in type or method called. Maybe call iid.GetEventBus<T>()");
    }
}