using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using aaaaaaaa;

/*
 * Use cases:
 * 
 * 1:
 *      sub to a particular Class instance and its property, ex: [creature.stats] .life  
 *      ->  [Subscribe(nameof(StatType.Life))
 *          healthbar.onLifeChanged(Stats stats)
 *          
 *      -> so to sub to only 1 particular Stat instance, you need to sub to the creature's/stat's EventBus
 *      
 *      -> can put all event buses in IFight: Dictionary<IID, EventBus> buses;
 * 
 * 2: 
 *      sub to any Class instance and their specific property, ex: [stats] .life
 *      
 * 3:
 *      sub to any object, ex: creature
 *      
 * 
 */


/*
 * Solution:
 * 
 * - Have 1 EventBus per Entity 
 * - Store buses in Dictionary<IID, EventBus> buses; in Eevee ~~IFight~~
 * - Can sub to specific entities or the whole fight entity
 * - 
 * 
 */

public static class CentralBus
{
    private static Dictionary<ObjectId, EventBus> eventBuses = new();
    public static readonly EventBus centralBus = new();


    public static bool RegisterEventBus(this Entity e)
    {
        var result = RegisterEventBus(e.entityUid);
        centralBus.publish(nameof(RegisterEventBus), e);
        return result;
    }
    public static bool DisposeEventBus(this Entity e)
    {
        var result = DisposeEventBus(e.entityUid);
        centralBus.publish(nameof(DisposeEventBus), e);
        return result;
    }
    public static EventBus GetEntityBus(this Entity e) => GetEntityBus(e.entityUid);
    public static bool RegisterEventBus(this ObjectId id)
    {
        lock (eventBuses)
        {
            if (eventBuses.ContainsKey(id))
                throw new ArgumentException("Id already exists");
            var e = new EventBus();
            centralBus.publish(nameof(RegisterEventBus), id, e);
            eventBuses.Add(id, e);
        }
        return true;
    }
    public static bool DisposeEventBus(this ObjectId id)
    {
        lock (eventBuses)
        {
            if (!eventBuses.ContainsKey(id))
                return false;
            var e = eventBuses[id];
            eventBuses[id].Dispose();
            eventBuses.Remove(id);
            centralBus.publish(nameof(DisposeEventBus), id, e);
        }
        return true;
    }
    public static EventBus GetEntityBus(this ObjectId id)
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


/// <summary>
/// Subscribe attribute
/// Can be used on methods. 
/// 
/// The attribute can target a string path (ex: nameof(StatType.Life), "my:scope:path", nameof(CreatureModel.nameId))
/// The path is only used to pipeline events, it can be anything, doesn't mean anything.
/// The method can have parameters to serve as event objects. 
/// The parameters must match the same as the parametrs in publish()
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class SubscribeAttribute : Attribute
{
    public string[] paths = { "" };
    public SubscribeAttribute() { }
    public SubscribeAttribute(params object[] paths)
    {
        if (paths != null && paths.Length > 0)
            this.paths = paths.Select(p => p.ToString()).ToArray();
    }
}

public class Subscription
{
    public object subscriber;
    public MethodInfo method;

    public string path = "";
    public List<Type> eventParameterTypes = new List<Type>();
}

public interface IEventBus : IDisposable
{
    public void subscribe(object subscriber, params string[] methodNames);
    public void unsubscribe(object subscriber, params string[] methodNames);
    public void publish(string path = "", params object[] param);
    public void publish(params object[] param);
}

public class EventBus : IEventBus
{
    private List<Subscription> subs { get; set; } = new List<Subscription>();

    public void subscribe(object subscriber, params string[] methodNames)
    {
        lock (subs)
        {
            var at = typeof(SubscribeAttribute);
            var stype = subscriber.GetType();
            var types = stype.GetInterfaces().ToList();
            types.Add(stype);

            var methods = types.SelectMany(t => t
                    .GetMethods()
                    .Where(m => methodNames.Length == 0 || methodNames.Contains(m.Name))
                    .Where(f => f.GetCustomAttributes(at, true).Any()))
                    .Distinct();
            foreach (var m in methods)
            {
                var @params = m.GetParameters();
                var attr = (SubscribeAttribute)m.GetCustomAttribute(at, true);
                foreach (var path in attr.paths)
                {
                    var sub = new Subscription();
                    sub.subscriber = subscriber;
                    sub.method = m;
                    sub.path = path; // attr.path;
                    sub.eventParameterTypes = @params.Select(p => p.ParameterType).ToList();

                    subs.Add(sub);
                }
            }
        }
    }

    public void unsubscribe(object subscriber, params string[] methodNames)
    {
        lock (subs)
        {
            subs.RemoveAll(s => s.subscriber.Equals(subscriber) && (methodNames.Length == 0 || methodNames.Contains(s.method.Name)));
        }
    }

    public void publish(params object[] param) => publish("", param);
    public void publish(string path = "", params object[] param)
    {
        lock (subs)
        {
            foreach (var sub in subs)
            {
                if (sub.path == path && sub.eventParameterTypes.Count == param.Length)
                {
                    var match = true;
                    for (int i = 0; i < param.Length; i++)
                    {
                        match &= sub.eventParameterTypes[i].IsAssignableFrom(param[i].GetType());
                    }
                    if (match)
                        sub.method.Invoke(sub.subscriber, param);
                }
            }
        }
    }

    public void Dispose()
    {
        lock (subs)
        {
            subs.Clear();
        }
    }
}
