using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.json;

namespace vampirekiller.logia.generation;

public class AssetPack : Config
{
    public List<Asset> Assets { get; set; } = new();
}

public class Asset
{
    public string Path { get; set; }
    public HashSet<Tag> Tags { get; set; } = new();
}

public enum Tag
{
    Wall,
    Floor,
    Decoration,
    Collision,
    OnTheFloor,
    OnTheWall,
    Light
}