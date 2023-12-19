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

namespace VampireKiller.eevee.vampirekiller.eevee.zones;


public interface IZone
{
    /// <summary>
    /// 
    /// </summary>
    public ZoneType zoneType { get; set; }
    /// <summary>
    /// 1) x = lengthMin (forward length)
    /// 2) z = lengthMax (side length)
    /// 3) y = ring width: 0 = full shape, y = actual ring width
    /// </summary>
    public Vector3 size { get; set; }
    /// <summary>
    /// If we substract this zone from its parent and siblings
    /// </summary>
    public bool negative { get; set; }

    /// <summary>
    /// World origin: source or target of the spelleffect  <br></br>
    /// aka the zone Anchor in the world
    /// </summary>
    public ActorType worldOrigin { get; set; }
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
    public TargetSamplingType samplingType { get; set; }



    public float GetLengthForward() => size.X;
    public float GetLengthSide() => size.Z;
    public float GetRingWidth() => size.Y;

    /// <summary>
    /// Get the cells touched by this area at target point
    /// </summary>
    //public IArea getArea(IFight fight, IPosition targetCell);
    public IZone copy();
}