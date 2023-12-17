using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.entity;

namespace vampierkiller.logia.commands;

public record struct CommandPlayGame : ICommand
{
    public byte[] serialize()
    {
        throw new NotImplementedException();
    }
}
