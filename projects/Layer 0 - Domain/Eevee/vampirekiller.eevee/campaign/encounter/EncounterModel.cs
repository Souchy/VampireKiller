using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.eevee.campaign.encounter;

public abstract class EncounterModel
{
    public string encounterNodeScenePath = "";

    public abstract void handleStart();
    public abstract void handleEnd();
}
