using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;

namespace Util.structures;

public class SmartDictionary<K, V> : IEntity
{
    public ID entityUid { get; set; }
    public Dictionary<K, V> dic { get; set; } = new();

    private SmartDictionary() { }
    private SmartDictionary(ID entityUid) : base()
    {
        this.entityUid = entityUid;
    }
    public static SmartDictionary<A, B> Create<A, B>()
    {
        var dic = new SmartDictionary<A, B>();
        dic.RegisterEventBus();
        return dic;
    }

    public void set(K key, V value)
    {
        if (dic.ContainsKey(key))
            remove(key);
        dic[key] = value;
        this.GetEntityBus().subscribe(value);
        this.GetEntityBus().publish(nameof(set), this, key, value);
    }
    public bool add(K key, V value)
    {
        if (dic.ContainsKey(key)) return false;
        set(key, value);
        this.GetEntityBus().subscribe(value);
        this.GetEntityBus().publish(nameof(add), this, key, value);
        return true;
    }
    public bool remove(K key)
    {
        if (!dic.ContainsKey(key))
            return false;
        var value = dic[key];
        var result = dic.Remove(key);
        this.GetEntityBus().publish(nameof(remove), this, key);
        this.GetEntityBus().unsubscribe(value);
        return result;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
