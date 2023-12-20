using Godot;
using Logia.vampirekiller.logia;
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
    /// Creature casting the action
    /// </summary>
    public ID sourceCreature { get; set; }
    /// <summary>
    /// Mouse position projected to the 3d ground by raycast
    /// </summary>
    public Vector3 raycastPosition { get; set; }
    /// <summary>
    /// Creature targeted by mouse ray cast or by statement (ActionEffectTarget)
    /// </summary>
    public ID raycastCreature { get; set; }
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
    public T? getParent<T>() ;
}

public abstract class Action : IAction
{
    public ID sourceCreature { get; set; }
    public Vector3 raycastPosition { get; set; }
    public ID raycastCreature { get; set; }
    public IAction parent { get; set; }
    public HashSet<IAction> children { get; set; } = new();
    public Fight fight { get; set; } = Universe.fight;
    public CreatureInstance getSourceCreature() => fight.creatures.values.FirstOrDefault(c => c.entityUid == sourceCreature);
    public CreatureInstance getRaycastCreature() => fight.creatures.values.FirstOrDefault(c => c.entityUid == raycastCreature);
    
    protected Action() { }
    public Action(IAction parent) 
    {
        this.parent = parent;
        this.sourceCreature = parent.sourceCreature;
        this.raycastPosition = parent.raycastPosition;
        this.raycastCreature = parent.raycastCreature;
        parent.children.Add(this);
    }
    public IAction copy() {
        IAction copy = copyImplementation();
        copy.sourceCreature = this.sourceCreature;
        copy.raycastPosition = this.raycastPosition;
        copy.raycastCreature = this.raycastCreature;
        copy.parent = this.parent;
        foreach (var child in children)
            copy.children.Add(child);
        return copy;
    }
    public T? getParent<T>() {
        if(parent is null)
            return default;
        if(parent is T t)
            return t;
        return parent.getParent<T>();
    }
    protected abstract IAction copyImplementation();
}

public class ActionCastActive : Action
{
    public ID activeItem { get; set; }
    
    public ActionCastActive() {}

    public Item getItem() => fight.items.get(i => i.entityUid == activeItem);
    protected override IAction copyImplementation()
        => new ActionCastActive() {
            activeItem = this.activeItem
        };
}

public class ActionProcessTick : Action
{
    public double delta { get; set; }

    public ActionProcessTick() {}

    protected override IAction copyImplementation()
        => new ActionProcessTick() {
            delta = this.delta
        };
}

public class ActionStatement : Action
{
    public IStatement statement { get; set; }
    
    protected ActionStatement() { }
    public ActionStatement(IAction parent) : base(parent) { }

    protected override IAction copyImplementation()
        => new ActionStatement() {
            statement = this.statement
        };
}

public class ActionStatementZone : ActionStatement
{
    public IEnumerable<CreatureInstance>? targets { get; set; }
    
    protected ActionStatementZone() { }
    public ActionStatementZone(IAction parent) : base(parent) { }

    
    protected override IAction copyImplementation()
        => new ActionStatementZone() {
            statement = this.statement,
            targets = this.targets?.ToList()
        };
}

public class ActionStatementTarget : ActionStatementZone
{
    public CreatureInstance currentTarget { get; set; }

    protected ActionStatementTarget() { }
    public ActionStatementTarget(IAction parent) : base(parent) { }

    public ActionStatementZone getZoneParent() => (ActionStatementZone)parent;
    public IEnumerable<CreatureInstance>? getParentTargets() => getZoneParent().targets;
    public IStatement getParentStatement() => getZoneParent().statement;

    protected override IAction copyImplementation()
        => new ActionStatementTarget() {
            currentTarget = this.currentTarget
        };
}
