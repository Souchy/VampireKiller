using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;

namespace Util.entity;

/// <summary>
/// Might be nice to add list of entities (à la ECS Engine/System): <code>public Dictionary<ID, Identifiable> entities { get; set; }</code>
/// </summary>
public interface IRegister
{
    public Dictionary<ID, IEventBus> eventBuses { get; set; }
    public IEventBus GetEntityBus(Identifiable entity);
    public IEventBus GetEntityBus(ID id);
    public bool RegisterEventBus(Identifiable entity);
    public bool RegisterEventBus(ID id);
    public bool DisposeEventBus(Identifiable entity);
    public bool DisposeEventBus(ID id);
    public T CreateEntity<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>() where T : Identifiable; //, new();
}

public class Register : IRegister
{
    public static Register instance = new Register();
    public static T Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>() where T : Identifiable //, new()
    {
        return instance.CreateEntity<T>();
    }

    public T CreateEntity<T>() where T : Identifiable //, new()
    {
        T t = (T)Activator.CreateInstance(typeof(T), true)!;
        // T t = new T();
        t.RegisterEventBus();
        t.initialize();
        return t;
    }

    //public static ID RegisterID()
    //{
    //    var id = IDGenerator.Instance.Generate();
    //    id.RegisterEventBus();
    //    return id;
    //}

    public Dictionary<ID, IEventBus> eventBuses { get; set; } = new();

    public IEventBus GetEntityBus(Identifiable entity)
        => GetEntityBus(entity.entityUid);
    public IEventBus GetEntityBus(ID id)
    {
        lock (eventBuses)
        {
            if (eventBuses.ContainsKey(id))
                return eventBuses[id];
        }
        return null; // when NewtonsoftJson deserializes objects, it sets properties which calls the event bus before the entities' id are registered
                     //throw new Exception("You made a mistake in type or method called. Maybe call iid.GetEventBus<T>()");
    }
    public bool RegisterEventBus(Identifiable entity)
    {
        if (entity.entityUid == null)
            entity.entityUid = IDGenerator.instance.Generate();
        if(eventBuses.ContainsKey(entity.entityUid))
            return false;
        var result = RegisterEventBus(entity.entityUid);
        if (result)
            EventBus.centralBus.publish(nameof(RegisterEventBus), entity);
        return result;
    }
    public bool RegisterEventBus(ID id)
    {
        lock (eventBuses)
        {
            if (eventBuses.ContainsKey(id)) {
                // throw new ArgumentException("Id already exists");
                Console.WriteLine("Id already exists: " + id);
                return false;
            }
            IEventBus eventBus = EventBus.factory();
            EventBus.centralBus.publish(nameof(RegisterEventBus), id, eventBus);
            eventBuses.Add(id, eventBus);
        }
        return true;
    }
    public bool DisposeEventBus(Identifiable entity)
    {
        var result = DisposeEventBus(entity.entityUid);
        if (result)
            EventBus.centralBus.publish(nameof(DisposeEventBus), entity);
        return result;
    }
    public bool DisposeEventBus(ID id)
    {
        lock (eventBuses)
        {
            if (!eventBuses.ContainsKey(id))
                return false;
            var eventBus = eventBuses[id];
            var result = eventBuses.Remove(id);
            if (result)
            {
                eventBus.Dispose();
                EventBus.centralBus.publish(nameof(DisposeEventBus), id, eventBus);
            }
            return result;
        }
    }
}

public static class RegisterExtensions
{
    public static IEventBus GetEntityBus(this Identifiable entity)
        => GetEntityBus(entity.entityUid);
    public static IEventBus GetEntityBus(this ID id)
        => Register.instance.GetEntityBus(id);
    /// <summary>
    /// Creates an EventBus for the Entity using its ID. <br></br>
    /// If the Entity doesn't have an ID yet, it is generated automatically.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool RegisterEventBus(this Identifiable entity)
        => Register.instance.RegisterEventBus(entity);
    public static bool RegisterEventBus(this ID id)
        => Register.instance.RegisterEventBus(id);
    public static bool DisposeEventBus(this Identifiable entity)
        => Register.instance.DisposeEventBus(entity);
    public static bool DisposeEventBus(this ID id)
        => Register.instance.DisposeEventBus(id);
}
