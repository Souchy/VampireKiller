using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampirekiller.logia.stub;
using vampirekiller.umbreon.commands;

namespace vampirekiller.umbreon;

public class UmbreonCommandManager : CommandManager
{
    public UmbreonCommandManager()
    {
        this.setHandler(new HandlerOnPlay());
        this.setHandler(new HandlerOnExitToMain());
    }
}
