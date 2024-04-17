using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.XmlParser;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using Util.structures;
using souchy.celebi.eevee.enums;
using Godot;
using vampirekiller.eevee.enums;

namespace VampireKiller.eevee.vampirekiller.eevee.zones;


public interface IZone
{
    /// <summary>
    /// 
    /// </summary>
    public ZoneType zoneType { get; set; }
    /// <summary>
    /// 1) x = lengthMin (forward length) (radius)
    /// 2) z = lengthMax (side length) (secondary length)
    /// 3) y = ring width: 0 = full shape, y = actual ring width = (outer_radius - inter_radius)
    /// </summary>
    public ZoneSize size { get; set; }
    /// <summary>
    /// If we substract this zone from its parent and siblings
    /// </summary>
    public bool negative { get; set; }

    /// <summary>
    /// World origin: source or target of the spelleffect  <br></br>
    /// aka the zone Anchor in the world
    /// </summary>
    public ZoneOriginType worldOrigin { get; set; } // ActorType
    /// <summary>
    /// offset from cast cell to local origin in the direction of the orientation
    /// </summary>
    public Vector3 worldOffset { get; set; }
    /// <summary>
    /// AKA the zone Anchor local to the aoe. <br></br>
    /// center for a circle/square <br></br>
    /// center for line perpendicular <br></br>
    /// bottom for line <br></br>
    /// top for line from source
    /// </summary>
    //public Direction9Type localOrigin { get; set; }
    /// <summary>
    /// Rotation of the zone around the localOrigin
    /// </summary>
    //public Rotation4Type rotation { get; set; }
    /// <summary>
    /// Wether the player can rotate the aoe manually or if it's fixed
    /// </summary>
    public bool canRotate { get; set; }
    /// <summary>
    /// This overrides a size variable like length/radius to make them equal to .distance(target,source) <br></br>
    /// Example: iop fracture, sacri fulguration, ... ça créé une zone partant du lanceur jusqu'à la cible <br></br>
    /// The index given is the {1,2,3} value overriden in the vector.
    /// </summary>
    public int sizeIndexExtendFromSource { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public SmartList<IZone> children { get; set; }
    /// <summary>
    /// Don't apply the zone if we dont have at least this amount of targets in it
    /// </summary>
    public int minSampleCount { get; set; }
    /// <summary>
    /// If value = 4, it will take a maximum of 4 targets among the cells in the area <br></br>
    /// If value = 1, it will only take the first target in the area. (Good for bouncing skills) <br></br>
    /// If value = int.MaxValue, then it can take infinite targets obviously. <br></br>
    /// Targets are chosen/sampled in accordance to the samplingType.
    /// </summary>
    public int maxSampleCount { get; set; }
    /// <summary>
    /// If value = random, the targets in the area will be chosen at random <br></br>
    /// If value = closestToOrigin, the targets in the area will be chosen by order of closest to origin. <br></br>
    /// This is also the order in which effects are applied to the targets. <br></br>
    /// This respects maxSampleCount.
    /// </summary>
    public ZoneSamplingType samplingType { get; set; }


    public float GetLengthForward() => size.radius;
    public float GetLengthSide() => size.sideRadius;
    public float GetRingWidth() => size.radius - size.ringWidth;

    /// <summary>
    /// Get the cells touched by this area at target point
    /// </summary>
    //public IArea getArea(IFight fight, IPosition targetCell);
    public IZone copy();
}

public class Zone : IZone
{
    public ZoneType zoneType { get; set; } = ZoneType.point;
    public ZoneSize size { get; set; } = ZoneSize.Zone0;
    public bool negative { get; set; }
    public ZoneOriginType worldOrigin { get; set; } = ZoneOriginType.Target;
    public Vector3 worldOffset { get; set; }
    public bool canRotate { get; set; }
    public int sizeIndexExtendFromSource { get; set; }
    public SmartList<IZone> children { get; set; }
    public int minSampleCount { get; set; }
    public int maxSampleCount { get; set; }
    public ZoneSamplingType samplingType { get; set; } = ZoneSamplingType.all;

    public IZone copy()
    {
        throw new NotImplementedException();
    }
}

public struct ZoneSize
{
    public static readonly ZoneSize Zone0 = new();
    /// <summary>
    /// Also known as forwardLength
    /// </summary>
    public float radius;
    /// <summary>
    /// Also known as sideLength
    /// Example: rectangle side
    /// </summary>
    public float sideRadius;
    /// <summary>
    /// Ring width = the hit zone.
    /// innerRadius = radius - ringWidth.
    /// If ring width = 0, then the aoe is full.
    /// </summary>
    public float ringWidth;
    public ZoneSize(float radius, float sideRadius = 0, float ringWidth = 0)
    {
        this.radius = radius;
        this.sideRadius = sideRadius;
        this.ringWidth = ringWidth;
    }
}

