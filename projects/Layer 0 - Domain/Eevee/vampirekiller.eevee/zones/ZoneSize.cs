using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.eevee.zones;

public abstract class IZoneSize
{
    public Vector3 sizeParams;
    public float ringWidth { get => sizeParams.y; set => sizeParams.y = value; }
}
public class ZoneSizePoint : IZoneSize
{
}
// line, diagonal
public class ZoneSizeLength : IZoneSize
{
    public float length { get => sizeParams.X; set => sizeParams.X = value; }
}
// circle O, square [], halfcircle /_\, v  /\
public class ZoneSizeRadius : IZoneSize
{
    public float radius { get => sizeParams.X; set => sizeParams.X = value; }
}
// cross, xcross, star, rectangle, ellipse, ellipseHalf
public class ZoneSizeRadius2 : IZoneSize
{
    public float radiusForward { get => sizeParams.X; set => sizeParams.X = value; }
    public float radiusSide { get => sizeParams.Z; set => sizeParams.Z = value; }
}