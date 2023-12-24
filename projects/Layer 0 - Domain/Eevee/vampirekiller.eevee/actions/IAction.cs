using Godot;
using Logia.vampirekiller.logia;
using Util.ecs;
using Util.entity;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee;
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
    public Entity getSourceEntity() => fight.entities.values.FirstOrDefault(c => c.entityUid == sourceEntity);
    public Entity getTargetEntity() => fight.entities.values.FirstOrDefault(c => c.entityUid == targetEntity);

    /// <summary>
    /// Contextual components like the procTrigger's current creature, item, status, ...
    /// </summary>
    public T? getContext<T>(string key) where T : class;
    public void setContext(string key, object value);
}

public abstract class Action : IAction
{
    public ID sourceEntity { get; set; }
    public Vector3 raycastPosition { get; set; }
    public ID targetEntity { get; set; }
    public IAction parent { get; set; }
    public HashSet<IAction> children { get; set; } = new();
    public Fight fight { get; set; } = Universe.fight;
    protected Dictionary<string, object> context { get; set; } = new();

    protected Action() { }
    public Action(IAction parent)
    {
        this.parent = parent;
        this.sourceEntity = parent.sourceEntity;
        this.raycastPosition = parent.raycastPosition;
        this.targetEntity = parent.targetEntity;
        parent.children.Add(this);
        foreach (var key in context.Keys)
        {
            setContext(key, parent.getContext<object>(key));
        }
    }
    public Entity getSourceEntity() => fight.entities.values.FirstOrDefault(c => c.entityUid == sourceEntity);
    public Entity getTargetEntity() => fight.entities.values.FirstOrDefault(c => c.entityUid == targetEntity);
    public void setContext(string key, object value)
    {
        context[key] = value;
    }
    public T? getContext<T>(string key) where T : class
    {
        if(context.ContainsKey(key))
            return context[key] as T;
        return default;
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
        foreach(var key in context.Keys)
        {
            copy.setContext(key, context[key]);
        }
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
    public const string creature = "trigger.creature";
    public const string projectile = "trigger.projectile";
    public const string status = "trigger.status";
    public const string item = "trigger.status";
    public TriggerType triggerType { get; }
    public CreatureInstance? getContextCreature();
    public ProjectileInstance? getContextProjectile();
    public Status? getContextStatus();
    public Item? getContextItem();
    public void setContextCreature(CreatureInstance creature);
    public void setContextProjectile(ProjectileInstance projectile);
    public void setContextStatus(Status status);
    public void setContextItem(Item item);
}
public abstract class ActionTrigger : Action, IActionTrigger
{
    public abstract TriggerType triggerType { get; }
    public CreatureInstance? getContextCreature()
        => getContext<CreatureInstance>(IActionTrigger.creature);
    public ProjectileInstance? getContextProjectile()
        => getContext<ProjectileInstance>(IActionTrigger.projectile);
    public Status? getContextStatus()
        => getContext<Status>(IActionTrigger.status);
    public Item? getContextItem()
        => getContext<Item>(IActionTrigger.item);
    public void setContextCreature(CreatureInstance creature)
        => setContext(IActionTrigger.creature, creature);
    public void setContextProjectile(ProjectileInstance projectile)
        => setContext(IActionTrigger.projectile, projectile);
    public void setContextStatus(Status status)
        => setContext(IActionTrigger.status, status);
    public void setContextItem(Item item)
        => setContext(IActionTrigger.item, item);

    public ActionTrigger() : base() { }
    public ActionTrigger(IAction parent) : base(parent) { }
}