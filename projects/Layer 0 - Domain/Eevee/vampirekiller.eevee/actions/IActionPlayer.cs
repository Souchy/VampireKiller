
using Util.entity;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.equipment;

namespace vampirekiller.eevee.actions;

// public interface IPlayerAction : IAction
// {
//     public CreatureInstance getSourceCreature() => fight.creatures.values.FirstOrDefault(c => c.entityUid == sourceEntity);
//     public CreatureInstance getRaycastCreature() => fight.creatures.values.FirstOrDefault(c => c.entityUid == targetEntity);
// }

public class ActionCastActive : Action, IActionTrigger
{
    public TriggerType triggerType => TriggerType.onCastActive;
    public ID activeItem { get; set; }

    public ActionCastActive() { }
    public ActionCastActive(ID activeItem) => this.activeItem = activeItem;

    public Item getItem() => fight.items.get(i => i.entityUid == activeItem);
    public CreatureInstance getSourceCreature() => fight.creatures.values.FirstOrDefault(c => c.entityUid == sourceEntity);
    public CreatureInstance getRaycastCreature() => fight.creatures.values.FirstOrDefault(c => c.entityUid == targetEntity);
    protected override IAction copyImplementation()
        => new ActionCastActive(activeItem);
}
