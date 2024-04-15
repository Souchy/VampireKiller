using Newtonsoft.Json;
using Util.entity;

namespace vampirekiller.eevee.util;

public class LootTable
{
    [JsonProperty("loot")]
    private readonly SortedDictionary<ID, int> _table = new();

    /// <summary>
    /// Total weight
    /// </summary>
    public int Size() 
        => _table.Values.Sum();

    public void Add(ID itemID, int weight)
        => _table.Add(itemID, weight);

    /// <summary>
    /// Pick a random item ID
    /// </summary>
    public ID Pick()
    {
        var i = new Random().Next(0, Size());
        var sum = 0;
        foreach (var pair in _table)
        {
            sum += pair.Value;
            if(i < sum)
                return pair.Key;
        }
        return _table.First().Key;
    }

    /// <summary>
    /// Adds the loot table to this one and returns this. No copy
    /// </summary>
    public LootTable Add(LootTable other)
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

    public LootTable Copy()
    {
        LootTable copy = new();
        foreach (var pair in _table)
        {
            copy._table[pair.Key] = pair.Value;
        }
        return copy;
    }

}
