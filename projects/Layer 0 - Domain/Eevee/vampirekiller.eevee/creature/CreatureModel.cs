using System;
using System.Collections.Generic;
using Util.entity;
using vampirekiller.eevee.creature;
using vampirekiller.eevee.stats.schemas.resources;
using vampirekiller.eevee.util;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace VampireKiller.eevee.creature;

public class CreatureModel : Identifiable
{
    public ID entityUid { get; set; }
    public List <CreatureSkin> skins { get; set; } = new();
    public LootTable LootTable { get; set; }
    public StatsDic baseStats { get; set; } = Register.Create<StatsDic>();

    protected CreatureModel()
    {
        baseStats.set(Register.Create<CreatureBaseLife>());
        baseStats.set(Register.Create<CreatureBaseLifeMax>());
        baseStats.set(Register.Create<CreatureIncreasedLife>());
        baseStats.set(Register.Create<CreatureIncreasedLifeMax>());
    }

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

    public void Dispose()
    {
        throw new NotImplementedException();
    }

}
