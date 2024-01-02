using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.stats.schemas.resources;

// Life
public class CreatureBaseLife : StatInt { }
public class CreatureBaseLifeMax : StatInt { }
public class CreatureIncreasedLife : StatInt { }
public class CreatureIncreasedLifeMax : StatInt { }
/// <summary>
/// Used for fight additions (damage, heal)
/// </summary>
public class CreatureAddedLife : StatInt { }
/// <summary>
/// used for fight additions (erosion, buff)
/// </summary>
public class CreatureAddedLifeMax : StatInt { }

// Total life including fight
public class CreatureTotalLife : StatIntTotalFight<CreatureBaseLife, CreatureIncreasedLife, CreatureAddedLife> { }
public class CreatureTotalLifeMax : StatIntTotalFight<CreatureBaseLifeMax, CreatureIncreasedLifeMax, CreatureAddedLifeMax> { }

// Regen
public class CreatureBaseLifeRegen : StatDouble { }
public class CreatureIncreasedLifeRegen : StatDouble { }
public class CreatureTotalLifeRegen : StatDoubleTotal<CreatureBaseLifeRegen, CreatureIncreasedLifeRegen> { }