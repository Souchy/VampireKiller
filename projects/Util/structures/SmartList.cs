using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Util.communication.events;
using Util.entity;

namespace Util.structures;

public class SmartList<T> : Identifiable
{
    [JsonIgnore]
    public ID entityUid { get; set; }
    private List<T> list { get; set; } = new();

    protected SmartList() { }
    protected SmartList(ID entityUid) : base()
    {
        this.entityUid = entityUid;
    }
    public static SmartList<T> Create()
    {
        var list = new SmartList<T>();
        list.RegisterEventBus();
        return list;
    }

    public IEnumerable<T> values => list;

    /// <summary>
    /// Let child class implement this
    /// </summary>
    public virtual void initialize() { }

    public T? get(Func<T, bool> predicate) => list.First(predicate);
    public T? get(int index) => list[index];

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
        lock(this)
        {
            if(index >= 0 && index < list.Count)
            {
                var value = this.list[index];
                this.list.RemoveAt(index);
                this.GetEntityBus().publish(nameof(remove), this, value);
                this.GetEntityBus().unsubscribe(value);
                return true;
            }
            return  false;
        }
    }
    public T? getAt(int index)
    {
        if (index >= 0 && index < list.Count)
            return list[index];
        return default;
    }
    public bool remove(T value)
    {
        return removeAt(list.IndexOf(value));
    }

    public void Dispose()
    {
        foreach (var item in list)
            if (item is IDisposable d)
                d.Dispose();
        this.list.Clear();
        this.DisposeEventBus();
    }
}
