using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;

namespace vampirekiller.espeon.commands;

public record struct CommandProcessTick(double delta) : ICommand
{
    public byte[] serialize()
    {
        throw new NotImplementedException();
    }
}

public class ProcessHandler : ICommandHandler<CommandProcessTick>
{
    public void handle(CommandProcessTick t)
    {
        throw new NotImplementedException();
    }
}