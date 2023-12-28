using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.statements.schemas;

/// <summary>
/// For Activation timers based on CastTime.
/// 
/// TODO: make a CreateCooldownTimerSchema class for cooldown timers
/// </summary>
public class CreateActivationTimerSchema : IStatementSchema, IStatementContainer
{
    /// <summary>
    /// Statements to activate on timer activation
    /// </summary>
    //public List<IStatement> activationStatements { get; set; } = new();
    public SmartList<IStatement> statements { get; set; } = SmartList<IStatement>.Create(); //= Register.Create<SmartList<IStatement>>();

    /// <summary>
    /// SpellBaseCastTime = activation time
    /// </summary>
    public StatsDic stats { get; set; } = Register.Create<StatsDic>();

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
