using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;

namespace vampirekiller.eevee.campaign;

public class CampaignInstance : Identifiable
{
    private LevelInstance[] levels;

    public CampaignInstance() {
    }

    public ID entityUid { get; set; }



    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
