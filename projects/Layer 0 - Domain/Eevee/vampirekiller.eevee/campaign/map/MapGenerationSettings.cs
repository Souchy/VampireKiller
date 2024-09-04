using Util.entity;
using vampirekiller.eevee.util;

namespace vampirekiller.eevee.campaign.map;

public class MapGenerationSettings
{
    public int? Seed { get; set; } = null;
    public int MaxFloorWidth { get; set; } = 5;
    //public int FloorWidthRandomness { get; set; } = 0;
    public WeightTable<FloorType> FloorTypeWeights { get; set; } = new(new()
    {
        { FloorType.Normal,      150 },
        { FloorType.Boss,        005 },
        { FloorType.Treasure,    010 },
    });
    public WeightTable<RoomType> RoomTypeWeights { get; set; } = new(new()
    {
        { RoomType.Boss,        000 }, // 0 = no random spawn of bosses, only obtainable through boss floors
        { RoomType.Fight,       200 },
        { RoomType.Merchant,    020 },
        { RoomType.ScrollVendor,015 },
        { RoomType.FoodVendor,  015 },
        { RoomType.Market,      005 },
        { RoomType.Nurse,       020 },
        //{ RoomType.Inn,         010 },
        { RoomType.Treasure,    020 },
        { RoomType.Campfire,    020 },
    });
    public WeightTable<ID> BiomeWeights { get; set; } = new();
    public WeightTable<ID> DropWeights { get; set; } = new();
    public int VisibleRoomTypeChance { get; set; } = 100;
    public int VisibleRoomRewardsChance { get; set; } = 100;
    public int VisibleRoomBiomeChance { get; set; } = 100;

    public WeightTable<int> BranchCountWeights = new WeightTable<int>();
    public void AutoDistributeBranchCountWeights()
    {
        BranchCountWeights.Clear();
        //BranchCountWeights.Add(1, 50);
        //BranchCountWeights.Add(2, 20);
        //BranchCountWeights.Add(3, 10);
        //BranchCountWeights.Add(4, 5);
        //BranchCountWeights.Add(5, 1);
        for (int i = 0; i <= MaxFloorWidth; i++)
        {
            double weight;
            double bSlope = 25;
            double a = Math.Pow(i - 1, -1.2) * MaxFloorWidth;
            double b = -i * bSlope + MaxFloorWidth + 2 * bSlope;
            if (i <= 2)
            {
                weight = b;
            }
            else
            {
                weight = a;
            }
            int precision = (int) (weight * 100);
            BranchCountWeights.Add(i + 1, precision); // (int) (Math.Pow(i + 1, -2) * 100));
        }
    }

    private Random _rnd;
    public Random Random()
    {
        return _rnd ??= Seed.HasValue ? new Random(Seed.Value) : new Random();
    }

}
