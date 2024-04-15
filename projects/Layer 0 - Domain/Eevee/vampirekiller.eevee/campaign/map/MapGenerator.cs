using Godot;

namespace vampirekiller.eevee.campaign.map;

public class MapGenerator
{
    public Map GenerateMap(MapGenerationSettings settings, int floorCount)
    {
        Map map = new Map();
        for (int i = 0; i < floorCount; i++)
        {
            var floor = GenerateFloor(settings);
            map.Floors.Add(floor);
        }
        return map;
    }

    /// <summary>
    /// x_xx
    /// xx_x
    /// xxxx
    /// 
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public Floor GenerateFloor(MapGenerationSettings settings)
    {
        var rnd = settings.Random();
        Floor floor = new Floor();
        floor.FloorType = settings.FloorTypeWeights.Pick();
        int numberOfRooms = settings.MaxFloorWidth;
        int numberOfRoomsRemoved = 0;
        if (floor.FloorType == FloorType.Boss)
        {
            numberOfRooms = 1;
        }
        else
        {
            numberOfRoomsRemoved = (int) Math.Floor(rnd.NextDouble() * settings.FloorWidthRandomness * settings.MaxFloorWidth);
            //numberOfRooms -= numberOfRoomsRemoved;
        }
        // [x,x,x,x]
        // [x,_,_,x]
        HashSet<int> indices = new();

        // Fill indices, then remove random indices
        for (int i = 0; i < numberOfRooms; i++)
            indices.Add(i);
        for(int i = 0; i < numberOfRoomsRemoved; i++)
            indices.Remove(rnd.Next(indices.Count));

        // Generate rooms for each index remaining
        foreach(var i in indices)
        {
            var room = GenerateRoom(settings, floor);
            room.Index = i;
            floor.Rooms.Add(room);
        }

        return floor;
    }

    public Room GenerateRoom(MapGenerationSettings settings, Floor floor)
    {
        var rnd = settings.Random();
        Room room = new Room();
        if (floor.FloorType != FloorType.Boss)
        {
            room.RoomType = RoomType.Boss;
        }
        else
        if (floor.FloorType == FloorType.Treasure)
        {
            room.RoomType = RoomType.Treasure;
        }
        else
        {
            room.RoomType = settings.RoomTypeWeights.Pick();
        }
        room.BiomeId = settings.BiomeWeights.Pick();
        room.ShowRoomType = rnd.Next(1, 100) >= settings.VisibleRoomTypeChance;
        room.ShowRoomBiome = rnd.Next(1, 100) >= settings.VisibleRoomBiomeChance;
        room.ShowRoomRewards = rnd.Next(1, 100) >= settings.VisibleRoomRewardsChance;
        room.VisualOffset = new Vector2(rnd.NextSingle() - 0.5f, rnd.NextSingle() - 0.5f);
        return room;
    }

    /// <summary>
    /// Each room needs at least one connection in and out
    /// Connections cannot intersect
    /// Connections can go to +1 or -1 index rooms, not more. // Creates a problem where nextFloor indices must be close to lastFloor indices
    /// x x_ 
    /// |\| \
    /// x x  x
    ///  \|  |
    ///   x  x
    ///   \ / \
    ///    x  |
    /// / / \ |
    /// x x x x
    /// 
    /// What the fuck do I do with connections that go over a floor
    /// </summary>
    public void Connect(MapGenerationSettings settings, Floor floor1, Floor floor2)
    {
        var rnd = settings.Random();
        (int roomCount1, int roomCount2) = (floor1.Rooms.Count, floor1.Rooms.Count);
        int diff = roomCount2 - roomCount1;
        int maxNumberOfConnections = roomCount1 + roomCount2 - 1;
        int minNumberOfConnections = Math.Max(roomCount1, roomCount2);
        int numberOfConnections = rnd.Next(minNumberOfConnections, maxNumberOfConnections);

    }

}
