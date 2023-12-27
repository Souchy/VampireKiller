using Util.ecs;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.eevee.actions;

/// <summary>
/// Leaf action
/// </summary>
public interface IActionStatement : IAction {
    public IStatement statement { get; set; }
}

/// <summary>
/// Leaf action
/// </summary>
public class ActionStatementTrigger : ActionTrigger, IActionTrigger
{
    public override TriggerType triggerType => TriggerType.onStatement;

    public ActionStatementTrigger(ActionStatementTarget parent, TriggerType triggerType) : base(parent) { }

    public ActionStatementTarget getParentStatement() => (ActionStatementTarget) parent;
    protected override IAction copyImplementation()
    {
        throw new NotImplementedException();
    }
}

public class ActionTriggerOnStatusAdd : ActionTrigger
{
    public override TriggerType triggerType => TriggerType.onStatusAdd;
    public Status status { get; set; }

    public ActionTriggerOnStatusAdd() : base() { }
    public ActionTriggerOnStatusAdd(IAction parent) : base(parent) { }
    protected override IAction copyImplementation()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Leaf action
/// </summary>
public class ActionStatementZone : Action, IActionStatement
{
    public IStatement statement { get; set; }
    /// <summary>
    /// Pour l'instant nullable pcq on peut avoir un statement qui n'a pas de zone (ex: single-point apply glyph)
    /// </summary>
    public IEnumerable<Entity>? targets { get; set; }

    protected ActionStatementZone() { }
    public ActionStatementZone(IAction parent) : base(parent) { 
        if(parent is IActionStatement actionStatement)
            this.statement = actionStatement.statement;
        if(parent is ActionStatementZone zone)
            this.targets = zone.targets;
    }
    protected ActionStatementZone(IAction parent, IStatement statement, IEnumerable<Entity>? targets) 
        : base(parent) { 
        this.statement = statement;
        this.targets = targets;
    }


    protected override IAction copyImplementation()
        => new ActionStatementZone()
        {
            statement = this.statement,
            targets = this.targets?.ToList()
        };
}

/// <summary>
/// Leaf action
/// </summary>
public class ActionStatementTarget : ActionStatementZone
{
    public Entity currentTarget { get; set; }

    protected ActionStatementTarget() { }
    public ActionStatementTarget(IAction parent) : base(parent) { 
        if(parent is ActionStatementTarget actionStatementTarget)
            this.currentTarget = actionStatementTarget.currentTarget;
    }
    public ActionStatementTarget(IAction parent, IStatement statement, IEnumerable<Entity>? targets, Entity currentTarget) 
        : base(parent, statement, targets) { 
        this.currentTarget = currentTarget;
    }


    public ActionStatementZone getZoneParent() => (ActionStatementZone)parent;
    public IEnumerable<Entity>? getParentTargets() => getZoneParent().targets;
    public IStatement getParentStatement() => getZoneParent().statement;

    protected override IAction copyImplementation()
        => new ActionStatementTarget() {
            statement = this.statement,
            targets = this.targets?.ToList(),
            currentTarget = this.currentTarget,
        };
}
