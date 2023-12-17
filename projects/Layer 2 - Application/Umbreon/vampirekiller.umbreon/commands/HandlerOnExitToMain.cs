using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampirekiller.logia.commands;

namespace vampirekiller.umbreon.commands;

public class HandlerOnExitToMain : ICommandHandler<CommandExitToMain>
{
    public void handle(CommandExitToMain t)
    {
        Universe.fight.Dispose();
        Universe.fight = null;
    }
}
