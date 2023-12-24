using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.stats.schemas.skill;

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
/// Base duration for statuses, fxnodes, projectilenodes...
/// </summary>
public class SkillBaseDuration : StatDouble { }
/// <summary>
/// Increases skill durations
/// </summary>
public class SkillIncreasedDuration : StatDouble { }
/// <summary>
/// Calculate increased duration to set into a status/fx/proj's SkillExpirationDate
/// </summary>
public class SkillTotalDuration : StatDoubleTotal<SkillBaseDuration, SkillIncreasedDuration> { }
