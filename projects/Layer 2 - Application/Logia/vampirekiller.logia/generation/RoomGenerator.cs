using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.json;
using vampirekiller.eevee.campaign.map;
using vampirekiller.eevee.zones;

namespace vampirekiller.logia.generation;

public enum WallShapes
{
    Rectangle,
    Donut,
    CorridorH,
    CorridorV,
    DemiCircle,
    Circle,
    ShapeH,
    ShapeArchn
}

/// <summary>
/// DecorShape.json files? Blender scene files? Godot scene file?
/// In godot you can have MeshLibraries, each mesh has a transform, collision and navigation setup.
/// Things that can swap:
/// - Mesh (but we dont have many and chairs at a table need to be the same)
/// - Material (every chair needs at a table the same material tho)
/// - Orientation Y (chairs shouldnt face too away from table tho)
/// - Orientation X (cant sit in a fallen chair tho)
/// - Position (cant clip tho)
/// </summary>
public enum DecorShapes
{
    Church,
    KingRoom,
    Hall,
    Barracks,
    Storage,
    Cemetery,
    Crypt,
    Camp,
    TrainingRoom,
}

public class RoomGenerator
{
    /*
     * Biomes:
     * - Dungeon
     * - Cavern
     * - Spaceship
     * - Viking village in the snow
     */
    // 1. delimit the area
    //      1. set portals points
    //      2. ~~set spawn points (?)~~ -> nvm it's just anywhere outisde of player's view
    // 2. set walls / barriers around the area
    //      - ocean, rivers
    //      - trees, rocks, mountains, cliffs, big plants
    //      - fences, walls, other buildings
    //      - dark fog + invisible wall xdd (or it tp's you to the otherside?)
    // 3. add doors / portals / roads to the next floor (maybe start with this)
    // 4. set floors
    //      - a layer of terrain like sand, dirt
    //      - a layer of tiles
    // 5. add floor collision props: boxes, pillars, furniture, statues...
    // 6. add no-collision props
    //      - on the walls: banners, paintings, weapons, library, shelves, windows, drapes...
    //          - horror mansion has decals
    //      - on the floors and corners: carpets, crystals, plants, corpses, blood, vases, weapons...
    //      - from the ceiling: chains, noose, chandeliers
    // 7. we also need light sources
    //      - from the ceiling: chandeliers, sun, moon
    //      - from the walls: chandeliers
    //      - from the floors: chandeliers, crystals, lava, campfire, fire, fireplace
    //      - candles placed anywhere on top of props
    //      - modern lights
    // 8. add vfx: fog, sparks on fire...
    //      - dark fog around the player, in its node, to delimit his light radius ? nah just the light from the player himself i think. also it shouldn't affect the player model (different layer)

    

    public Node3D Generate(Room room)
    {
        string assetConfig = "C:\\Robyn\\godot\\VampireKiller\\projects\\Layer 1 - Data\\VampireAssets\\Assets\\PolygonDungeon\\Models\\assetfolder.json";
        AssetPack assetPack = Config.load<AssetPack>(assetConfig);


        Node3D root = new Node3D();

        // Generate Room Shape
        Points points = new();
        points.Add(PointsGenerator.rectangle(15, 10).tag(VoxelTag.Floor));
        points.Add(PointsGenerator.circleHalf(15).offset(0, 10).rotate(Rotation4Type.top).tag(VoxelTag.Wall));
        points.Add(PointsGenerator.circleHalf(10).offset(15, 0).rotate(Rotation4Type.right).tag(VoxelTag.Wall));
        points.Add(PointsGenerator.circleHalf(10).offset(-10, 0).rotate(Rotation4Type.left).tag(VoxelTag.Wall));
        points.Add(PointsGenerator.line(15).offset(0, -10).rotate(Rotation4Type.bottom).tag(VoxelTag.Wall, VoxelTag.Portals)); // portal line
        //points.Add(PointsGenerator.circleHalf(10).offset(0, -10).rotate(Rotation4Type.bottom));


        // Points as Set
        HashSet<(float, float)> set = points.toSet();

        // Get outer walls placements
        Points edges = DetectEdges(points, set);



        return root;
    }

    public Points DetectEdges(Points points, HashSet<(float, float)> set)
    {
        Points edges = new();
        foreach (var point in points)
        {
            if (set.Contains((point.X + 1, point.Z)))
                continue;
            if (set.Contains((point.X - 1, point.Z)))
                continue;
            if (set.Contains((point.X, point.Z + 1)))
                continue;
            if (set.Contains((point.X + 1, point.Z - 1)))
                continue;
            edges.Add(point);
        }
        return edges;
    }

    public void AddWalls(Room room, Points points)
    {

    }


}

public class Cell
{
    public Vector2 Index { get; set; }
    public int Size { get; set; }
}