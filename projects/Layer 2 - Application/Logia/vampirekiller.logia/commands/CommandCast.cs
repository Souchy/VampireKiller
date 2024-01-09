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
using VampireKiller.eevee.vampirekiller.eevee.spells;
using Json = vampirekiller.eevee.util.json.Json;

namespace vampirekiller.logia.commands;

public record struct CommandCast : ICommand
{
    public ID sourceCreature { get; set; }
    public ID? raycastEntity { get; set; }
    public Vector3 raycastMouse { get; set; }
    public ID skillInstanceId { get; set; }

    /// <summary>
    /// Pour la deserialisation
    /// </summary>
    public CommandCast() { }
    public CommandCast(CreatureInstance sourceCreature, Entity raycastEntity, Vector3 raycastMouse, SpellInstance skill)
       : this(sourceCreature.entityUid, raycastEntity.entityUid, raycastMouse, skill.entityUid) { }
    public CommandCast(ID sourceCreature, ID raycastEntity, Vector3 raycastMouse, ID skillInstanceId)
    {
        this.sourceCreature = sourceCreature;
        this.raycastEntity = raycastEntity;
        this.raycastMouse = raycastMouse;
        this.skillInstanceId = skillInstanceId;
    }

    public byte[] serialize()
    {
        var json = Json.serialize(this);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }
}
