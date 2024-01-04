using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.campaign.encounter;

namespace vampirekiller.eevee.campaign;

public class LevelModel
{
    public string name { get; set; } = "";
    public string levelBackgroundScenePath { get; set; } = "";

    public EncounterModelStart start = new EncounterModelStart();
    public EncounterModelFinish finish = new EncounterModelFinish();
    public EncounterModelFight[] fightModels = Array.Empty<EncounterModelFight>();
    public EncounterModelShop[] shopModels = Array.Empty<EncounterModelShop>();
    public EncounterModelEvent[] eventModels = Array.Empty<EncounterModelEvent>();

    // 15% shops, 15% events, 70% battles
    public int prcShops { get; set; } = 15;
    public int prcEncounters { get; set; } = 15;
}
