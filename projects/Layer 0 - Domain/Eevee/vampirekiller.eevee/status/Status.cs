

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
/// Time at which the status duration was created or refreshed 
/// The time of expiration = StatusDate + StatusDuration.
/// Used  for status/fx/proj
/// </summary>
//public class DurationStartDate : StatDate { }
///// <summary>
///// Actual current duration, calculated from TotalDuration and input here through the CreateStatusScript
///// Used  for status/fx/proj
///// Must be less than StatusMaxDuration.
///// </summary>
//public class CurrentDuration : StatDouble { }
/// <summary>
/// Time at which the status/fx/proj must end.
/// Can get refreshed
/// </summary>
public class SkillExpirationDate : StatDate { }
/// <summary>
/// Max duration set by the status model.
/// 0 or null means infinite.
/// </summary>
public class SkillMaxDuration : StatDouble { }
/// <summary>
/// Current Status stacks
/// </summary>
public class StatusStacks : StatInt { }
/// <summary>
/// Max stacks set by the status model.
/// </summary>
public class StatsMaxStacks : StatInt { }


/// <summary>
/// Base duration for statuses, fxnodes, projectilenodes...
/// </summary>
public class BaseDuration : StatDouble { }
/// <summary>
/// Increases skill durations
/// </summary>
public class IncreasedDuration : StatDouble { }
/// <summary>
/// Calculate increased duration to set into a status/fx/proj's SkillExpirationDate
/// </summary>
public class TotalDuration : StatDoubleTotal<BaseDuration, IncreasedDuration> { }
