using System.Numerics;
using vampirekiller.eevee.campaign.map;
using VampireKiller.eevee.creature;

namespace vampirekiller.eevee.campaign;

public class Campaign
{
    /// <summary>
    /// Map
    /// </summary>
    public Map Map { get; set; } = new();

    /// <summary>
    /// Gold might be managed only by the owner of the campaign
    /// </summary>
    public int Gold { get; set; }

    /// <summary>
    /// How the fuck do we do multiplayer campaign? Each player has their items, skills..
    /// How do each player chooses their correct creature when coming back to this campaign?
    /// Is the campaign just not playable if all players aren't there?
    /// Is it impossible to join a friend's campaign midway?
    /// 
    /// Another point is that creatures are supposed to be throwables. Aka you die and start over again.
    /// 
    /// I think a creature is tied to a campaign.
    /// 
    /// You can just other people's campaign with your own creatures, but if you die, you lose it. You can get PK'd that way.
    /// 
    /// 
    /// 
    /// </summary>
    public CreatureInstance PlayerCreature { get; set; }

    /// <summary>
    /// Current player position on the map [x = room, y = floor]
    /// </summary>
    public Vector2 CurrentRoom { get; set; } = new(0, 0);

    public CampaignSettings settings {  get; set; } = new();

}
