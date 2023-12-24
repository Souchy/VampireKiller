

using Util.entity;
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.statements.schemas;

public class Status : Identifiable, IStatementContainer
{
    public ID entityUid { get; set; }
    public SmartList<IStatement> statements { get; set; } = SmartList<IStatement>.Create();
    /// <summary>
    /// Model stats:
    /// - maxDuration
    /// - maxStacks
    /// - mergeStrategy (not a stat but a property)
    /// 
    /// Instance stats:
    /// - remainingDuration
    /// - stacks
    /// </summary>
    public StatsDic stats { get; set; } = Register.Create<StatsDic>();

    private Status() { }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Current Status stacks
/// </summary>
public class StatusStacks : StatInt { }
/// <summary>
/// Max stacks set by the status model.
/// </summary>
public class StatusMaxStacks : StatInt { }

/// <summary>
/// Can we unbewitch the status
/// </summary>
public class StatusUnbewitchable : StatBool { }