using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.eevee.enums;

/// <summary>
/// How to position a zone in the world.
/// </summary>
public enum ZoneOriginType
{
    /// <summary>
    /// Position the zone at the action source. <para></para>
    /// Always the source/caster entity (creature or projectile)
    /// </summary>
    Source,
    /// <summary>
    /// Position the zone at the action raycastEntity. <para></para>
    /// If from a skillcast: the raycast entity <br></br>
    /// If from a collision: the collidee entity
    /// </summary>
    Target,
    /// <summary>
    /// Position the zone at the raycast position <para></para>
    /// Raycast position from a skillcast
    /// </summary>
    Raycast
}
