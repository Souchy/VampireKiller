using Util.communication.events;
using Util.entity;

namespace Util.structures;

public class SmartSet<T> : IEntity
{
    public ID entityUid { get; set; }
    private HashSet<T> list { get; set; } = new();

    private SmartSet() { }
    private SmartSet(ID entityUid) : base()
    {
        this.entityUid = entityUid;
    }
    public static SmartSet<V> Create<V>()
    {
        var set = new SmartSet<V>();
        set.RegisterEventBus();
        return set;
    }

    public bool add(T value)
    {
        bool result = list.Add(value);
        if (!result) 
            return false;
        this.GetEntityBus().subscribe(value);
        this.GetEntityBus().publish(nameof(add), this, value);
        return true;
    }
    public bool remove(T value)
    {
        var result = list.Remove(value);
        this.GetEntityBus().publish(nameof(remove), this, value);
        this.GetEntityBus().unsubscribe(value);
        return result;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}