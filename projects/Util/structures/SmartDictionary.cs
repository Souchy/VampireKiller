using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;

namespace Util.structures;

public class SmartDictionary<K, V> : Identifiable
{
    public const string EventSet = "SmartDictionary." + nameof(set);
    public const string EventRemove = "SmartDictionary." + nameof(remove);

    [JsonIgnore]
    public ID entityUid { get; set; }
    public Dictionary<K, V> dic { get; set; } = new();

    protected SmartDictionary() { }
    protected SmartDictionary(ID entityUid) : base()
    {
        this.entityUid = entityUid;
    }
    public static SmartDictionary<K, V> Create()
    {
        var dic = new SmartDictionary<K, V>();
        dic.RegisterEventBus();
        return dic;
    }

    [JsonIgnore]
    public IEnumerable<K> keys => dic.Keys;
    [JsonIgnore]
    public IEnumerable<V> values => dic.Values;
    [JsonIgnore]
    public IEnumerable<KeyValuePair<K, V>> pairs => dic;

    /// <summary>
    /// Let child class implement this
    /// </summary>
    public virtual void initialize() { }

    public virtual V? get(K key)
    {
        if (!dic.ContainsKey(key))
            return default;
        return dic[key];
    }
    /// <summary>
    /// Sets a key-value pair, removing existing pair if necessary
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public virtual void set(K key, V value)
    {
        if (dic.ContainsKey(key))
            remove(key);
        dic[key] = value;
        this.GetEntityBus().subscribe(value);
        this.GetEntityBus().publish(EventSet, this, key, value);
    }
    /// <summary>
    /// Sets only if there's no existing pair already
    /// </summary>
    public virtual bool add(K key, V value)
    {
        if (dic.ContainsKey(key)) return false;
        set(key, value);
        return true;
    }
    public virtual bool remove(K key)
    {
        if (!dic.ContainsKey(key))
            return false;
        var value = dic[key];
        var result = dic.Remove(key);
        if(result)
        {
            this.GetEntityBus().publish(EventRemove, this, key, value);
            this.GetEntityBus().unsubscribe(value);
        }
        return result;
    }

    public virtual void Dispose()
    {
        foreach (var item in values)
            if (item is IDisposable d)
                d.Dispose();
        this.dic.Clear();
        this.DisposeEventBus();
    }
}
