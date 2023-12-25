using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.communication.events;
using vampirekiller.logia;
using vampirekiller.logia.commands;
using vampirekiller.logia.net;

namespace vampirekiller.umbreon.commands;

public class HandlerOnJoin : ICommandHandler<CommandJoin>
{
    public void handle(CommandJoin command)
    {
        var net = new UmbreonNet();
        Umbreon.net = net;
        EventBus.centralBus.publish(Events.EventNet, net); //"umbreon");
        EventBus.centralBus.publish(Events.EventChangeScene, Events.SceneFight);

        Umbreon.net.RpcId(1, Rpcs.onPacketCommand, command.serialize());
    }
}
