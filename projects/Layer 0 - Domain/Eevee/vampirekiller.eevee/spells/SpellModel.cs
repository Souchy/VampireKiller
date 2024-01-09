using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.zones;

namespace VampireKiller.eevee.vampirekiller.eevee.spells;

public class SpellModel : Identifiable, IStatementContainer
{
    public ID entityUid { get; set; }
    /// <summary>
    /// TODO skill icon
    /// </summary>
    public string iconPath { get; set; }
    public List<SkillSkin> skins { get; set; } = new();

    public StatsDic stats = Register.Create<StatsDic>();
    public SmartList<IStatement> statements { get; set; } = SmartList<IStatement>.Create();

    // TODO spell range, use SpellRange stat always in circles anyway. Clamp raycastMouse to the range zone (ex: syndra's q)
    // So if you have a 0 range ability, it will always cast on top of yourself no matter your cursor
    // public IZone range { get; set; } = new Zone();

    public ICondition castCondition { get; set; }

    protected SpellModel() { }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// ?? what about different resources: mana, life, rage, bullets, arrows, consumables....
/// Maybe just dont have costs. Cooldowns & cast speed & charges could be enough.
/// Charges + cd already kinda act like a ressource that automatically recharges, which is nicer than having to buy ammo for example.
/// So maybe just have mana for some spells, rage for special stuff, charges for ammo....
/// </summary>
public class SpellBaseCostLife : StatInt { }
public class SpellBaseCostMana : StatInt { }
public class SpellBaseCostRage : StatInt { }
/// <summary>
/// Could use the same increase to apply to all resources? 
/// Don't forget we can have negative increase to reduce the cost. (100% + -30% = 70%)
/// </summary>
public class SpellIncreasedCost : StatInt { }

public class SpellTotalCostLife : StatIntTotal<SpellBaseCostLife, SpellIncreasedCost> { }
public class SpellTotalCostMana : StatIntTotal<SpellBaseCostMana, SpellIncreasedCost> { }
public class SpellTotalCostRage : StatIntTotal<SpellBaseCostRage, SpellIncreasedCost> { }

// Range
public class SpellBaseRange : StatDouble { }
public class SpellIncreasedRange : StatDouble { }
public class SpellTotalRange : StatDoubleTotal<SpellBaseRange, SpellIncreasedRange> { }

// Cooldown
public class SpellBaseCooldown : StatDouble { }
public class SpellIncreasedCooldownRecovery : StatDouble { }
public class SpellTotalCooldown : StatDoubleTotal<SpellBaseCooldown, SpellIncreasedCooldownRecovery>  {
    public override double value
    {
        get
        {
            double castPerSecond = 1 / totalBase;
            double improvedCastPerSecond = castPerSecond * (100 + totalIncrease) / 100;
            double improvedCastTime = 1 / improvedCastPerSecond;
            return improvedCastTime;
        }
        set { }
    }
}

// Charges
public class SpellAddMaxCharges : StatInt { }

/// <summary>
/// How long it takes to cast the skill at its baseline, ex: 0.6s
/// Number of casts per seconds = 1 / 0.6s = 1.66/s
/// </summary>
public class SpellBaseCastTime : StatDouble { }
/// <summary>
/// Increases cast speed, increasing the number of casts per second.
/// </summary>
public class SpellIncreasedCastSpeed : StatDouble { }
/// <summary>
/// Ex with 0.6s base time and 30% increased cast speed: 
/// Number of casts per seconds base = 1 / 0.6s = 1.66/s
/// Number of casts per seconds increased = 1.66/s * (100 + 30%) = 2.166/s 
/// Final cast time = 1 / 2.166 = 0.46s
/// </summary>
public class SpellTotalCastTime : StatDoubleTotal<SpellBaseCastTime, SpellIncreasedCastSpeed>
{
    // todo sum with player stats
    public override double value
    {
        get
        {
            double castPerSecond = 1 / totalBase;
            double improvedCastPerSecond = castPerSecond * (100 + totalIncrease) / 100;
            double improvedCastTime = 1 / improvedCastPerSecond;
            return improvedCastTime;
        }
        set { }
    }
}
