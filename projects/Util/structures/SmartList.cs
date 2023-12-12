using Util.communication.events;
using Util.entity;

namespace Util.structures;

public class SmartList<T> : IEntity
{
    public ID entityUid { get; set; }
    private List<T> list { get; set; } = new();

    private SmartList() { }
    private SmartList(ID entityUid) : base()
    {
        this.entityUid = entityUid;
    }
    public static SmartList<V> Create<V>()
    {
        var list = new SmartList<V>();
        list.RegisterEventBus();
        return list;
    }

    public void setAt(int index, T value)
    {
        removeAt(index);
        list.Insert(index, value);
        this.GetEntityBus().subscribe(value);
        this.GetEntityBus().publish(nameof(setAt), this, index, value);
    }
    public void add(T value)
    {
        list.Add(value);
        this.GetEntityBus().subscribe(value);
        this.GetEntityBus().publish(nameof(add), this, value);
    }
    public bool removeAt(int index)
    {
        if (index > 0 && index < list.Count)
            return remove(list[index]);
        return false;
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
