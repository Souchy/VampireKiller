using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.stats.schemas.resources;

// Rage
public class CreatureBaseRage : StatInt { }
public class CreatureBaseRageMax : StatInt { }
public class CreatureIncreaseRage : StatInt { }
public class CreatureIncreaseRageMax : StatInt { }
/// <summary>
/// Used for fight additions (damage, heal)
/// </summary>
public class CreatureAddedRage : StatInt { }
/// <summary>
/// used for fight additions (erosion, buff)
/// </summary>
public class CreatureAddedRageMax : StatInt { }

// Total Rage including fight
public class CreatureTotalRage : StatIntTotalFight<CreatureBaseRage, CreatureIncreaseRage, CreatureAddedRage> { }
public class CreatureTotalRageMax : StatIntTotalFight<CreatureBaseRageMax, CreatureIncreaseRageMax, CreatureAddedRageMax> { }

// Regen
public class CreatureBaseRageRegen : StatDouble { }
public class CreatureIncreasedRageRegen : StatDouble { }
public class CreatureTotalRageRegen : StatDoubleTotal<CreatureBaseRageRegen, CreatureIncreasedRageRegen> { }