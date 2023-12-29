﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.entity;
using vampirekiller.eevee.util.json;

namespace vampierkiller.logia.commands;

public record struct CommandPlayGame : ICommand
{
    public byte[] serialize()
    {
        var json = Json.serialize(this);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }
}
