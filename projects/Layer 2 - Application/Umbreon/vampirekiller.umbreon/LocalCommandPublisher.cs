using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;

namespace Logia.vampirekiller.logia.commands;

public class LocalCommandPublisher : ICommandPublisher
{
    private readonly ICommandManager _manager;
    public LocalCommandPublisher(ICommandManager manager)
    {
        _manager = manager;
    }

    public void publish<T>(T command) where T : ICommand
        => _manager.handle(command);

    public async Task publishAsync<T>(T command) where T : ICommand
        => await _manager.handleAsync(command);

}
