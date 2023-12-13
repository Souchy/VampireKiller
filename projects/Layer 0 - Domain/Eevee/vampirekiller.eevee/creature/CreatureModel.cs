﻿using System;
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

    /*
     TODO growth:
     {
        0: {
            "_type": "Linear" / "Logarithm"
            a: 1
            b: 10
        }
        10: {
            "_type": "Logarithm"
            a: 0
            power: 0
            b: 0
        }
     }
     */

    public CreatureInstance createInstance()
    {
        return null;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

}
