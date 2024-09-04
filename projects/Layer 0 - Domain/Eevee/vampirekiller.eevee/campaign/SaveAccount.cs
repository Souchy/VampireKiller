using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.eevee.campaign;

public class SaveAccount
{
    public const int saveFormat = 1;

    public string SteamId { get; set; }

    public List<Campaign> Campaigns { get; set; } = new();

    public List<object> unlockedThings {  get; set; } = new();

}
