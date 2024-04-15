using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.util;
using VampireKiller.eevee.vampirekiller.eevee.equipment;

namespace vampirekiller.eevee.campaign.map;

public class Room
{
   
    // Like Sanctum, we can turn on/off showing room type, rewards, etc. But I dont think this goes in the Room, but rather in the Player creature passives.
    // Or I guess some rooms could be naturally "Unknown"

    public bool ShowRoomType { get; set; }
    public bool ShowRewards { get; set; }
    public bool ShowBiome { get; set; }

    public RoomType RoomType { get; set; }
    public List<Item> Rewards { get; set; } = new();
    /// <summary>
    /// Based on the biome type, we should have different bosses, merchants, treasures, rewards... on top of the biomes properties already changing
    /// </summary>
    public BiomeType BiomeType { get; set; }
}

/// <summary>
/// RoomType, and then we need a script for each of those, to actually generate the room, NPCs, environment, rewards, etc
/// 
/// Some of these rooms should have a reward and we should show it on the door, like in Sanctum and Tinyrogues.
/// </summary>
public enum RoomType
{
    //Unknown, // = ShowRoomType == false ? 
    Fight,
    Merchant, // Can have different merchants based on biome type
    Market, // all the merchants together
    Treasure, // just rewards, no fight
    Campfire, // 
    Inn,
    Nurse,
    Boss,
}

/// <summary>
/// For each biome, we have a different tileset, mobs, drops, etc
/// </summary>
public enum BiomeType
{
    Dungeon, // ⛓
    Castle, // 👑
    Plains, // 🌿
    Mountain, // ⛰
    Desert, // 🌵
    Snow, // ❄
    Caverns, // 🍄💣
    Lava, // 🔥
}

public class Biome
{
    public BiomeType BiomeType { get; set; }
    /// <summary>
    /// <CreatureModelID>
    /// </summary>
    public List<ID> SpawnableCreatures { get; set; } = new();
    /// <summary>
    /// <ItemID, Weight>
    /// </summary>
    public LootTable LootTable {  get; set; } = new();
}

