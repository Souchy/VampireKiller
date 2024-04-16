using Godot;
using System.Drawing;
using vampirekiller.eevee.util;

namespace vampirekiller.eevee.campaign.map;

public class MapGenerator
{

    public Map GenerateByPaths(MapGenerationSettings settings, int floorCount)
    {
        Map map = new Map();
        AddFloors(settings, map, floorCount);
        return map;
    }

    public void AddFloors(MapGenerationSettings settings, Map map, int addCount)
    {
        //var rnd = settings.Random();
        int initialCount = map.Floors.Count;
        for (int i = initialCount; i < initialCount + addCount; i++)
        {
            Floor floor = new Floor();
            floor.Index = i;
            map.Floors.Add(floor);
            GenFloor(settings, map, floor);
            if (i == 0)
            {
                _addRoomsToStartingFloor(settings, floor);
            }
            else
            if (floor.FloorType == FloorType.Boss)
            {
                _addRoomsToBossFloor(settings, map, floor);
            }
            else
            {
                _addRoomsToRegularFloor(settings, map, floor);
            }
            foreach (var room in floor.Rooms)
                GenRoom(settings, map, floor, room);
        }
    }
    private void _addRoomsToStartingFloor(MapGenerationSettings settings, Floor floor)
    {
        var startIndex = GetMiddleIndex(settings);
        floor.Rooms.Add(new Room(startIndex));
    }
    private int GetMiddleIndex(MapGenerationSettings settings)
    {
        var a = settings.MaxFloorWidth - 1;
        double b = a / 2.0;
        int middle = settings.Random().Next((int) Math.Floor(b), (int) Math.Ceiling(b));
        return middle;
    }
    private void _addRoomsToBossFloor(MapGenerationSettings settings, Map map, Floor floor)
    {
        var room = new Room(settings.Random().Next(settings.MaxFloorWidth));
        floor.Rooms.Add(room);

        room.Index = GetMiddleIndex(settings);

        Floor previous = map.Floors[floor.Index - 1];
        foreach (Room previousRoom in previous.Rooms)
        {
            previousRoom.Connections.Add(room.Index);
        }
    }
    private void _addRoomsToRegularFloor(MapGenerationSettings settings, Map map, Floor floor)
    {
        Floor previous = map.Floors[floor.Index - 1];
        List<Room> previousRooms = previous.Rooms.ToList();
        while (previousRooms.Count > 0)
        {
            var id = settings.Random().Next(previousRooms.Count);
            var previousRoom = previousRooms[id];
            previousRooms.Remove(previousRoom);
            Branch(settings, map, floor.Index - 1, previousRoom.Index);
            foreach (int connection in previousRoom.Connections)
                if (floor.GetRoomAt(connection) == null)
                {
                    var room = new Room(connection);
                    floor.Rooms.Add(room);
                }
        }
    }


    private void GenFloor(MapGenerationSettings settings, Map map, Floor floor)
    {
        do
        {
            floor.FloorType = settings.FloorTypeWeights.Pick(settings.Seed);
        }
        // Dont pick a boss floor twice in a row
        while (floor.FloorType == FloorType.Boss && map.Floors[floor.Index - 1].FloorType == FloorType.Boss);
    }

