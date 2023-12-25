using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampirekiller.logia.net;

namespace vampirekiller.umbreon;

public class UmbreonCommandPublisher : ICommandPublisher
{
    private readonly ICommandManager _manager;
    public UmbreonCommandPublisher(ICommandManager manager)
    {
        _manager = manager;
    }

    public void publish<T>(T command) where T : ICommand
    {
        //if (command.preferOnline && Universe.isOnline)
        //{
        //    Umbreon.net.RpcServer(Rpcs.onPacketCommand, command.serialize());
        //}
        //else
        //{
            _manager.handle(command);
        //}
    }

    public async Task publishAsync<T>(T command) where T : ICommand
    {
        //if (command.preferOnline && Universe.isOnline)
        //{
        //    Umbreon.net.RpcServer(Rpcs.onPacketCommand, command.serialize());
        //}
        //else
        //{
            await _manager.handleAsync(command);
        //}
    }
}
