using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;

namespace Util.ecs;

public class Entity : Identifiable
{
    public ID entityUid { get; set; }
    public Dictionary<Type, object> components { get; set; } = new();

    public T? get<T>()
    {
        if (!components.ContainsKey(typeof(T)))
            return default;
        return (T)components[typeof(T)];
    }

    public void set(object component)
    {
        if (component == null)
            return;
        remove(component.GetType());
        components[component.GetType()] = component;
        this.GetEntityBus().subscribe(component);
        this.GetEntityBus().publish(nameof(set), this, component);
    }

    public void remove<T>() => remove(typeof(T));

    public void remove(Type type)
    {
        if (!components.ContainsKey(type))
            return;

        var component = components[type];
        components.Remove(type);
        this.GetEntityBus().publish(nameof(remove), this, component);
        this.GetEntityBus().unsubscribe(component);
    }

    public void Dispose()
    {
        this.GetEntityBus().publish(nameof(Dispose), this);
        foreach (object component in components.Values)
        {
            this.GetEntityBus().unsubscribe(component);
        }
        components.Clear();
    }
}
