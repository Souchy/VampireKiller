using System;
using System.Collections.Generic;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace VampireKiller.eevee.creature;

public class CreatureModel : Identifiable
{
    public ID entityUid { get; set; }

    public string meshScenePath;
    public string iconPath;

    public StatsDic baseStats = new();
    // public CreatureModelStatsDictionary stats { get; set; } = new();

    public CreatureInstance createInstance()
    {
        return null;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

}
