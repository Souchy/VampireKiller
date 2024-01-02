using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.stats.schemas.resources;

// Mana
public class CreatureBaseMana : StatInt { }
public class CreatureBaseManaMax : StatInt { }
public class CreatureIncreaseMana : StatInt { }
public class CreatureIncreaseManaMax : StatInt { }
/// <summary>
/// Used for fight additions (damage, heal)
/// </summary>
public class CreatureAddedMana : StatInt { }
/// <summary>
/// used for fight additions (erosion, buff)
/// </summary>
public class CreatureAddedManaMax : StatInt { }

// Total Mana including fight
public class CreatureTotalMana : StatIntTotalFight<CreatureBaseMana, CreatureIncreaseMana, CreatureAddedMana> { }
public class CreatureTotalManaMax : StatIntTotalFight<CreatureBaseManaMax, CreatureIncreaseManaMax, CreatureAddedManaMax> { }

// Regen
public class CreatureBaseManaRegen : StatDouble { }
public class CreatureIncreasedManaRegen : StatDouble { }
public class CreatureTotalManaRegen : StatDoubleTotal<CreatureBaseManaRegen, CreatureIncreasedManaRegen> { }