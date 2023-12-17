using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;

namespace Logia.vampirekiller.logia.commands;

public class RemoteCommandPublisher : ICommandPublisher
{
    public RemoteCommandPublisher()
    {
    }

    public void publish<T>(T command) where T : ICommand
        => Universe.net.RpcServer("onPacketCommand", command.serialize());

    public async Task publishAsync<T>(T command) where T : ICommand
        => publish(command);

}
