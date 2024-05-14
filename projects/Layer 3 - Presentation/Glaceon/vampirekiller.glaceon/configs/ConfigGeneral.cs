using System.Collections.Generic;
using Util.json;

namespace vampirekiller.glaceon.configs;

public class ConfigGeneral : Config
{

    public string lastProfileUsed { get; set; } = "default";
    public List<string> profiles { get; set; } = new() { "default" };

}
