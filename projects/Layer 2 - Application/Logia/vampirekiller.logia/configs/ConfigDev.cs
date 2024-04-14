using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.util.json;

namespace vampirekiller.logia.configs;

public class ConfigDev : Config
{
    public static ConfigDev Instance { get; set; }

    public ConfigDev()
    {
        Instance = this;
    }

    public string vampireAssetsPath { get; set; } = Path.GetFullPath("../../Layer 1 - Data/VampireAssets/");
    //public string entitiesPath { get; set; }


}
