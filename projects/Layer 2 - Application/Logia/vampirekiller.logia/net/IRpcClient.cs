using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.logia.net;

public interface IRpcClient
{
    public static IRpcClient Instance { get; set; }
    public void sendCommand(params Variant[] args);
}
