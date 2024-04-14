using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.util.json;

namespace vampirekiller.glaceon.configs;

public class ConfigGeneral : Config
{

    public string lastProfileUsed { get; set; } = "default";
    public List<string> profiles { get; set; } = new() { "default" };

}
