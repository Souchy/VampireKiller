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
    public int playerId { get; set; } = -1;
    //public CreatureInstance source { get; private set; }
    public Vector3 raycastMouse { get; private set; }
    // Would also contain the spell/ability being casted/used
    public int activeSlot = 0;

    public CommandCast(int playerId, Vector3 raycastMouse, int activeSlot) //CreatureInstance source)
    {
        //this.source = source;
        this.playerId = playerId;
        this.raycastMouse = raycastMouse;
        this.activeSlot = activeSlot;
    }

    public byte[] serialize()
    {
        throw new NotImplementedException();
    }
}
