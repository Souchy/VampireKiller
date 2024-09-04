using Util.entity;
using vampirekiller.eevee.campaign.map;

namespace vampirekiller.eevee.campaign;

public class CampaignSettings
{
    public int StartGold { get; set; } = 0;
    public List<ID> StartItems { get; set; } = new();

    public MapGenerationSettings MapGenerationSettings { get; set; } = new();
}
