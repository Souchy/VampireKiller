using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.ecs;
using Util.entity;
using vampirekiller.eevee.util.json;
using VampireKiller.eevee.creature;
using Json = vampirekiller.eevee.util.json.Json;

namespace vampirekiller.logia.commands;

public record struct CommandCast : ICommand
{
    public int playerId { get; set; } = -1;
    public ID? raycastEntity { get; set; }
    public Vector3 raycastMouse { get; private set; }
    // Would also contain the spell/ability being casted/used
    public int activeSlot = 0;

    public CommandCast(int playerId, Entity raycastEntity, Vector3 raycastMouse, int activeSlot) //CreatureInstance source)
    {
        //this.source = source;
        this.playerId = playerId;
        this.raycastEntity = raycastEntity?.entityUid;
        this.raycastMouse = raycastMouse;
        this.activeSlot = activeSlot;
    }

    public byte[] serialize()
    {
        var json = Json.serialize(this);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }
}
