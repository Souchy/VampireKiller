using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampirekiller.logia.commands;

namespace vampirekiller.umbreon.commands;

public class HandlerOnHost : ICommandHandler<CommandHost>
{
    public void handle(CommandHost t)
    {
        Universe.container.RegisterUmbreon();
        Universe.container.GetInstance<ICommandManager>().handle(t);
    }
}
