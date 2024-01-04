using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Util.entity;
using vampirekiller.eevee.campaign.encounters;

namespace vampirekiller.eevee.campaign;

public class LevelInstance : Identifiable
{
    public ID entityUid { get; set; }

    public LevelModel levelModel { get; set; }

    public LevelInstance() { }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    private static List<EncounterInstance> generateEncounters(LevelModel model)
    {
        List<EncounterInstance> encounters = new List<EncounterInstance>();



        return encounters;
    }

    private static LevelPath[] generatePaths(EncounterInstance[] encounters)
    {
        return Array.Empty<LevelPath>();
    }

}