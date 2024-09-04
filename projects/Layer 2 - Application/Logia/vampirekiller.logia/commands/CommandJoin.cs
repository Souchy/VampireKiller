using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.json;
using vampirekiller.eevee.util.json;

namespace vampirekiller.logia.commands;

public class CommandJoin : ICommand
{
    // join different servers
    public string serverUrl { get; set; } = "localhost:7000";


    public byte[] serialize()
    {
        var json = Json.serialize(this);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }
}
