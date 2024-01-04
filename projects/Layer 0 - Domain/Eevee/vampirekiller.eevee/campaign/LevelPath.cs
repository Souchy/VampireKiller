using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.campaign.encounters;

namespace vampirekiller.eevee.campaign;

public class LevelPath
{
    public EncounterInstance from { get; }
    public EncounterInstance to { get; } 

    public LevelPath(EncounterInstance from, EncounterInstance to)
    {
        this.from = from;
        this.to = to;
    }
}
