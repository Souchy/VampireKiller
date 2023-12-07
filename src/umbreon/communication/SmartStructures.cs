using System.Collections.Generic;
using aaaaaaaa;

public class SmartDictionary<K, V> : Entity {
    public ObjectId entityUid { get; set; }
    public Dictionary<K, V> dic { get; set; } = new();

    private SmartDictionary() {
        this.RegisterEventBus();
    }
    private SmartDictionary(ObjectId entityUid) : base() {
        this.entityUid = entityUid;
    }
    public static SmartDictionary<A, B> Create<A, B>() {
        return new SmartDictionary<A, B>();
    }

    public void set(K key, V value) {
        dic[key] = value;
        this.GetEntityBus().publish(nameof(set), key, value);
    }
    public bool add(K key, V value) {
        if(dic.ContainsKey(key)) return false;
        set(key, value);
        this.GetEntityBus().publish(nameof(add), key, value);
        return true;
    }
    public bool remove(K key) {
        var result = dic.Remove(key);
        this.GetEntityBus().publish(nameof(remove), key);
        return result;
    }
}

public class SmartList<T> : Entity {
    public ObjectId entityUid { get; set; }
    private List<T> list { get; set; } = new();

    private SmartList() {
        this.RegisterEventBus();
    }
    private SmartList(ObjectId entityUid) : base() {
        this.entityUid = entityUid;
    }
    public static SmartList<V> Create<V>() {
        var id = Entity.RegisterObjectId();
        return new SmartList<V>(id);
    }

    public void set(int index, T value) {
        list.Insert(index, value);
        this.GetEntityBus().publish(nameof(set), index, value);
    }
    public void add(T value) {
        list.Add(value);
        this.GetEntityBus().publish(nameof(add), value);
    }
    public bool remove(T value) {
        var result = list.Remove(value);
        this.GetEntityBus().publish(nameof(remove), value);
        return result;
    }

}

public class SmartSet<T> : Entity {
    public ObjectId entityUid { get; set; }
    private HashSet<T> list { get; set; } = new();

    private SmartSet() {
        this.RegisterEventBus();
    }
    private SmartSet(ObjectId entityUid) : base() {
        this.entityUid = entityUid;
    }
    public static SmartSet<V> Create<V>() {
        var id = Entity.RegisterObjectId();
        return new SmartSet<V>(id);
    }

    public void add(T value) {
        list.Add(value);
        this.GetEntityBus().publish(nameof(add), value);
    }
    public bool remove(T value) {
        var result = list.Remove(value);
        this.GetEntityBus().publish(nameof(remove), value);
        return result;
    }

}