using vampirekiller.eevee.stats.schemas.global;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace Eevee.vampirekiller.eevee.stats.schemas;

public class ProjectileStats
{
}


public class ProjectileAddCount : StatInt {}
public class ProjectileAddChain : StatInt {}
public class ProjectileAddFork : StatInt {}
public class ProjectileAddPierce : StatInt {}
public class ProjectileFireInCircle : StatBool { }
// Rain
public class ProjectileFireAsRain : StatBool { }
public class ProjectileRainBaseRadius : StatDouble { }
public class ProjectileRainTotalRadius : StatDoubleTotal<ProjectileRainBaseRadius, IncreasedAreaOfEffect> { }



public class ProjectileBaseSpeed : StatDouble {}
public class ProjectileIncreasedSpeed : StatDouble {}
public class ProjectileTotalSpeed : StatDoubleTotal<ProjectileBaseSpeed, ProjectileIncreasedSpeed> {}

