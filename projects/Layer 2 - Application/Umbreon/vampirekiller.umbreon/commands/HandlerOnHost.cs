using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.communication.events;
using vampirekiller.espeon;
using vampirekiller.logia;
using vampirekiller.logia.commands;
using vampirekiller.logia.stub;

namespace vampirekiller.umbreon.commands;

public class HandlerOnHost : ICommandHandler<CommandHost>
{
    public void handle(CommandHost command)
    {
        Universe.container.RegisterEspeon();
        Universe.container.GetInstance<ICommandManager>().handle(command);
    }
}
