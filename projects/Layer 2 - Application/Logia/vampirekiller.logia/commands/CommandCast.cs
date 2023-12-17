using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Util.communication.commands;
using VampireKiller.eevee.creature;

namespace vampirekiller.logia.commands;

public record struct CommandCast : ICommand
{
    public CreatureInstance originator { get; private set; }
    public Vector3 originatorFacing { get; private set; }
    // Would also contain the spell/ability being casted/used

    public CommandCast(CreatureInstance originator, Vector3 originatorFacing)
    {
        this.originator = originator;
        this.originatorFacing = originatorFacing;
    }

    public byte[] serialize()
    {
        throw new NotImplementedException();
    }
}
