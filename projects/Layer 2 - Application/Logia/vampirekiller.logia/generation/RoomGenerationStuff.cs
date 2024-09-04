using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.logia.generation;

/*
 * Collections:
 *      MeshLibrary containing multiple variant models, hit 'randomize' on it to swap to a random model. Like PoE hideout decorations have multiple variants.
 *          - Godot mod: Show item name next to the item index menu
 *      
 *      Create 1 library per PropType * per Biome
 *      Ex: 
 *          - DungeonWalls, DungeonFloors, DungeonFloorDecors, 
 *          - RomeFloors, 
 *          - MansionWalls, 
 *      
 */

public partial class DbLibrary
{
    public List<PropRes> Models;
    public List<object> Materials;
}

public partial class DbTexture
{
    public string path;
    public string use;
}

public class DbModelCollection
{
    public string name;
    public List<PropRes> Models;
}

[GlobalClass]
public partial class PropRes : Resource
{
    [Export]
    public Resource Resource { get; set; }
    [Export]
    public Area3D CollisionBox { get; set; }
    [Export]
    public Array<PropType> PropTypes { get; set; }
    [Export(PropertyHint.Range, "0,100")]
    public int ChanceToPlaceAgainstWall { get; set; } = 0;
    [Export(PropertyHint.Range, "0,100")]
    public int PlacementHeight { get; set; } = 0;
    [Export(PropertyHint.Range, "0,100")]
    public int PlacementHeightRandomness { get; set; } = 0;
    /// <summary>
    /// Ex: walls, roofs, floors, carpets, paintings, glass, dirt
    /// </summary>
    [Export]
    public Resource Texture {  get; set; }

    public bool RandomXRotation { get; set; } = false;
    public bool RandomYRotation { get; set; } = false;
    public bool RandomZRotation { get; set; } = false;


    public bool Collides => CollisionBox != null;
}

[GlobalClass]
public partial class BiomeTileset : Resource
{
    public Array<Resource> Lights { get; set; } = new();
    public Array<Resource> FloorDecors { get; set; } = new();
}

public enum PropType
{
    Barrier,
    Wall,
    FloorTile,
    Door,
    Window,

    LightWall,
    LightCeiling,
    LightFloor,
    LightFurniture,

    DecorFloor,
    DecorWall,

}