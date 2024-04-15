using Godot;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.eevee.campaign.map;

public class Room
{
   
    // Like Sanctum, we can turn on/off showing room type, rewards, etc. But I dont think this goes in the Room, but rather in the Player creature passives.
    // Or I guess some rooms could be naturally "Unknown"
    public bool ShowRoomType { get; set; }
    public bool ShowRoomRewards { get; set; }
    public bool ShowRoomBiome { get; set; }

    /// <summary>
    /// Offset from the index
    /// </summary>
    public Vector2 VisualOffset { get; set; }
    /// <summary>
    /// Index, X, Position of the room on the floor
    /// </summary>
    public int Index {  get; set; }

    public RoomType RoomType { get; set; }
    /// <summary>
    /// Item rewards
    /// </summary>
    public List<ID> Rewards { get; set; } = new();
    /// <summary>
    /// Based on the biome type, we should have different bosses, merchants, treasures, rewards... on top of the biomes properties already changing
    /// </summary>
    public ID BiomeId { get; set; }
    //public BiomeType BiomeType { get; set; }
    /// <summary>
    /// Room indices that are connected on the next floor 
    /// </summary>
    public List<int> Connections { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    public SmartList<IStatement> MonsterModifiers { get; set; } = SmartList<IStatement>.Create();
    public SmartList<IStatement> PlayerModifiers { get; set; } = SmartList<IStatement>.Create();
}

/// <summary>
/// RoomType, and then we need a script for each of those, to actually generate the room, NPCs, environment, rewards, etc
/// 
/// Some of these rooms should have a reward and we should show it on the door, like in Sanctum and Tinyrogues.
/// </summary>
public enum RoomType
{
    //Unknown, // = ShowRoomType == false ? 
    Boss,
    Fight,
    Merchant, // Can have different merchants based on biome type
    Market, // all the merchants together
    Nurse,
    Inn,
    Treasure, // just rewards, no fight
    Campfire, // 
}
