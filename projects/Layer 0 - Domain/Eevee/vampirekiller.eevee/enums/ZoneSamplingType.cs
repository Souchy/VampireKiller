
namespace vampirekiller.eevee.enums;

public enum ZoneSamplingType
{
    all,
    // careful, the origin can be placed on Actor.Source as well as Actor.Target
    closestToOrigin,
    furthestToOrigin,
    closestToSource,
    furthestToSource,
    random,
}
