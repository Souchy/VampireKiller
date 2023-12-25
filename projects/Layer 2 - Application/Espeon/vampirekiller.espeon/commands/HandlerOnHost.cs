using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.communication.events;
using vampirekiller.logia.stub;
using vampirekiller.logia;
using vampirekiller.logia.commands;

namespace vampirekiller.espeon.commands;

public class HandlerOnHost : ICommandHandler<CommandHost>
{
    public void handle(CommandHost command)
    {
        EventBus.centralBus.publish(Events.EventNet, new EspeonNet()); //"espeon"); //Events.NetEspeon);
        Universe.fight = new StubFight();
    }
}
