using System.Numerics;
using vampirekiller.eevee.campaign.map;
using VampireKiller.eevee.creature;

namespace vampirekiller.eevee.campaign;

public class Campaign
{
    public CampaignSettings settings {  get; set; } = new();

    public int seed {  get; init; }

    public string Name { get; set; }

    /// <summary>
    /// Current floor level on the map
    /// </summary>
    public int CurrentFloor { get; set; }
    /// <summary>
    /// CurrentRoom set when only you enter it.
    /// You can stay & save in a room after you completed it. 
    /// Then can choose the next floor's room.
    /// </summary>
    public int CurrentRoom { get; set; }

    /// <summary>
    /// Gold might be managed only by the owner of the campaign
    /// </summary>
    public int Gold { get; set; }

    /// <summary>
    /// Map containing all the floors and rooms
    /// </summary>
    public Map Map { get; set; } = new();

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
    /// You can just join other people's campaign with your own creatures, but if you die, you lose it. You can get PK'd that way.
    /// 
    /// ---
    /// 
    /// Maybe can't join campaigns that are 10+ levels (floors) apart from yours
    /// 
    /// </summary>
    public CreatureInstance PlayerCreature { get; set; }

    /// <summary>
    /// To differentiate depth ladders.
    /// People who duo queued or grinded levels in lower level campaigns have an advantage.
    /// </summary>
    public bool PlayedMultiplayer {  get; set; }

}
