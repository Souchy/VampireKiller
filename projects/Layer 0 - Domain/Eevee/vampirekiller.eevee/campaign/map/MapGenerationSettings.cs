using Util.entity;
using vampirekiller.eevee.util;

namespace vampirekiller.eevee.campaign.map;

public class MapGenerationSettings
{
    public int? Seed { get; set; } = null;
    public int MaxFloorWidth { get; set; }
    public int FloorWidthRandomness { get; set; } = 0;
    public WeightTable<FloorType> FloorTypeWeights { get; set; } = new(new()
    {
        { FloorType.Normal,      100 },
        { FloorType.Boss,        005 },
        { FloorType.Treasure,    010 },
    });
    public WeightTable<RoomType> RoomTypeWeights { get; set; } = new(new()
    {
        { RoomType.Boss,        000 }, // 0 = no random spawn of bosses, only obtainable through boss floors
        { RoomType.Fight,       100 },
        { RoomType.Merchant,    010 },
        { RoomType.Market,      005 },
        { RoomType.Nurse,       010 },
        { RoomType.Inn,         010 },
        { RoomType.Treasure,    010 },
        { RoomType.Campfire,    010 },
    });
    public WeightTable<ID> BiomeWeights { get; set; } = new();
    public WeightTable<ID> DropWeights { get; set; } = new();
    public int VisibleRoomTypeChance {  get; set; } = 100;
    public int VisibleRoomRewardsChance { get; set; } = 100;
    public int VisibleRoomBiomeChance { get; set; } = 100;

    public Random Random() => Seed.HasValue ? new Random(Seed.Value) : new Random();

}
