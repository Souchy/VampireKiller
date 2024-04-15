using Util.entity;
using vampirekiller.eevee.util;

namespace vampirekiller.eevee.campaign.map;

/// <summary>
/// TODO: Define biomes in TestGems generators, then save them as json, then load them in Diamonds
/// </summary>
public class Biome : Identifiable
{
    public ID entityUid { get; set; }

    public ID nameId { get; set; }

    /// <summary>
    /// <CreatureModelID>
    /// </summary>
    public List<ID> SpawnableCreatures { get; set; } = new();
    /// <summary>
    /// <ItemID, Weight>
    /// </summary>
    public WeightTable<ID> LootTable {  get; set; } = new();

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// For each biome, we have a different tileset, mobs, drops, etc
/// </summary>
//public enum BiomeType
//{
//    Dungeon, // ⛓
//    Castle, // 👑
//    Plains, // 🌿
//    Mountain, // ⛰
//    Desert, // 🌵
//    Snow, // ❄
//    Caverns, // 🍄💣
//    Lava, // 🔥
//}

//public class BiomeType2
//{
//    public string bla = "Dungeon";
//}
