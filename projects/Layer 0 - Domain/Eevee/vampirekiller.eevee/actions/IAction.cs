using Godot;
using Logia.vampirekiller.logia;
using Util.ecs;
using Util.entity;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.eevee.actions;

public interface IAction
{
    /// <summary>
    /// Creature/Projectile casting the action
    /// </summary>
    public ID sourceEntity { get; set; }
    /// <summary>
    /// Creature/Projectile targeted by mouse ray cast or by statement (ActionEffectTarget)
    /// </summary>
    public ID targetEntity { get; set; }
    /// <summary>
    /// Mouse position projected to the 3d ground by raycast
    /// </summary>
    public Vector3 raycastPosition { get; set; }
    /// <summary>
    /// Current Fight, set after reception
    /// </summary>
    public Fight fight { get; set; }
    /// <summary>
    /// Parent action
    /// </summary>
    public IAction parent { get; set; }
    /// <summary>
    /// Children actions
    /// </summary>
    public HashSet<IAction> children { get; set; }
    /// <summary>
    /// Get the closest parent of type T
    /// </summary>
    public T? getParent<T>();
}

public abstract class Action : IAction
{
    public ID sourceEntity { get; set; }
    public Vector3 raycastPosition { get; set; }
    public ID targetEntity { get; set; }
    public IAction parent { get; set; }
    public HashSet<IAction> children { get; set; } = new();
    public Fight fight { get; set; } = Universe.fight;

    public Entity getSourceEntity() => fight.entities.values.FirstOrDefault(c => c.entityUid == sourceEntity);
    public Entity getTargetEntity() => fight.entities.values.FirstOrDefault(c => c.entityUid == targetEntity);
    protected Action() { }
    public Action(IAction parent)
    {
        this.parent = parent;
        this.sourceEntity = parent.sourceEntity;
        this.raycastPosition = parent.raycastPosition;
        this.targetEntity = parent.targetEntity;
        parent.children.Add(this);
    }
    public IAction copy()
    {
        IAction copy = copyImplementation();
        copy.sourceEntity = this.sourceEntity;
        copy.raycastPosition = this.raycastPosition;
        copy.targetEntity = this.targetEntity;
        copy.parent = this.parent;
        foreach (var child in children)
            copy.children.Add(child);
        return copy;
    }
    public T? getParent<T>()
    {
        if (parent is null)
            return default;
        if (parent is T t)
            return t;
        return parent.getParent<T>();
    }
    protected abstract IAction copyImplementation();
}

public interface IActionTrigger : IAction {
    public TriggerType triggerType { get; }
}
