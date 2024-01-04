using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.campaign.encounter;

public class EncounterModelShop
{
    public List<Item> availableItems;

    public StatInt availableGold; // To limit amount which can be sold to the shop
}