    private void GenRoom(MapGenerationSettings settings, Map map, Floor floor, Room room)
    {
        var rnd = settings.Random();
        if (floor.FloorType == FloorType.Boss)
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
            room.RoomType = settings.RoomTypeWeights.Pick(settings.Seed);
        }
        room.BiomeId = settings.BiomeWeights.Pick(settings.Seed);
        room.Rewards = room.GetBiome().RewardsWeights.Pick(settings.Seed, 3);
        room.ShowRoomType = rnd.Next(1, 100) >= settings.VisibleRoomTypeChance;
        room.ShowRoomBiome = rnd.Next(1, 100) >= settings.VisibleRoomBiomeChance;
        room.ShowRoomRewards = rnd.Next(1, 100) >= settings.VisibleRoomRewardsChance;
        room.VisualOffset = new Vector2((rnd.NextSingle() - 0.5f) / 2f, (rnd.NextSingle() - 0.5f) / 2f);
    }

    /// <summary>
    /// Branch from a floor to the next
    /// </summary>
    /// <param name="floor">floor - 1</param>
    /// <param name="room">room on floor - 1</param>
    private void Branch(MapGenerationSettings settings, Map map, int floor, int room)
    {
        var possiblePaths = new List<int>();
        // If there's only one room on the floor, it can branch to any room on the next floor
        if (map.Floors[floor].Rooms.Count == 1)
        {
            for (int i = 0; i < settings.MaxFloorWidth; i++)
                possiblePaths.Add(i);
        }
        // Otherwise branch only to neighbooring rooms on the next floor
        else
        {
            possiblePaths.Add(room - 1);
            possiblePaths.Add(room);
            possiblePaths.Add(room + 1);
        }
        possiblePaths = possiblePaths
            .Where(i => i >= 0 && i < settings.MaxFloorWidth) // horizontal edge
            .Where(i => !IsCrossingPath(map, floor, room, i)) // no crossing
            .ToList();
        // If the floor has only 1 room, we want at least 2 outgoing connections
        int branchCount = settings.BranchCountWeights.Pick(settings.Seed);
        if (map.Floors[floor].Rooms.Count == 1)
            branchCount = Math.Max(2, branchCount);
        // Pick x random paths out of the possibilities.
        var chosenPaths = Util.Util.PickUniques(settings.Random(), possiblePaths, branchCount);
        // Set connections
        map.Floors[floor].GetRoomAt(room)!.Connections = chosenPaths;
    }

    private bool IsCrossingPath(Map map, int floor1, int room1, int room2)
    {
        if (map.Floors.Count <= floor1 + 1)
            return false;
        var floor = map.Floors[floor1];
        var nextRoom = floor.GetRoomAt(room2);
        var hasConnection = nextRoom?.HasConnection(room1);
        return hasConnection ?? false;
    }

    /*
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
        floor.FloorType = settings.FloorTypeWeights.Pick(settings.Seed);
        int numberOfRooms = settings.MaxFloorWidth;
        int numberOfRoomsRemoved = 0;
        if (floor.FloorType == FloorType.Boss)
        {
            numberOfRooms = 1;
        }
        else
        {
            numberOfRoomsRemoved = (int)Math.Floor(rnd.NextDouble() * settings.FloorWidthRandomness * settings.MaxFloorWidth);
            //numberOfRooms -= numberOfRoomsRemoved;
        }
        // [x,x,x,x]
        // [x,_,_,x]
        HashSet<int> indices = new();

        // Fill indices, then remove random indices
        for (int i = 0; i < numberOfRooms; i++)
            indices.Add(i);
        for (int i = 0; i < numberOfRoomsRemoved; i++)
            indices.Remove(rnd.Next(indices.Count));

        // Generate rooms for each index remaining
        foreach (var i in indices)
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
        if (floor.FloorType == FloorType.Boss)
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
            room.RoomType = settings.RoomTypeWeights.Pick(settings.Seed);
        }
        room.BiomeId = settings.BiomeWeights.Pick(settings.Seed);
        room.Rewards = room.GetBiome().RewardsWeights.Pick(settings.Seed, 3);
        room.ShowRoomType = rnd.Next(1, 100) >= settings.VisibleRoomTypeChance;
        room.ShowRoomBiome = rnd.Next(1, 100) >= settings.VisibleRoomBiomeChance;
        room.ShowRoomRewards = rnd.Next(1, 100) >= settings.VisibleRoomRewardsChance;
        room.VisualOffset = new Vector2(rnd.NextSingle() - 0.5f, rnd.NextSingle() - 0.5f);
        return room;
    }

    public void GenerateConnections(MapGenerationSettings settings, Map map, int startFloor = 0)
    {
        for (int i = startFloor; i < map.Floors.Count - 1; i++)
        {
            var f1 = map.Floors[i];
            var f2 = map.Floors[i + 1];

            IEnumerable<(Room current, IEnumerable<Room> nexts)> closeRooms = f1.Rooms.Select(r => (r, f2.GetRoomClose(r.Index)));

            foreach (var pair in closeRooms)
            {
                // si seulement 1 room proche de la courrante, obligé de connecter.
                if (pair.nexts.Count() == 1)
                {
                    pair.current.Connections.Add(pair.nexts.First().Index);
                }
            }

            // Probleme: il peut rester des rooms qui n'ont pas de connections sortante.
            // Il peut y avoir des floors comme ça vu qu'on est 100% aléatoire.
            // [x___]
            // [___x]
            // Ce serait mieux d'avoir un perlin noise, ou respecter des limites.
            // Surtout quand on augmente le floorWidth à 5,6,7,8,9,10, ce sera trop aléatoire et impossible de créer un chemin
            // P-e tu créé une grid de 10 floors d'un coup à générer


            for (int j = 0; j < f1.Rooms.Count; j++)
            {

            }
            for (int j = 0; j < f1.Rooms.Count; j++)
            {

            }

        }
    }

    public void ConnectBackwards(MapGenerationSettings settings, Map map, Floor floor)
    {

    }

    public void ConnectForward(MapGenerationSettings settings, Map map, Floor floor)
    {

    }

    /// <summary>
    /// Each room needs at least one connection in and out
    /// Connections cannot intersect
    /// Connections can go to +1 or -1 index rooms, not more. // Creates a problem where nextFloor[1,2] indices must be close to lastFloor[0] indices
    /// x x_ 
    /// |\| \
    /// x x  x
    ///  \|  |
    ///   x  x
    ///   \ / \
    ///   x   |
    /// / | \ |
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
    */


}
