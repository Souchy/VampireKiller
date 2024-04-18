using Godot;
using VampireKiller.eevee.vampirekiller.eevee.zones;

namespace vampirekiller.eevee.zones;

public static class Rotation4Type
{
    public static readonly Vector2 top = new Vector2(0, 0);
    public static readonly Vector2 right = new Vector2(0, -90);
    public static readonly Vector2 bottom = new Vector2(0, 180);
    public static readonly Vector2 left = new Vector2(0, 90);
}

//public class Aaa<T>
//{
//    public T[][] cells;
//    public int width => cells.Length;
//    public int height => cells[0].Length;
//    //publi
//    public void rotate(float unit)
//    {
//        float angle = (float)(unit * Math.PI / 180);
//        double cos = Math.Cos(angle);
//        double sin = Math.Sin(angle);
//        for(int i = 0; i < cells.Length; i++)
//        {

//        }
//    }
//    public void Copy()
//    {
//        Aaa<T> copy = new();
//        copy.cells = new T[width][];
//        cells.CopyTo(copy.cells, 0);
//    }
//}

//public class GridPoints : HashSet<(float X, float Y, float Z)>
//{
//    public void rotate(float unit)
//    {
//        float angle = (float)(unit * Math.PI / 180);

//        for (int i = 0; i < this.Count; i++)
//        {
//            //var p = this[i];
//            //float xb = (float)Math.Round(p.X * Math.Cos(angle) - p.Z * Math.Sin(angle));
//            //float zb = (float)Math.Round(p.Z * Math.Cos(angle) + p.X * Math.Sin(angle));
//            //this[i] = new Vector3(xb, p.Y, zb);
//        }
//    }
//}

public static class PointsExtensions
{
    public static bool isEdge(this Vector3[][] grid, Vector3 v)
    {
        var left = grid[0][0];
        return false;
    }
}

/// <summary>
/// TODO: Convert to HashSet ? 
/// </summary>
public class Points : List<Vector3>
{
    public HashSet<(float x, float z)> toSet()
    {
        return this.Select(p => (p.X, p.Z)).ToHashSet();
        //return this.ToHashSet(p => ((int) p.X, (int) p.Z));
    }

    /// <summary>
    /// Grid[x][z]
    /// </summary>
    public Vector3[][] toGrid()
    {
        var bounds = boundingSize();
        var width = (int) (bounds.max.X - bounds.min.X);
        var depth = (int) (bounds.max.Z - bounds.min.Z);

        //var table = new Dictionary<(float, float), bool>();
        //for(var p in this)
        //{
        //    table.Add()

        var dic = this.ToDictionary(p => ((int) p.X, (int) p.Z));

        Vector3[][] grid = new Vector3[width][];
        for (int i = 0; i < this.Count; i++)
        {
            var p = this[i];
            if (grid[(int) p.X] == null)
                grid[(int) p.X] = new Vector3[depth];
            grid[(int) p.X][(int) p.Z] = p;
        }
        return grid;
    }

    public bool isEdge(Vector3 v)
    {
        int count = 0;
        foreach (var p in this)
            if ((p - v).Length() == 1)
                count++;
        return count < 4;
    }

    //public IZone zone { get; set; }
    //public Points(IZone zone) => this.zone = zone;
    /// <summary>
    /// rotate around the anchor
    /// </summary>
    /// <returns></returns>
    public Points rotate(Vector2 rotation)
    {
        var unit = rotation.Y;
        float angle = (float) (unit * Math.PI / 180);
        for (int i = 0; i < this.Count; i++)
        {
            var p = this[i];
            float xb = (float) Math.Round(p.X * Math.Cos(angle) - p.Z * Math.Sin(angle));
            float zb = (float) Math.Round(p.Z * Math.Cos(angle) + p.X * Math.Sin(angle));
            this[i] = new Vector3(xb, p.Y, zb);
        }
        return this;
    }
    //public Points anchor()
    //{
    //    switch (zone.zoneType.value)
    //    {
    //        case ZoneType.point:
    //            return this;
    //        case ZoneType.line:
    //        case ZoneType.diagonal:
    //            return Anchoring.anchorLine(this);
    //        case ZoneType.crossHalf:
    //        case ZoneType.circleHalf:
    //        case ZoneType.squareHalf:
    //        case ZoneType.ellipseHalf:
    //            return Anchoring.anchorFormHalf(this);
    //        case ZoneType.cross:
    //        case ZoneType.xcross:
    //        case ZoneType.star:
    //        case ZoneType.circle:
    //        case ZoneType.square:
    //        case ZoneType.rectangle:
    //        case ZoneType.ellipse:
    //            return Anchoring.anchorForm(this);
    //            //default:
    //            //    throw new Exception("Invalid zonetype: " + zone.zoneType.value);
    //    }
    //    return this;
    //}
    //public Points offset() => offset(zone.worldOffset.X, zone.worldOffset.Z);
    public Points offset(float x, float z)
    {
        for (int i = 0; i < Count; i++)
            this[i] += new Vector3(x, 0, z);
        return this;
    }
    public Points Add(Points points2)
    {
        foreach (var p in points2)
            Add(p);
        return this;
    }
    public new List<Vector3> AddRange(IEnumerable<Vector3> points2)
    {
        foreach (var p in points2)
            Add(p);
        return this;
    }
    public new void Add(Vector3 p)
    {
        if (!this.Any(v => v == p))
            base.Add(p);
    }
    public float minX() => this.Min(v => v.X);
    public float minZ() => this.Min(v => v.Z);
    public float maxX() => this.Max(v => v.X);
    public float maxZ() => this.Max(v => v.Z);
    public (Vector3 min, Vector3 max) boundingSize()
    {
        float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;
        foreach (var v in this)
        {
            if (v.X < minX) minX = v.X;
            if (v.Y < minY) minY = v.Y;
            if (v.Z < minZ) minZ = v.Z;
            if (v.X > maxX) maxX = v.X;
            if (v.Y > maxY) maxY = v.Y;
            if (v.Z > maxZ) maxZ = v.Z;
        }
        return (new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
    }
    public float sizeX() => maxX() - minX() + 1;
    public float sizeZ() => maxZ() - minZ() + 1;
    public IArea projectToBoard()
    {
        throw new Exception();
    }
    public override string ToString()
    {
        return "[" + string.Join(", ", this) + "]";
    }
}
