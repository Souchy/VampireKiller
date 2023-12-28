using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.ecs;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.logia.extensions;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.statements;

/// <summary>
///
/// </summary>
public class SpawnFxScript : IStatementScript
{
    public Type schemaType => typeof(SpawnFxSchema);

    public void apply(ActionStatementTarget action)
    {
        var props = action.statement.GetProperties<SpawnFxSchema>();

        //Entity spawnTarget = null;
        //if (props.followActor == ActorType.Source)
        //{
        //    spawnTarget = action.getSourceEntity();
        //}
        //else
        //if (props.followActor == ActorType.Target)
        //{
        //    spawnTarget = action.getTargetEntity();
        //}
        var status = action.getParent<ActionTrigger>()?.getContextStatus();
        var spawnTarget = action.currentTargetEntity;
        var spawnPos = action.currentTargetPos;

        EventBus.centralBus.publish(SpawnFxSchema.EventFx, props.scene, spawnPos, spawnTarget, props.follow, status); //spawnTarget, props.follow, status);

    }
}
