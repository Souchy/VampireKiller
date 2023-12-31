
using Util.ecs;
using vampirekiller.eevee.triggers;

namespace vampirekiller.eevee.actions;

public interface IEngineAction : IActionTrigger
{

}

/// <summary>
/// Root action
/// </summary>
public class ActionCollision : ActionTrigger, IEngineAction
{
    public override TriggerType triggerType => TriggerType.onCollision;

    public ActionCollision() { }
    public ActionCollision(Entity collider, Entity collidee)
    {
        this.sourceEntity = collider.entityUid;
        this.raycastEntity = collidee.entityUid;
    }
    protected override IAction copyImplementation()
        => new ActionCollision();

}

/// <summary>
/// Root action
/// </summary>
public class ActionProcessTick : ActionTrigger, IEngineAction
{
    public override TriggerType triggerType => TriggerType.onProcess;
    public double delta { get; set; }

    public ActionProcessTick(double delta) => this.delta = delta;

    protected override IAction copyImplementation()
        => new ActionProcessTick(delta);
}
