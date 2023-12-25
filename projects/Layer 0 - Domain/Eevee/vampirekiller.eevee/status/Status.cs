

using Util.entity;
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.statements.schemas;

/// <summary>
/// One way to sinchronize statuses & items over multiplayer might be to put them as an export on creatureNode, 
/// then we could reference them in the MultiSynchronizer.
/// Which means the UI would have to be based off the CreatureNode.statuses + CreatureNode.items instead of the CreatureInstance's
/// </summary>
public class Status : Identifiable, IStatementContainer
{
    public ID entityUid { get; set; }
    /// <summary>
    /// Can be a status model or a skill model
    /// </summary>
    public ID modelUid { get; set; }
    /// <summary>
    /// Set it in statuses schemas. -> used with AddStatus
    /// Otherwise CreateStatus uses the spell's icon
    /// </summary>
    public string iconPath { get; set; }
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