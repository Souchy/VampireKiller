﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;

namespace vampirekiller.logia.commands;

public record struct CommandHost : ICommand
{
    public byte[] serialize()
    {
        throw new NotImplementedException();
    }
}
