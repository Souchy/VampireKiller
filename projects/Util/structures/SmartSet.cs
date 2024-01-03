using System.Linq;
using Util.communication.events;
using Util.entity;

namespace Util.structures;

public class SmartSet<T> : Identifiable
{
    public ID entityUid { get; set; }
    protected HashSet<T> list { get; set; } = new();

    protected SmartSet() { }
    protected SmartSet(ID entityUid) : base()
    {
        this.entityUid = entityUid;
    }
    public static SmartSet<T> Create()
    {
        var set = new SmartSet<T>();
        set.RegisterEventBus();
        return set;
    }

    public IEnumerable<T> values => list;

    /// <summary>
    /// Let child class implement this
    /// </summary>
    public virtual void initialize() { }

    public T? get(Func<T, bool> predicate) => list.FirstOrDefault(predicate);

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
        if(result)
        {
            this.GetEntityBus().publish(nameof(remove), this, value);
            this.GetEntityBus().unsubscribe(value);
        }
        return result;
    }

    public void clear() {
        foreach(var item in list.ToList())
            remove(item);
    }

    public int size()
    {
        return list.Count;
    }
    public void Dispose()
    {
        foreach(var item in list)
            if(item is IDisposable d) 
                d.Dispose();
        clear();
        this.DisposeEventBus();
    }
}
