using Newtonsoft.Json;
using Util.entity;

namespace vampirekiller.eevee.util;

public class WeightTable<T> where T : notnull
{
    /// <summary>
    /// Previously SortedDictionary. 
    /// </summary>
    [JsonProperty("loot")]
    private readonly Dictionary<T, int> _table = new(); 

    public WeightTable() { }
    public WeightTable(Dictionary<T, int> table)
    {
        _table = table;
    }

    /// <summary>
    /// Total weight
    /// </summary>
    public int Size()
        => _table.Values.Sum();

    public void Add(T itemId, int weight)
        => _table.Add(itemId, weight);

    /// <summary>
    /// Pick a random item ID
    /// </summary>
    public T Pick(int? seed = null)
    {
        var rnd = seed.HasValue ? new Random(seed.Value) : new Random();
        var i = rnd.Next(0, Size());

        var sum = 0;
        foreach (var pair in _table)
        {
            sum += pair.Value;
            if (i < sum)
                return pair.Key;
        }
        return _table.First().Key;
    }

    /// <summary>
    /// Adds the loot table to this one and returns this. No copy
    /// </summary>
    public WeightTable<T> Add(WeightTable<T> other)
    {
        foreach (var pair in other._table)
        {
            if (_table.ContainsKey(pair.Key))
                _table[pair.Key] += pair.Value;
            else
                _table[pair.Key] = pair.Value;
        }
        return this;
    }

    public WeightTable<T> Copy()
    {
        WeightTable<T> copy = new();
        foreach (var pair in _table)
        {
            copy._table[pair.Key] = pair.Value;
        }
        return copy;
    }

}
