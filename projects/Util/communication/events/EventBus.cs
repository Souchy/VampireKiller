using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Util.entity;

namespace Util.communication.events;


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
 * - Have 1 EventBus per IEntity 
 * - Store buses in Dictionary<IID, EventBus> buses; in Eevee ~~IFight~~
 * - Can sub to specific entities or the whole fight entity
 * - 
 * 
 */


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
    /// <summary>
    /// Creates a new Subscription for each method in the subscriber that has the [Subscribe] attribute and for each path in the attribute.
    /// </summary>
    /// <param name="subscriber">Subscriber object who will receive events</param>
    /// <param name="methodNames">Leave empty if you want to subcsribe all methods. Add method names to subscribe only few.</param>
    public void subscribe(object subscriber, params string[] methodNames);
    /// <summary>
    /// Removes every Subscription for the subscriber (1 per path/method)
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="methodNames"></param>
    public void unsubscribe(object subscriber, params string[] methodNames);
    /// <summary>
    /// Publish an event with a path. It will iterate through all subscriptions and activate only matching ones.
    /// </summary>
    /// <param name="path">Filters subscriptions based on path. Ex: can use nameof(myMethod).</param>
    /// <param name="param">Parameters to transfer. The parameters must match the subscriber's method to activate it.</param>
    public void publish(string path = "", params object[] param);
    /// <summary>
    /// Publish an event with no path. It will iterate through all subscriptions and activate only matching ones.
    /// </summary>
    /// <param name="param">Parameters to transfer. The parameters must match the subscriber's method to activate it.</param>
    public void publish(params object[] param);
}

public class EventBus : IEventBus
{
    public static Func<IEventBus> factory = () => new EventBus();
    public static readonly IEventBus centralBus = EventBus.factory();


    protected List<Subscription> subs { get; set; } = new List<Subscription>();
    protected EventBus() { }

    public virtual void subscribe(object subscriber, params string[] methodNames)
    {
        lock (subs)
        {
            var at = typeof(SubscribeAttribute);
            var stype = subscriber.GetType();
            var types = stype.GetInterfaces().ToList();
            types.Add(stype);

            var methods = types.SelectMany(t => t
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(m => methodNames.Length == 0 || methodNames.Contains(m.Name))
                    .Where(f => f.GetCustomAttributes(at, true).Any()))
                    .Distinct();
            foreach (var m in methods)
            {
                var @params = m.GetParameters();
                var attr = (SubscribeAttribute) m.GetCustomAttribute(at, true);
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

    public virtual void unsubscribe(object subscriber, params string[] methodNames)
    {
        lock (subs)
        {
            subs.RemoveAll(s => s.subscriber.Equals(subscriber) && (methodNames.Length == 0 || methodNames.Contains(s.method.Name)));
        }
    }

    public virtual void publish(params object[] param) => publish("", param);
    public virtual void publish(string path = "", params object[] param)
    {
        List<Subscription> subs;
        lock (this.subs)
        {
            subs = this.subs.ToList();
        }
        foreach (var sub in subs)
        {
            if (sub.path == path && sub.eventParameterTypes.Count == param.Length)
            {
                var match = true;
                for (int i = 0; i < param.Length; i++)
                {
                    if (param[i] != null)
                        match &= sub.eventParameterTypes[i].IsAssignableFrom(param[i].GetType());
                }
                if (match)
                    sub.method.Invoke(sub.subscriber, param);
            }
        }
    }

    public virtual void Dispose()
    {
        lock (subs)
        {
            subs.Clear();
        }
    }
}
